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
        if (DynamicDataManager.Ins.CurTutorialStep == 0)
        {
            settingButton.gameObject.SetActive(false);
            StartCoroutine(IEShowTutorial());
        }
        else
        {
            settingButton.gameObject.SetActive(true);
        }
    }

    IEnumerator IEShowTutorial()
    {
        yield return new WaitForSeconds(5f);
        tutorial.SetActive(true);
    }

    private void Start()
    {
        settingButton.onClick.AddListener(OnClickSettingUI);
    }

    public void OnClickSettingUI()
    {
        OnSetting();
    }

    public void OnSetting()
    {
        //CanvasManager.Ins.OpenUI(UIName.PauseSettingUI, null);
        LevelManager.Ins.PauseUnpause();
    }
}
