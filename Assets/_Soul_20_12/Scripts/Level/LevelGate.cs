using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using API.UI;
using System.Reflection;

public class LevelGate : MonoBehaviour
{
    public static LevelGate Ins;

    [SerializeField] GameObject gate_1;
    [SerializeField] GameObject gate_2;

    float timeDelayShowComplete = 1f;

    public int coinBonus = 1000;

    public bool isComplete = false;

    Tween OnEnableTween_1;
    Tween OnEnableTween_2;

    private void Awake()
    {
        Ins = this;
    }

    void OnEnable()
    {
        OnEnableTween_1?.Kill();
        OnEnableTween_2?.Kill();

        OnEnableTween_1 = gate_1.transform.DORotate(new Vector3(0f, 0f, 360f), 5f, RotateMode.Fast)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart)
            .SetRelative();
        OnEnableTween_2 = gate_2.transform.DORotate(new Vector3(0f, 0f, -360f), 2f, RotateMode.Fast)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart)
            .SetRelative();
    }

    private void OnDisable()
    {
        OnEnableTween_1?.Kill();
        OnEnableTween_2?.Kill();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isComplete = true;
            PlayerController.Ins.isMove = false;
            EndLevel();

            UnlockReward();
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
        if (LevelManager.Ins.isTestLevel == false)
        {
            DynamicDataManager.AddNewLevelUnlocked(DynamicDataManager.Ins.CurLevel + 1);
        }
    }
}
