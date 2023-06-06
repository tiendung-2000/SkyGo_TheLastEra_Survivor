using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopTabs : MonoBehaviour
{
    public int index;

    public Button buyButton;

    [SerializeField] Text itemName;
    [SerializeField] Text itemValue;
    [SerializeField] Image itemImage;
    [SerializeField] Text itemPrice;
    [SerializeField] Sprite watchAdsSprite;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Image buttonImage;

    ShopTabs currentTab;

    private void Start()
    {
        buyButton.onClick.AddListener(OnClickBuyButton);
    }

    public void SetUp(string name, string value, Sprite sprite, string price)
    {
        if (index == 0)
        {
            //Debug.Log(index);
            itemName.text = name;
            itemValue.text = value;
            itemImage.sprite = sprite;
            itemPrice.text = "Watch Ads";
            buttonImage.sprite = watchAdsSprite;
        }
        else
        {
            itemName.text = name;
            itemValue.text = value;
            itemImage.sprite = sprite;
            itemPrice.text = "$"+price;
            buttonImage.sprite = defaultSprite;
        }
    }

    void OnClickBuyButton()
    {
        OnBuy();
        AudioManager.Ins.SoundUIPlay(4);
    }

    void OnBuy()
    {
        if(index == 0)
        {
            //reward
            Debug.Log("Watch Ads");
            DynamicDataManager.Ins.CurNumCoin += ResourceSystem.Ins.ShopData.shopData[index].value;
        }
        else
        {
            //purchase
            DynamicDataManager.Ins.CurNumCoin += ResourceSystem.Ins.ShopData.shopData[index].value;
        }
    }
}
