using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopTabButton : MonoBehaviour
{
    [SerializeField] ShopTabButton[] tabButton;
    public int id;
    public Image unSelect;
    public Image select;

    public void OnClickButton( int tabID)
    {
        for(int i = 0;i<=tabButton.Length-1;i++)
        {
            if(i == tabID)
            {
                tabButton[i].select.gameObject.SetActive(true);
                tabButton[i].unSelect.gameObject.SetActive(false);
            }
            else
            {
                tabButton[i].select.gameObject.SetActive(false);
                tabButton[i].unSelect.gameObject.SetActive(true);
            }
        }
    }
}
