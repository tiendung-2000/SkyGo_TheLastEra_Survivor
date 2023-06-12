using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevel : MonoBehaviour
{
    private void OnEnable()
    {
        LevelManager.Ins.ChangeState(GameState.Gameplay);
    }
}
