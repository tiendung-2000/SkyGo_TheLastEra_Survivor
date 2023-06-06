using UnityEngine;
using API.UI;
using System.Collections;

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

    IEnumerator IEOutGateFX()
    {
        Instantiate(GamePlayController.Ins.outGateFX, (PlayerController.Ins.transform.position + new Vector3(0f,2.5f)), Quaternion.identity);

        yield return new WaitForSeconds(1f);
        ResourceSystem.Ins.players[DynamicDataManager.Ins.CurPlayer].gameObject.SetActive(false);

    }

    public void UnlockReward()
    {
        ResourceSystem.Ins.RewardsLevel.RewardsData[DynamicDataManager.Ins.CurLevel].isUnlock = true;
    }

    void EndLevel()
    {

        StartCoroutine(IEOutGateFX());
        Invoke(nameof(ShowComplete), timeDelayShowComplete);
    }

    void ShowComplete()
    {
        //inter

        CanvasManager.Ins.OpenUI(UIName.CompleteUI, null);

        if (LevelManager.Ins.isTestLevel == false && DynamicDataManager.Ins.CurTutorialStep > 0)
        {
            DynamicDataManager.AddNewLevelUnlocked(DynamicDataManager.Ins.CurLevel + 1);
        }
    }
}
