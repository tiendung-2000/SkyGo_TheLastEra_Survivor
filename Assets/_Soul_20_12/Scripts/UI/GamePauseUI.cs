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

    [SerializeField] bool isSound;
    [SerializeField] bool isMusic;

    private void Start()
    {
        isSound = true; isMusic = true;

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

    public void OnClickSound()
    {
        if (isSound)
        {
            soundSprite[0].SetActive(false);
            soundSprite[1].SetActive(true);
            isSound = false;
            AudioManager.Ins.SoundOff();
        }
        else
        {
            soundSprite[0].SetActive(true);
            soundSprite[1].SetActive(false);
            isSound = true;
            AudioManager.Ins.SoundOn();
        }
    }

    public void OnClickMusic()
    {
        if (isMusic)
        {
            musicSprites[0].SetActive(false);
            musicSprites[1].SetActive(true);
            isMusic = false;
            AudioManager.Ins.MusicOff();
        }
        else
        {
            musicSprites[0].SetActive(true);
            musicSprites[1].SetActive(false);
            isMusic = true;
            AudioManager.Ins.MusicOn();
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
