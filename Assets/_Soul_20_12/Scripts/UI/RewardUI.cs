using API.UI;
using UnityEngine;
using UnityEngine.UI;

public class RewardUI : BaseUIMenu
{
    [SerializeField] Button backButton;
    //[SerializeField] Button addCoinButton;

    [SerializeField] Transform tabsParent;
    [SerializeField] RewardTabs rewardTab;

    private void Start()
    {
        backButton.onClick.AddListener(OnClickBackButton);

        SetUpReward();
    }

    void SetUpReward()
    {
        var rewardLevel = ResourceSystem.Ins.RewardsLevel;
        var index = 0;
        foreach (var reward in rewardLevel.RewardsData)
        {
            if (reward.isUnlock == true)
            {
                rewardTab.tabBackground.sprite = rewardTab.unlockSp;
                rewardTab.claimButton.gameObject.SetActive(true);
                rewardTab.lockClaim.gameObject.SetActive(false);
            }
            else
            {
                rewardTab.tabBackground.sprite = rewardTab.lockSp;
                rewardTab.claimButton.gameObject.SetActive(false);
                rewardTab.lockClaim.gameObject.SetActive(true);
            }

            if (reward.isClamed == true)
            {
                //Debug.Log("Claimed");
                rewardTab.claimButton.gameObject.SetActive(false);
                rewardTab.collected.gameObject.SetActive(true);
            }
            else if (reward.isClamed == false)
            {
                //Debug.Log("NotClaim");
                rewardTab.claimButton.gameObject.SetActive(true);
                rewardTab.collected.gameObject.SetActive(false);
            }

            var tab = Instantiate(rewardTab, tabsParent);
            tab.index = index;
            tab.SetUp(reward.RewardName, reward.RewardValue.ToString());
            index++;
        }
    }

    public void OnClickBackButton()
    {
        OnBack();
        AudioManager.Ins.SoundUIPlay(2);
    }

    public void OnBack()
    {
        CanvasManager.Ins.OpenUI(UIName.SelectLevelUI, null);
        Close();
    }
}
