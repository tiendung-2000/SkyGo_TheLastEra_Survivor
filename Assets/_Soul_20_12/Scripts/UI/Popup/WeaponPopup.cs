using API.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPopup : BaseUIMenu
{
    [SerializeField] Button watchAdsButton;
    [SerializeField] Button noButton;
    [SerializeField] PopupAdsType adsType;
    private GunPickup theWeapon;
    public GunPickup[] normalWeapon;
    public GunPickup[] legendWeapon;
    public Image gunSpriteUI;
    public Sprite goldSpriteUI;
    public Text titleText;

    private void Start()
    {
        watchAdsButton.onClick.AddListener(OnClickAdsButton);
        noButton.onClick.AddListener(OnClickNoButton);
    }

    IEnumerator IEDelay()
    {
        yield return new WaitForSeconds(.2f);
        PlayerController.Ins.isMove = false;
        PlayerController.Ins.PlayerStopMove();
    }

    private void OnEnable()
    {       
        StartCoroutine(IEDelay());

        switch (adsType)
        {
            case PopupAdsType.DefaultAds:
                int selectedGun = Random.Range(0, normalWeapon.Length);
                theWeapon = normalWeapon[selectedGun];
                gunSpriteUI.sprite = theWeapon.icon;
                titleText.text = "Watch Ads To Get Common Weapon?";
                break;
            case PopupAdsType.Ads5s:
                gunSpriteUI.sprite = goldSpriteUI;
                titleText.text = "Watch Ads To Get More Gold?";
                break;
            case PopupAdsType.Ads10s:
                int normal = Random.Range(0, normalWeapon.Length);
                theWeapon = normalWeapon[normal];
                gunSpriteUI.sprite = theWeapon.icon;
                titleText.text = "Watch Ads To Get Common Weapon?";
                break;
            case PopupAdsType.Ads30s:
                int legend = Random.Range(0, legendWeapon.Length);
                theWeapon = legendWeapon[legend];
                gunSpriteUI.sprite = theWeapon.icon;
                titleText.text = "Watch Ads To Get Legend Weapon?";
                break;
        }

        //int selectedGun = Random.Range(0, potentialGuns.Length);
        //theGun = potentialGuns[selectedGun];
        //gunSpriteUI.sprite = theGun.gunSprite;
    }

    void OnClickAdsButton()
    {
        OnAds();
    }

    void OnAds()
    {
        #region Old
        //reward
        //AudioManager.Ins.SoundUIPlay(2);

        //bool hasGun = false;
        //foreach (Weapon gunToCheck in PlayerController.Ins.availableGuns)
        //{
        //    //if hasgun
        //    print(PlayerController.Ins.availableGuns.Count);
        //    Debug.Log(theGun.weaponName + "  -  " + gunToCheck.weaponName);
        //    if (theGun.weaponName == gunToCheck.weaponName)
        //    {
        //        hasGun = true;
        //        //theGun.PickupAmmo(30);
        //    }
        //}

        //if (!hasGun)
        //{
        //    Weapon gunClone = Instantiate(theGun);
        //    gunClone.transform.parent = PlayerController.Ins.theHand;
        //    gunClone.transform.position = PlayerController.Ins.theHand.position;
        //    gunClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
        //    gunClone.transform.localScale = Vector3.one;

        //    PlayerController.Ins.availableGuns.Add(gunClone);
        //    PlayerController.Ins.currentGun = PlayerController.Ins.availableGuns.Count - 1;
        //    PlayerController.Ins.SwitchGun();
        //}

        //CanvasManager.Ins.CloseUI(UIName.WeaponPopup);
        #endregion
        switch (adsType)
        {
            case PopupAdsType.DefaultAds:
                Instantiate(theWeapon, PlayerController.Ins.transform.position, Quaternion.identity);
                break;
            case PopupAdsType.Ads5s:
                LevelManager.Ins.RewardAdsItem();
                break;
            case PopupAdsType.Ads10s:
                Instantiate(theWeapon, PlayerController.Ins.transform.position, Quaternion.identity);
                break;
            case PopupAdsType.Ads30s:
                Instantiate(theWeapon, PlayerController.Ins.transform.position, Quaternion.identity);
                break;
        }

        OnClose();
        
    }

    void OnClose()
    {
        ButtonControllerUI.Ins.OnEnableJoyStick();
        PlayerController.Ins.isMove = true;
        Close();
    }

    void OnClickNoButton()
    {
        OnNo();
    }

    void OnNo()
    {
        OnClose();
    }
}

public enum PopupAdsType
{
    DefaultAds,
    Ads5s,
    Ads10s,
    Ads30s,
}
