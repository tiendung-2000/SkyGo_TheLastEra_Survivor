using API.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPopup : BaseUIMenu
{
    [SerializeField] Button watchAdsButton;
    [SerializeField] Button noButton;

    private Gun theGun;
    public Gun[] potentialGuns;
    public Image gunSpriteUI;

    private void Start()
    {
        watchAdsButton.onClick.AddListener(OnClickAdsButton);
        noButton.onClick.AddListener(OnClickNoButton);      
    }

    private void OnEnable()
    {
        int selectedGun = Random.Range(0, potentialGuns.Length);
        theGun = potentialGuns[selectedGun];
        gunSpriteUI.sprite = theGun.gunSprite;
    }

    void OnClickAdsButton()
    {
        OnAds();
    }

    void OnAds()
    {
        bool hasGun = false;
        foreach (Gun gunToCheck in PlayerController.Ins.availableGuns)
        {
            //if hasgun
            print(PlayerController.Ins.availableGuns.Count);
            Debug.Log(theGun.weaponName + "  -  " + gunToCheck.weaponName);
            if (theGun.weaponName == gunToCheck.weaponName)
            {
                hasGun = true;
                theGun.PickupAmmo(30);
            }
        }

        if (!hasGun)
        {
            Gun gunClone = Instantiate(theGun);
            gunClone.transform.parent = PlayerController.Ins.theHand;
            gunClone.transform.position = PlayerController.Ins.theHand.position;
            gunClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
            gunClone.transform.localScale = Vector3.one;

            PlayerController.Ins.availableGuns.Add(gunClone);
            PlayerController.Ins.currentGun = PlayerController.Ins.availableGuns.Count - 1;
            PlayerController.Ins.SwitchGun();
        }

        CanvasManager.Ins.CloseUI(UIName.WeaponPopup);
    }

    void OnClickNoButton()
    {
        OnNo();
    }

    void OnNo()
    {
        CanvasManager.Ins.CloseUI(UIName.WeaponPopup);
    }
}
