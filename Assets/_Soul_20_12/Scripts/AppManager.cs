using API.UI;
using System;
using UnityEngine;

public class AppManager : Singleton<AppManager>
{
    [SerializeField] bool passTutorial;

    void Start()
    {
        if(passTutorial == true)
        {
            DynamicDataManager.Ins.CurTutorialStep = 1;
        }

        Application.targetFrameRate = 60;

        Action onLoaded = () =>
        {
            CanvasManager.Ins.OpenUI(UIName.StartUI, null);
            AudioManager.Ins.PlayMainMenuBGM();
        };

        CanvasManager.Ins.OpenUI(UIName.LoadingUI, new object[] { onLoaded });
    }
}
