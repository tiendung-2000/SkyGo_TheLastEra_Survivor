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
        UITransition.Ins.ShowTransition(() =>
        {
            start.gameObject.SetActive(false);

            CanvasManager.Ins.OpenUI(UIName.SelectLevelUI, null);
            AudioManager.Instance.PlayStageBGM();
        });
    }
}
