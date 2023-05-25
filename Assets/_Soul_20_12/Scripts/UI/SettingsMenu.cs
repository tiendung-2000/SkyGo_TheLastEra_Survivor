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
    [SerializeField] bool isSound;
    [SerializeField] bool isMusic;

    void Start()
    {
        isSound = true; isMusic = true;
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

    private void OnOffSound()
    {
        if (isSound)
        {
            soundButton.image.sprite = soundSprite[1];
            isSound = false;
            AudioManager.Ins.SoundOff();
        }
        else
        {
            soundButton.image.sprite = soundSprite[0];
            isSound = true;
            AudioManager.Ins.SoundOn();
        }
    }

    private void OnOffMusic()
    {
        if (isMusic)
        {
            musicButton.image.sprite = musicSprites[1];
            isMusic = false;
            AudioManager.Ins.MusicOff();
        }
        else
        {
            musicButton.image.sprite = musicSprites[0];
            isMusic = true;
            AudioManager.Ins.MusicOn();
        }
    }
}
