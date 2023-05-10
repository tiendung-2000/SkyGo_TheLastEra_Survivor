using API.UI;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : BaseUIMenu
{
    [SerializeField] Button settingButton;

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
