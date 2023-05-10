using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicDataManager : MonoBehaviour
{
    public static DynamicDataManager Ins;

    private static readonly string UnlockedCharacterPref = "UnlockedCharacter";

    private static readonly string UnlockedLevelPref = "UnlockedLevel";

    private static readonly string CurPlayerPref = "CurPlayer";

    private static readonly string CurLevelPref = "CurLevel";

    private static readonly string CurRewardPref = "CurReward";

    private static readonly string CurPlayerHPUpgradePref = "CurHPUpgrade";

    private static readonly string CurPlayerSpeedUpgradePref = "CurSpeedUpgrade";

    private static readonly string CurPlayerCooldownUpgradePref = "CurPlayerCooldownUpgrade";

    private static readonly string CurHealthPriceUpgradePref = "CurHealthPriceUpgrade";

    private static readonly string CurSkillPriceUpgradePref = "CurSkillPriceUpgrade";

    private static readonly string CurNumCoinPref = "CurNumCoin";

    private static readonly string CurNumGemPref = "CurNumGem";

    private static readonly string CurTutorialStepPref = "CurTutorial";

    public bool IsDataLoaded;

    public Action<int> OnRewardChange;

    public Action<int> OnHealthChange;

    public Action<int> OnHPUpgrade;

    public Action<int> OnSpeedUpgrade;

    public Action<int> OnCooldownUpgrade;

    public Action<int> OnHealthPrice;

    public Action<int> OnSkillPrice;

    public Action<int> OnCoinNumChange;

    public Action<int> OnCoinUse;

    public Action<int> OnGemNumChange;

    public Action<int> OnGemUse;

    public Action<int> OnTutorialStepChange;

    #region UnlockedCharacter/Map

    //UnlockCharacter/map

    static List<int> GetListIntListFromString(string input)
    {
        string[] stringvalues = input.Split(':');
        List<int> result = new List<int>();
        foreach (string s in stringvalues)
        {
            try
            {
                int val = int.Parse(s);
                if (!result.Contains(val)) result.Add(val);
            }
            catch (Exception e)
            {

            }
        }
        return result;
    }

    static string GetStringFromListInt(List<int> input)
    {
        return string.Join(":", input);
    }

    public static string UnlockedCharacter
    {
        get
        {
            return PlayerPrefs.GetString(UnlockedCharacterPref, "0"); //playerID: 0:1:2:3:4
        }
        set
        {
            PlayerPrefs.SetString(UnlockedCharacterPref, value);
        }
    }

    public static bool IsCharacterUnlocked(int playerID)//check
    {
        return GetListIntListFromString(UnlockedCharacter).Contains(playerID);
    }

    public static void AddNewCharacterUnlocked(int playerID) //add character
    {
        List<int> charUnlock = GetListIntListFromString(UnlockedCharacter);
        if (!charUnlock.Contains(playerID))
        {
            charUnlock.Add(playerID);
        }
        UnlockedCharacter = GetStringFromListInt(charUnlock);
    }
    //=======================================================
    public static string UnlockedLevel
    {
        get
        {
            return PlayerPrefs.GetString(UnlockedLevelPref, "0"); //playerID: 0:1:2:3:4
        }
        set
        {
            PlayerPrefs.SetString(UnlockedLevelPref, value);
        }
    }

    public static bool IsLevelUnlocked(int levelID)//check
    {
        return GetListIntListFromString(UnlockedLevel).Contains(levelID);
    }

    public static void AddNewLevelUnlocked(int levelID) //add character
    {
        List<int> levelUnlock = GetListIntListFromString(UnlockedLevel);
        if (!levelUnlock.Contains(levelID))
        {
            levelUnlock.Add(levelID);
        }
        UnlockedLevel = GetStringFromListInt(levelUnlock);
    }

    //=========================================

    #endregion

    #region Get/Set Hell
    public int CurPlayer//character
    {
        get => curPlayer;
        set
        {
            curPlayer = value;
            PlayerPrefs.SetInt(CurPlayerPref, curPlayer);
        }
    }

    [SerializeField]
    private int curPlayer;

    public int CurLevel//map
    {
        get => curLevel;
        set
        {
            if (value > ResourceSystem.Ins.levels.Count - 1)
            {
                value = 0;
            }
            curLevel = value;
            PlayerPrefs.SetInt(CurLevelPref, curLevel);
        }
    }

    [SerializeField]
    private int curLevel;

    public int CurReward//reward
    {
        get => curReward;
        set
        {
            curReward = value;
            PlayerPrefs.SetInt(CurRewardPref, curReward);
            OnRewardChange?.Invoke(curReward);
        }
    }

    [SerializeField]
    private int curReward;

    public int CurPlayerHPUpgrade
    {
        get => PlayerPrefs.GetInt(CurPlayerPref + curPlayer + CurPlayerHPUpgradePref, 0);
        set
        {
            curPlayerHPUpgrade = value;
            PlayerPrefs.SetInt(CurPlayerPref + curPlayer + CurPlayerHPUpgradePref, curPlayerHPUpgrade);
            OnHPUpgrade?.Invoke(curPlayerHPUpgrade);
        }
    }

    [SerializeField]
    private int curPlayerHPUpgrade;

    public int CurPlayerCooldownUpgrade
    {
        get => PlayerPrefs.GetInt(CurPlayerPref + curPlayer + CurPlayerCooldownUpgradePref, 0);
        set
        {
            curPlayerCooldownUpgrade = value;
            PlayerPrefs.SetInt(CurPlayerPref + curPlayer + CurPlayerCooldownUpgradePref, curPlayerCooldownUpgrade);
            OnCooldownUpgrade?.Invoke(curPlayerCooldownUpgrade);
        }
    }

    [SerializeField]
    private int curPlayerCooldownUpgrade;

    public int CurPlayerSpeedUpgrade
    {
        get => curPlayerSpeedUpgrade;
        set
        {
            curPlayerSpeedUpgrade = value;
            PlayerPrefs.SetInt(CurPlayerPref + curPlayer + CurPlayerSpeedUpgradePref, curPlayerSpeedUpgrade);
            OnSpeedUpgrade?.Invoke(curPlayerSpeedUpgrade);
        }
    }
    [SerializeField]
    private int curPlayerSpeedUpgrade;

    public int CurHealthPriceUpgrade
    {
        get => curHealthPriceUpgrade;
        set
        {
            curHealthPriceUpgrade = value;
            PlayerPrefs.SetInt(CurPlayerPref + curPlayer + CurHealthPriceUpgradePref, curHealthPriceUpgrade);
            OnHealthPrice?.Invoke(curHealthPriceUpgrade);
        }
    }

    [SerializeField]
    private int curHealthPriceUpgrade;

    public int CurSkillPriceUpgrade
    {
        get => curSkillPriceUpgrade;
        set
        {
            curSkillPriceUpgrade = value;
            PlayerPrefs.SetInt(CurPlayerPref + curPlayer + CurSkillPriceUpgradePref, curSkillPriceUpgrade);
            OnSkillPrice?.Invoke(curSkillPriceUpgrade);
        }
    }

    [SerializeField]
    private int curSkillPriceUpgrade;

    public int CurNumCoin
    {
        get => curNumCoin;
        set
        {
            if (value < 0)
                return;
            if (value < curNumCoin)
            {
                UseCoin(curNumCoin - value);
            }
            curNumCoin = value;
            OnCoinNumChange?.Invoke(curNumCoin);
            PlayerPrefs.SetInt(CurNumCoinPref, curNumCoin);
        }
    }

    [SerializeField]
    private int curNumCoin;

    public int CurNumGem
    {
        get => curNumGem;
        set
        {
            if (value < 0)
                return;
            curNumGem = value;
            OnGemNumChange?.Invoke(curNumGem);
            PlayerPrefs.SetInt(CurNumGemPref, curNumGem);
        }
    }

    [SerializeField]
    private int curNumGem;

    public int CurTutorialStep
    {
        get => curTutorialStep;
        set
        {
            curTutorialStep = value;
            OnTutorialStepChange?.Invoke(curTutorialStep);
            PlayerPrefs.SetInt(CurTutorialStepPref, curTutorialStep);
        }
    }

    [SerializeField]
    private int curTutorialStep;

    //private BaseSaveData curBaseData;

    //public List<int> GunKillCountData;
    #endregion

    //public float DamagePlayerCooldownMaxValue = 1f;

    //public float damagePlayerCooldown;

    //public bool IsStayDmg;

    private void Awake()
    {
        Ins = this;
    }

    private void Start()
    {
        StartCoroutine(LoadData());
    }

    IEnumerator LoadData()
    {
        yield return new WaitUntil(() => ResourceSystem.Ins && ResourceSystem.Ins.IsDataLoaded);
        curPlayer = PlayerPrefs.GetInt(CurPlayerPref, 0);
        curLevel = PlayerPrefs.GetInt(CurLevelPref, 0);
        curPlayerHPUpgrade = PlayerPrefs.GetInt(CurPlayerPref + curPlayer + CurPlayerHPUpgradePref, 0);
        curPlayerCooldownUpgrade = PlayerPrefs.GetInt(CurPlayerPref + curPlayer + CurPlayerCooldownUpgradePref, 0);
        curPlayerSpeedUpgrade = PlayerPrefs.GetInt(CurPlayerPref + curPlayer + CurPlayerSpeedUpgradePref, 0);
        curHealthPriceUpgrade = PlayerPrefs.GetInt(CurPlayerPref + curPlayer + CurHealthPriceUpgradePref, 0);
        curSkillPriceUpgrade = PlayerPrefs.GetInt(CurPlayerPref + curPlayer + CurSkillPriceUpgradePref, 0);
        curNumCoin = PlayerPrefs.GetInt(CurNumCoinPref, 0);
        curNumGem = PlayerPrefs.GetInt(CurNumGemPref, 0);
        curTutorialStep = PlayerPrefs.GetInt(CurTutorialStepPref, 0);

        IsDataLoaded = true;
    }

    public void UpdateCurPlayerUpgrade()
    {
        curPlayerHPUpgrade = PlayerPrefs.GetInt(CurPlayerPref + curPlayer + CurPlayerHPUpgradePref, 0);
        curPlayerSpeedUpgrade = PlayerPrefs.GetInt(CurPlayerPref + curPlayer + CurPlayerSpeedUpgradePref, 0);
    }

    public void UpdateCurCoolDownUpgrader()
    {
        curPlayerCooldownUpgrade = PlayerPrefs.GetInt(CurPlayerPref + curPlayer + CurPlayerCooldownUpgradePref, 0);
    }

    #region GreenWitch Data
    //private void LoadListData(ref List<int> list, string prefKey, int totalCount, int defaultFirstData = 1, int defaultData = 0)
    //{
    //    string stringData = PlayerPrefs.GetString(prefKey, "O");
    //    if (stringData != "O")
    //    {
    //        string[] data = stringData.Split('-');
    //        if (data.Length >= totalCount)
    //        {
    //            for (int i = 0; i < totalCount; i++)
    //            {
    //                if (!string.IsNullOrEmpty(data[i]))
    //                {
    //                    list.Add(int.Parse(data[i]));
    //                }
    //                else
    //                {
    //                    list.Add(defaultData);
    //                }
    //            }

    //        }
    //        else
    //        {
    //            for (int i = 0; i < data.Length; i++)
    //            {
    //                if (!string.IsNullOrEmpty(data[i]))
    //                {
    //                    list.Add(int.Parse(data[i]));
    //                }
    //                else
    //                {
    //                    list.Add(defaultData);
    //                }
    //            }
    //            for (int i = data.Length; i < totalCount; i++)
    //            {
    //                list.Add(defaultData);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        ResetListData(ref list, prefKey, totalCount, defaultFirstData, defaultData);
    //    }
    //}

    //private void ResetListData(ref List<int> list, string prefKey, int totalCount, int defaultFirstData = 1, int defaultData = 0)
    //{
    //    list.Add(defaultFirstData);
    //    for (int i = 1; i < totalCount; i++)
    //    {
    //        list.Add(defaultData);
    //    }
    //    SaveListData(list, prefKey);
    //}

    //private void SaveListData(List<int> list, string prefKey)
    //{
    //    string stringData = "";
    //    for (int i = 0; i < list.Count; i++)
    //    {
    //        stringData += list[i] + "-";
    //    }
    //    PlayerPrefs.SetString(prefKey, stringData);
    //}
    #endregion

    private void UseCoin(int num)
    {
        OnCoinUse?.Invoke(num);
    }
}
#region utf
[Serializable]
public class BaseSaveData
{
    public List<RoomSaveData> RoomSaveData = new List<RoomSaveData>();
}

[Serializable]
public class RoomSaveData
{
    public bool IsUnlock;

    public int StackUpCoinNum;

    public List<int> RoomItemLevel = new List<int>();
}
#endregion