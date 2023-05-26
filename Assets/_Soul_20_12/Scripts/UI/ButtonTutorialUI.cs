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

    private void OnEnable()
    {
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
        this.gameObject.SetActive(false);
    }
}
