using UnityEngine;
using API.UI;

public class LevelGate : MonoBehaviour
{
    public static LevelGate Ins;

    float timeDelayShowComplete = 1f;

    public int coinBonus = 1000;

    public bool isComplete = false;

    private void Awake()
    {
        Ins = this;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (DynamicDataManager.Ins.CurTutorialStep == 0)
            {
                EndLevel();
            }
            else
            {
                isComplete = true;
                PlayerController.Ins.isMove = false;
                EndLevel();
                UnlockReward();
            }
        }
    }

    public void UnlockReward()
    {
        ResourceSystem.Ins.RewardsLevel.RewardsData[DynamicDataManager.Ins.CurLevel].isUnlock = true;
    }

    void EndLevel()
    {
        Invoke(nameof(ShowComplete), timeDelayShowComplete);
    }

    void ShowComplete()
    {
        CanvasManager.Ins.OpenUI(UIName.CompleteUI, null);

        if (LevelManager.Ins.isTestLevel == false && DynamicDataManager.Ins.CurTutorialStep > 0)
        {
            DynamicDataManager.AddNewLevelUnlocked(DynamicDataManager.Ins.CurLevel + 1);
        }
    }
}
