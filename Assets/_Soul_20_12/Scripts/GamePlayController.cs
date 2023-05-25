using UnityEngine;

public class GamePlayController : Singleton<GamePlayController>
{
    public bool isMusic;
    public bool isSound;

    [SerializeField] float startTimeScale = 1;

    #region GameLoop

    public void Replay()
    {
        ResourceSystem.Ins.players[DynamicDataManager.Ins.CurPlayer].gameObject.SetActive(true);
        ResourceSystem.Ins.players[DynamicDataManager.Ins.CurPlayer].transform.position = Vector3.zero;
        ResourceSystem.Ins.SpawnLevel(1);
        CharacterSelectManager.Ins.activePlayer = ResourceSystem.Ins.players[DynamicDataManager.Ins.CurPlayer];

        ResetPlayerStats();
    }

    public void NextLevel()
    {
        ResourceSystem.Ins.players[DynamicDataManager.Ins.CurPlayer].gameObject.SetActive(true);
        ResourceSystem.Ins.players[DynamicDataManager.Ins.CurPlayer].transform.position = Vector3.zero;
        ResourceSystem.Ins.SpawnLevel(0);
        CharacterSelectManager.Ins.activePlayer = ResourceSystem.Ins.players[DynamicDataManager.Ins.CurPlayer];

        ResetPlayerStats();
    }

    public void StartGame()
    {
        Time.timeScale = startTimeScale;
    }

    public void LoseGame()
    {

    }

    public void WinGame()
    {

    }

    public void OnRecoverGame()
    {
        ResourceSystem.Ins.players[DynamicDataManager.Ins.CurPlayer].gameObject.SetActive(true);
        PlayerController.Ins.currentHealth = PlayerController.Ins.curPlayerMaxHP;
        DynamicDataManager.Ins.OnHealthChange?.Invoke(PlayerController.Ins.curPlayerMaxHP);
    }

    public void ResetGamePlay()
    {
        //if (ResourceSystem.Ins.CurLevelGameObj != null)
        //{
            SmartPool.Ins.Despawn(ResourceSystem.Ins.CurLevelGameObj);
        //}
        //else
        //{
        //    SmartPool.Ins.Despawn(ResourceSystem.Ins.tutorialLevel.gameObject);
        //}
        PlayerController.Ins.currentHealth = PlayerController.Ins.curPlayerMaxHP;
        DynamicDataManager.Ins.OnHealthChange?.Invoke(PlayerController.Ins.curPlayerMaxHP);
        ResourceSystem.Ins.players[DynamicDataManager.Ins.CurPlayer].gameObject.SetActive(false);
        ResourceSystem.Ins.players[DynamicDataManager.Ins.CurPlayer].transform.position = Vector3.zero;
    }
    public void ResetPlayerStats()
    {
        PlayerController.Ins.currentHealth = PlayerController.Ins.curPlayerMaxHP;
        DynamicDataManager.Ins.OnHealthChange?.Invoke(PlayerController.Ins.curPlayerMaxHP);
    }

    #endregion
}
