using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayController : Singleton<GamePlayController>
{
    public bool isMusic;
    public bool isSound;

    [SerializeField] float startTimeScale = 1;
    public List<Weapon> weapons;

    public ParticleSystem outGateFX;

    #region GameLoop

    public void GunSetUp()
    {
        if (DynamicDataManager.Ins.CurPlayer == 1)
        {
            //ResourceSystem.Ins.players[DynamicDataManager.Ins.CurPlayer].availableDupliGuns = weapons;

            StartCoroutine(IEWeaponSetUp());
        }
        else
        {
            return;
        }
    }

    IEnumerator IEWeaponSetUp()
    {
        yield return new WaitForSeconds(2.5f);
        foreach (Weapon weap in weapons)
        {
            Weapon weapClone = Instantiate(weap);
            weapClone.transform.parent = PlayerController.Ins.theHand;
            weapClone.transform.position = new Vector2(PlayerController.Ins.theHand.position.x - 0.2f, PlayerController.Ins.theHand.position.y + 0.2f);
            weapClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
            weapClone.transform.localScale = Vector3.one;
            PlayerController.Ins.availableDupliGuns.Add(weapClone);
        }
    }

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
