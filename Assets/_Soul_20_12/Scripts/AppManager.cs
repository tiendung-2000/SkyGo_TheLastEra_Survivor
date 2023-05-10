using API.Sound;
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
            SoundManager.Ins.ChangeBGM(0);
        };
        CanvasManager.Ins.OpenUI(UIName.LoadingUI, new object[] { onLoaded });
    }
}
