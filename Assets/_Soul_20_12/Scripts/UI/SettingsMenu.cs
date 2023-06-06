using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class SettingsMenu : MonoBehaviour
{
    [Header("space between menu items")]
    [SerializeField] Vector2 spacing;

    [Space]
    [Header("Main button rotation")]
    [SerializeField] float rotationDuration;
    [SerializeField] Ease rotationEase;

    [Space]
    [Header("Animation")]
    [SerializeField] float expandDuration;
    [SerializeField] float collapseDuration;
    [SerializeField] Ease expandEase;
    [SerializeField] Ease collapseEase;

    [Space]
    [Header("Fading")]
    [SerializeField] float expandFadeDuration;
    [SerializeField] float collapseFadeDuration;

    Button mainButton;
    SettingsMenuItem[] menuItems;

    //is menu opened or not
    bool isExpanded = false;

    Vector2 mainButtonPosition;
    int itemsCount;

    [SerializeField] Button musicButton;
    [SerializeField] Button soundButton;

    void Start()
    {
        musicButton.onClick.AddListener(OnOffMusic);
        soundButton.onClick.AddListener(OnOffSound);

        //add all the items to the menuItems array
        itemsCount = transform.childCount - 1;
        menuItems = new SettingsMenuItem[itemsCount];
        for (int i = 0; i < itemsCount; i++)
        {
            // +1 to ignore the main button
            menuItems[i] = transform.GetChild(i + 1).GetComponent<SettingsMenuItem>();
        }

        mainButton = transform.GetChild(0).GetComponent<Button>();
        mainButton.onClick.AddListener(ToggleMenu);
        //SetAsLastSibling () to make sure that the main button will be always at the top layer
        mainButton.transform.SetAsLastSibling();

        mainButtonPosition = mainButton.GetComponent<RectTransform>().anchoredPosition;

        //set all menu items position to mainButtonPosition
        ResetPositions();
    }

    void ResetPositions()
    {
        for (int i = 0; i < itemsCount; i++)
        {
            menuItems[i].rectTrans.anchoredPosition = mainButtonPosition;
        }
    }

    void ToggleMenu()
    {
        isExpanded = !isExpanded;

        if (isExpanded)
        {
            //menu opened
            for (int i = 0; i < itemsCount; i++)
            {
                menuItems[i].rectTrans.DOAnchorPos(mainButtonPosition + spacing * (i + 1), expandDuration).SetEase(expandEase);
                //Fade to alpha=1 starting from alpha=0 immediately
                menuItems[i].img.DOFade(1f, expandFadeDuration).From(0f);
            }

            SetupButton();
        }
        else
        {
            //menu closed
            for (int i = 0; i < itemsCount; i++)
            {
                menuItems[i].rectTrans.DOAnchorPos(mainButtonPosition, collapseDuration).SetEase(collapseEase);
                //Fade to alpha=0
                menuItems[i].img.DOFade(0f, collapseFadeDuration);
            }
        }

        //rotate main button arround Z axis by 180 degree starting from 0
        mainButton.transform
              .DORotate(Vector3.forward * 180f, rotationDuration)
              .From(Vector3.zero)
              .SetEase(rotationEase);
    }

    [SerializeField] List<Sprite> soundSprite;
    [SerializeField] List<Sprite> musicSprites;

    void SetupButton()
    {
        if (PlayerPrefs.GetInt("music") == 0) //if is on
        {
            musicButton.image.sprite = musicSprites[0];
        }
        else // if is off
        {
            musicButton.image.sprite = musicSprites[1];
        }

        if (PlayerPrefs.GetInt("sound") == 0) //if is on
        {
            soundButton.image.sprite = soundSprite[0];
        }
        else //if is off
        {
            soundButton.image.sprite = soundSprite[1];
        }
    }

    private void OnOffSound()
    {
        if (PlayerPrefs.GetInt("sound") == 0)
        {
            soundButton.image.sprite = soundSprite[1];
            AudioManager.Ins.SoundOff();
            PlayerPrefs.SetInt("sound", 1);
        }
        else
        {
            soundButton.image.sprite = soundSprite[0];
            AudioManager.Ins.SoundOn();
            //AudioManager.Ins.PlaySelectBGM();

            PlayerPrefs.SetInt("sound", 0);
        }
    }

    private void OnOffMusic()
    {
        if (PlayerPrefs.GetInt("music") == 0)
        {
            musicButton.image.sprite = musicSprites[1];
            AudioManager.Ins.MusicOff();
            PlayerPrefs.SetInt("music", 1);
        }
        else
        {
            musicButton.image.sprite = musicSprites[0];
            AudioManager.Ins.MusicOn();
            PlayerPrefs.SetInt("music", 0);
        }
    }
}
