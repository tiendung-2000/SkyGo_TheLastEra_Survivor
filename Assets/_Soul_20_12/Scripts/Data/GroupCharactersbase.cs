using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerDatabase", menuName = "PlayerDatabase")]
public class GroupCharactersbase : ScriptableObject
{
    public List<PlayerDatabase> Characters;
}

[Serializable]
public class PlayerDatabase
{
    public PlayerData Data;

    public PlayerUpgradeData UpgradeData;
}

[Serializable]
public class PlayerData
{
    public int priceToUnlock;

    public List<int> HP;

    public List<int> CoolDown;

    public List<int> Speed;

    public Sprite skillImg;
    public string skillDetail;
}

[Serializable]
public class PlayerUpgradeData
{
    public List<int> HPUpgradePrice;

    public List<int> CoolDownUpgradePrice;
}