using API.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BigMapUI : BaseUIMenu
{
    [SerializeField] Button closeButton;
    [SerializeField] GameObject miniMap;

    public void OnClickCloseButton()
    {
        miniMap.SetActive(true);
        CameraMovement.Ins.canDrag = false;
        Close();
    }

    public void OnEnable()
    {
        CameraMovement.Ins.ResetOrigin();
        CameraMovement.Ins.transform.position = new Vector3(CharacterSelectManager.Ins.activePlayer.gameObject.transform.position.x,
                                            CharacterSelectManager.Ins.activePlayer.gameObject.transform.position.y,
                                            -10);
    }
}
