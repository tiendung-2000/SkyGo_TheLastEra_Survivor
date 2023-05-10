using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scriptable Level Reward")]
public class ScriptableRewardsLevel : ScriptableObject {

    //public GameObject Prefab;

    public int TotalReward;

    public int[] Level;

    //public int[] Rewards;

    public List<RewardData> RewardsData;

}

[System.Serializable]
public class RewardData
{
    public bool isUnlock;
    public bool isClamed;
    public string RewardName;

    public int RewardValue;

    //public Image RewardImage;
}