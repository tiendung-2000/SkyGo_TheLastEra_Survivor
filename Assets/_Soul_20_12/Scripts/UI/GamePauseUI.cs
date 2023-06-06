using API.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : BaseUIMenu
{
    [SerializeField] Button closeButton;
    [SerializeField] Button homeButton;
    [SerializeField] Button replayButton;

    [SerializeField] Button soundButton;
    [SerializeField] Button musicButton;

    [SerializeField] List<GameObject> soundSprite;
    [SerializeField] List<GameObject> musicSprites;

    private void Start()
    {
        homeButton.onClick.AddListener(OnClickHomeButton);
        replayButton.onClick.AddListener(OnClickReplayButton);
        closeButton.onClick.AddListener(OnClickCloseButton);

        soundButton.onClick.AddListener(OnClickSound);
        musicButton.onClick.AddListener(OnClickMusic);

        //CanvasManager.Ins.CloseUI(UIName.GameplayUI);
    }

    private void OnEnable()
    {
        PlayerController.Ins.isMove = false;

        SetupButton();
    }

    private void OnDisable()
    {
        PlayerController.Ins.isMove = true;
    }

    private void OnClickCloseButton()
    {
        OnClose();
        AudioManager.Ins.SoundUIPlay(2);

    }

    void SetupButton()
    {
        if (PlayerPrefs.GetInt("music") == 1) //if is on
        {
            musicSprites[0].SetActive(true);
            musicSprites[1].SetActive(false);
        }
        else // if is off
        {
            musicSprites[0].SetActive(false);
            musicSprites[1].SetActive(true);
        }

        if (PlayerPrefs.GetInt("sound") == 1) //if is on
        {
            soundSprite[0].SetActive(true);
            soundSprite[1].SetActive(false);
        }
        else //if is off
        {
            soundSprite[0].SetActive(false);
            soundSprite[1].SetActive(true);
        }
    }

    public void OnClickSound()
    {
        if (PlayerPrefs.GetInt("sound") == 1)
        {
            AudioManager.Ins.SoundOff();
            soundSprite[0].SetActive(false);
            soundSprite[1].SetActive(true);
            PlayerPrefs.SetInt("sound", 0);

        }
        else
        {
            AudioManager.Ins.SoundOn();
            soundSprite[0].SetActive(true);
            soundSprite[1].SetActive(false);
            PlayerPrefs.SetInt("sound", 1);

        }
    }

    public void OnClickMusic()
    {
        if (PlayerPrefs.GetInt("music") == 1)
        {
            AudioManager.Ins.MusicOff();
            musicSprites[0].SetActive(false);
            musicSprites[1].SetActive(true);
            PlayerPrefs.SetInt("music", 0);
        }
        else
        {
            AudioManager.Ins.MusicOn();
            musicSprites[0].SetActive(true);
            musicSprites[1].SetActive(false);
            PlayerPrefs.SetInt("music", 1);
        }
    }

    private void OnClose()
    {
        LevelManager.Ins.PauseUnpause();
    }

    private void OnClickHomeButton()
    {
        OnHome();

        CinemachineShake.Instance.ResetShakeCamera();
    }

    void OnHome()
    {
        //inter


        CanvasManager.Ins.CloseUI(UIName.GameplayUI);

        CanvasManager.Ins.OpenUI(UIName.LoadingUI, null);

        GamePlayController.Ins.ResetGamePlay();
        //GamePlayController.Ins.ResetPlayerStats();
        ResourceSystem.Ins.players[DynamicDataManager.Ins.CurPlayer].gameObject.SetActive(false);

        CanvasManager.Ins.OpenUI(UIName.SelectLevelUI, null);
        Close();
    }

    private void OnClickReplayButton()
    {
        //OnReplay();
        //CinemachineShake.Instance.ResetShakeCamera();
        LevelManager.Ins.PauseUnpause();
    }

    //void OnReplay()
    //{
    //    CanvasManager.Ins.OpenUI(UIName.LoadingUI, null);

    //    GamePlayController.Ins.Replay();

    //    int openPopup = 5;
    //    if (Random.Range(1, 10) < openPopup)
    //    {
    //        CanvasManager.Ins.OpenUI(UIName.WeaponPopup, null);
    //    }

    //    Close();
    //}
}
