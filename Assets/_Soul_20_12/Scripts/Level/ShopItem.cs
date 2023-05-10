//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;
//using UnityEngine.UI;

//public class ShopItem : MonoBehaviour
//{
//    public GameObject buyMessage;

//    public bool inBuyZone;

//    public bool isHealthRestore, isHealthUpgrade, isWeapon;

//    public int itemCost;

//    public int healthUpgradeAmount;

//    public Gun[] potentialGuns;
//    private Gun theGun;
//    public SpriteRenderer gunSprite;
//    public Text infoText;

//    public bool canBuy = false;

//    // Start is called before the first frame update
//    void Start()
//    {
//        if (isWeapon)
//        {
//            int selectedGun = Random.Range(0, potentialGuns.Length);
//            theGun = potentialGuns[selectedGun];

//            gunSprite.sprite = theGun.gunSprite;
//            infoText.text = theGun.weaponName + "\n - " + theGun.itemCost + " Gold - ";
//            itemCost = theGun.itemCost;
//        }
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (inBuyZone)
//        {
//            if (UIController.instance.buy)
//            {
//                BuyItem(RoomCenter.Ins.typeItem);

//                UIController.instance.buy = false;
//            }
//        }
//    }

//    public void BuyItem(int type)
//    {
//        Debug.Log("Can Buy");
//        if (inBuyZone)
//        {
//            if (LevelManager.instance.currentCoins >= itemCost)
//            {
//                LevelManager.instance.SpendCoins(itemCost);

//                if (isHealthRestore && type == 1)
//                {
//                    Debug.Log("buy health"); 
//                    DataManager.Ins.HealPlayer(ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.HP[DynamicDataManager.Ins.CurPlayerHPUpgrade]);
//                }

//                if (isHealthUpgrade && type == 2)
//                {
//                    Debug.Log("upgrade health");
//                    DynamicDataManager.Ins.CurPlayerHPUpgrade++; 
//                    //DataManager.Ins.IncreaseMaxHealth(healthUpgradeAmount);
//                }

//                if (isWeapon && type == 3)
//                {
//                    Debug.Log("buy weapon");
//                    Gun gunClone = Instantiate(theGun);
//                    gunClone.transform.parent = PlayerController.instance.gunArm;
//                    gunClone.transform.position = PlayerController.instance.gunArm.position;
//                    gunClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
//                    gunClone.transform.localScale = Vector3.one;

//                    PlayerController.instance.availableGuns.Add(gunClone);
//                    PlayerController.instance.currentGun = PlayerController.instance.availableGuns.Count - 1;
//                    PlayerController.instance.SwitchGun();

//                }

//                gameObject.SetActive(false);
//                inBuyZone = false;

//                AudioManager.instance.PlaySFX(18);
//            }
//            else
//            {
//                AudioManager.instance.PlaySFX(19);
//            }
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.tag == "Player")
//        {
//            buyMessage.SetActive(true);
//            inBuyZone = true;

//            if (this.gameObject.name == "Shop Item - Restore Health")
//            {
//                RoomCenter.Ins.typeItem = 1;
//            }
//            else if (this.gameObject.name == "Shop Item - Upgrade Health")
//            {
//                RoomCenter.Ins.typeItem = 2;
//            }
//            else if (this.gameObject.name == "Shop Item - Buy Weapon")
//            {
//                RoomCenter.Ins.typeItem = 3;
//            }
//            UIController.instance.shootButton.SetActive(false);
//            UIController.instance.buyButton.SetActive(true);
//        }
//    }

//    private void OnTriggerExit2D(Collider2D other)
//    {
//        if (other.tag == "Player")
//        {
//            buyMessage.SetActive(false);

//            inBuyZone = false;

//            UIController.instance.shootButton.SetActive(true);
//            UIController.instance.buyButton.SetActive(false);
//        }
//    }
//}
