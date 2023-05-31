using API.UI;
using MagneticScrollView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectLevelUI : BaseUIMenu
{
    public static SelectLevelUI Ins;

    [SerializeField] Animator animUI;

    [SerializeField] GameObject scroll;

    [Header("===LevelUI Button===")]
    public Button selectLevelButton;
    public Button unlockLevelButton;
    public Button watchAdsToTestButton;
    [SerializeField] Button settingButton;
    [SerializeField] Button rewardButton;
    [SerializeField] Button shopButton;

    //[SerializeField] GameObject settingPopup;
    [SerializeField] bool isOpenSettingPopup;

    public Text levelName;
    public Text levelUnlockPrice;

    public List<LevelItem> listLevel = new List<LevelItem>();

    public int levelIndex;

    private void Awake()
    {
        Ins = this;
    }

    private void Start()
    {
        unlockLevelButton.onClick.AddListener(OnUnlockLevel);
        watchAdsToTestButton.onClick.AddListener(OnWatchAdsLevel);

        isOpenSettingPopup = true;

        selectLevelButton.onClick.AddListener(OnClickSelectLevelButton);

        settingButton.onClick.AddListener(OnClickSettingButton);
        rewardButton.onClick.AddListener(OnClickRewardButton);
        shopButton.onClick.AddListener(OnClickShopButton);
    }

    private void OnEnable()
    {
        AudioManager.Ins.PlaySelectBGM();

        CanvasManager.Ins.OpenUI(UIName.CoinBar, null);

        Time.timeScale = 1f;
        SetUpLevel();

        MagneticScrollRect magneticScroll = scroll.GetComponent<MagneticScrollRect>();

        if (scroll.transform.childCount != 0)
        {
            magneticScroll.ResetScroll();
            magneticScroll.StartAutoArranging();
        }

        magneticScroll.StartAutoArranging();

        magneticScroll.m_currentSelected = DynamicDataManager.Ins.CurLevel;
        magneticScroll.StartScrolling();

        CheckLevelUnlocked();
    }

    public void SetUpLevel()
    {
        foreach (LevelItem level in listLevel)
        {
            if (DynamicDataManager.IsLevelUnlocked(level.id) == true)
            {
                DynamicDataManager.Ins.CurLevel = level.id;
                level.locked.SetActive(false);
                levelName.text = ResourceSystem.Ins.levels[level.id].levelName.ToString();
            }
            else
            {
                level.locked.SetActive(true);
            }
        }
    }

    public void CheckLevelUnlocked()
    {
        if (DynamicDataManager.IsLevelUnlocked(DynamicDataManager.Ins.CurLevel))
        {
            selectLevelButton.gameObject.SetActive(true);
            unlockLevelButton.gameObject.SetActive(false);
            watchAdsToTestButton.gameObject.SetActive(false);
        }
        else
        {
            selectLevelButton.gameObject.SetActive(false);
            unlockLevelButton.gameObject.SetActive(true);
            watchAdsToTestButton.gameObject.SetActive(true);
        }
    }

    public void OnUnlockLevel()
    {
        AudioManager.Ins.SoundUIPlay(2);

        int priceToUnLock = ResourceSystem.Ins.levels[DynamicDataManager.Ins.CurLevel].priceToUnlock;
        if (DynamicDataManager.Ins.CurNumCoin >= priceToUnLock)
        {
            AudioManager.Ins.SoundUIPlay(3);

            DynamicDataManager.Ins.CurNumCoin -= priceToUnLock;
            DynamicDataManager.AddNewLevelUnlocked(DynamicDataManager.Ins.CurLevel);
            selectLevelButton.gameObject.SetActive(true);
            unlockLevelButton.gameObject.SetActive(false);
            watchAdsToTestButton.gameObject.SetActive(false);
            SetUpLevel();
        }
        else
        {
            CanvasManager.Ins.OpenUI(UIName.ShopUI, null);
        }
    }

    void OnWatchAdsLevel()
    {
        AudioManager.Ins.SoundUIPlay(2);

        LevelManager.Ins.isTestLevel = true;
        DynamicDataManager.Ins.CurLevel = scroll.GetComponent<MagneticScrollRect>().m_currentSelected;

        UITransition.Ins.ShowTransition(() =>
        {
            CanvasManager.Ins.OpenUI(UIName.SelectCharactersUI, null);
            CanvasManager.Ins.OpenUI(UIName.CoinBar, null);
            Close();
        });
    }

    

    public void OnClickSelectLevelButton()
    {
        OnSelectLevel();
    }

    public void OnClickSettingButton()
    {
        OnSetting();
    }

    public void OnClickRewardButton()
    {
        OnReward();
    }

    public void OnClickShopButton()
    {
        OnShop();
    }

    public void OnSelectLevel()
    {
        AudioManager.Ins.SoundUIPlay(1);

        DynamicDataManager.Ins.CurLevel = scroll.GetComponent<MagneticScrollRect>().m_currentSelected;

        UITransition.Ins.ShowTransition(() =>
        {
            CanvasManager.Ins.OpenUI(UIName.SelectCharactersUI, null);
            CanvasManager.Ins.OpenUI(UIName.CoinBar, null);
            Close();
        });
    }

    [SerializeField] RectTransform buttonSetting;

    public void OnSetting()
    {
        AudioManager.Ins.SoundUIPlay(2);

        if (isOpenSettingPopup)
        {
            //settingPopup.SetActive(true);
            isOpenSettingPopup = false;
        }
        else
        {
            //settingPopup.SetActive(false);
            isOpenSettingPopup = true;
        }

        //Debug.Log("Click");

        //listLevel[2].isUnlock= true;
        //SetUpLevel();


        //settingPopup.SetActive(isOpenSettingPopup == true ? true : false);

        //isOpenSettingPopup = isOpenSettingPopup == false ? true : false;


    }

    public void OnReward()
    {
        AudioManager.Ins.SoundUIPlay(2);

        CanvasManager.Ins.OpenUI(UIName.RewardsUI, null);
        CanvasManager.Ins.OpenUI(UIName.CoinBar, null);
        Close();
    }

    public void OnShop()
    {
        AudioManager.Ins.SoundUIPlay(2);

        CanvasManager.Ins.OpenUI(UIName.ShopUI, null);
    }
}
