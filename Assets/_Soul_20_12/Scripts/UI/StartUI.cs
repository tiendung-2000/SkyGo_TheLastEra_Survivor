using API.Sound;
using API.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : BaseUIMenu
{
    [SerializeField] GameObject start;
    [SerializeField] Button startButton;

    private void Start()
    {
        startButton.onClick.AddListener(OnStart);
    }

    public void OnStart()
    {
        //if (DynamicDataManager.IsLevelUnlocked(DynamicDataManager.Ins.CurLevel) == true)
        //{
            UITransition.Ins.ShowTransition(() =>
            {
                start.gameObject.SetActive(false);
                SoundManager.Ins.ChangeBGM(1);

                CanvasManager.Ins.OpenUI(UIName.SelectLevelUI, null);
            });


        //}
        //else
        //{
        //    Debug.Log("Level doesn't Unlocked");
        //}

    }
}
