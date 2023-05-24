using API.UI;
using System;
using UnityEngine;

public class AppManager : Singleton<AppManager>
{
    void Start()
    {
        Application.targetFrameRate = 60;

        Action onLoaded = () =>
        {
            CanvasManager.Ins.OpenUI(UIName.StartUI, null);
            AudioManager.Instance.PlayMainMenuBGM();
        };
        CanvasManager.Ins.OpenUI(UIName.LoadingUI, new object[] { onLoaded });
    }
}
