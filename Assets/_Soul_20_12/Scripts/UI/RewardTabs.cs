using UnityEngine;
using UnityEngine.UI;
//[RequireComponent(typeof(Button))]
public class RewardTabs : MonoBehaviour
{
    public int index;

    public Button claimButton;
    public GameObject lockClaim;

    [SerializeField] Text rewardName;
    [SerializeField] Text coinTextValue;

    public Image tabBackground;
    public Sprite lockSp;
    public Sprite unlockSp;

    public GameObject collected;

    RewardTabs currentTab;

    private void Start()
    {
        claimButton.onClick.AddListener(OnClickClaimButton);
    }

    public void SetUp(string name, string value)
    {
        rewardName.text = name;
        coinTextValue.text = value;      
    }

    void OnClickClaimButton()
    {
        OnClaim();
    }

    void OnClaim()
    {
        AudioManager.Ins.SoundUIPlay(4);
        DynamicDataManager.Ins.CurNumCoin += ResourceSystem.Ins.RewardsLevel.RewardsData[index].RewardValue;

        claimButton.gameObject.SetActive(false);
        collected.gameObject.SetActive(true);
        ResourceSystem.Ins.RewardsLevel.RewardsData[index].isClamed = true; //claimed
    }

    public void SetUpClaim()
    {
        Debug.Log("setup");
        claimButton.gameObject.SetActive(false);
        collected.gameObject.SetActive(true);
    }
}
