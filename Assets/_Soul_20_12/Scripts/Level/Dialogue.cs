using API.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public string dialogText;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CanvasManager.Ins.OpenUI(UIName.TutotialDialog, null);
            TutorialUI.Ins.SetDetail(dialogText);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CanvasManager.Ins.CloseUI(UIName.TutotialDialog);
        }
    }
}
