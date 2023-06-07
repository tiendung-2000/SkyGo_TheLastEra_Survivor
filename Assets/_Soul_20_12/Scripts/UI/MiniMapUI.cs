using API.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapUI : BaseUIMenu
{
    [SerializeField] Button openMap;
    [SerializeField] GameObject bigMap;

    public void OpenMap()
    {
        bigMap.SetActive(true);

        //CameraMovement.Ins.canDrag = true;
        Close();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            OpenMap();
        }
    }
}
