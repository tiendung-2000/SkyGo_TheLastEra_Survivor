using API.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : BaseUIMenu
{
    [SerializeField] Button settingButton;
    [SerializeField] GameObject tutorial;

    private void OnEnable()
    {
        //if (DynamicDataManager.Ins.CurTutorialStep == 0)
        //{
        //    settingButton.gameObject.SetActive(false);
        //}
        //else
        //{
        //    settingButton.gameObject.SetActive(true);
        //}
        StartCoroutine(IESetupGunStats());
    }

    IEnumerator IESetupGunStats()
    {
        yield return new WaitForSeconds(2f);
        ButtonControllerUI.Ins.SetupGunStats();

    }

    #region Desktop
    bool settingOn;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            settingOn = !settingOn;
            if (settingOn)
            {
                CanvasManager.Ins.OpenUI(UIName.PauseSettingUI, null);
            }
            else
            {
                CanvasManager.Ins.CloseUI(UIName.PauseSettingUI);
            }
        }
    }
    #endregion

    IEnumerator IEShowTutorial()
    {
        yield return new WaitForSeconds(5f);
        tutorial.SetActive(true);

    }

    private void Start()
    {
        settingButton.onClick.AddListener(OnClickSettingUI);

        if (DynamicDataManager.Ins.CurTutorialStep == 0)
        {
            StartCoroutine(IEShowTutorial());
        }
    }

    public void OnClickSettingUI()
    {
        AudioManager.Ins.SoundUIPlay(2);
        OnSetting();
    }

    public void OnSetting()
    {
        //CanvasManager.Ins.OpenUI(UIName.PauseSettingUI, null);
        LevelManager.Ins.PauseUnpause();
    }
}
