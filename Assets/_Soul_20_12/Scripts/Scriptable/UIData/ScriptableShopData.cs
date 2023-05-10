using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ShopData")]
public class ScriptableShopData : ScriptableObject
{
    public int totalShopItems;
    public int index;
    public List<ShopItemData> shopData;
}

[System.Serializable]

public class ShopItemData
{
    public string name;
    public Sprite image;
    public int value;
    //public bool isBuy;
    public float price;
}
