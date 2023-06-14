using API.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTutorialUI : BaseUIMenu
{
    [SerializeField] Button buttonSkill;
    [SerializeField] Button buttonWeapon;

    [SerializeField] Text tutorialText;

    IEnumerator IEDelay()
    {
        yield return new WaitForSeconds(.2f);
        PlayerController.Ins.isMove = false;
        PlayerController.Ins.PlayerStopMove();
    }

    private void OnEnable()
    {
        StartCoroutine(IEDelay());
        buttonSkill.gameObject.SetActive(true);
        tutorialText.gameObject.SetActive(true);
    }

    private void Start()
    {
        buttonSkill.onClick.AddListener(ShowButtonWeaponTutorial);
        buttonWeapon.onClick.AddListener(CloseButton);
    }

    void ShowButtonWeaponTutorial()
    {
        buttonSkill.gameObject.SetActive(false);
        buttonWeapon.gameObject.SetActive(true);
        tutorialText.text = "Touch to switch weapon";
    }

    void CloseButton()
    {
        ButtonControllerUI.Ins.OnEnableJoyStick();
        PlayerController.Ins.isMove = true;
        this.gameObject.SetActive(false);
    }
}
