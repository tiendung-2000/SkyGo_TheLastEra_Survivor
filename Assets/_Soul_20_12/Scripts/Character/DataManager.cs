using API.UI;
using Spine.Unity;
using System.Collections;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    [SerializeField] float immortalCount;
    [SerializeField] float damageInvincLength = 2f;


    #region PlayerHealthController


    void Update()
    {
        if (immortalCount > 0)
        {
            immortalCount -= Time.deltaTime;

            if (immortalCount <= 0)
            {
                //PlayerController.Ins.color = new Color(PlayerController.Ins.bodySR.color.r, PlayerController.Ins.bodySR.color.g, PlayerController.Ins.bodySR.color.b, 1f);
            }
        }
    }


    public void DamagePlayer()
    {
        if (immortalCount <= 0)
        {
            PlayerController.Ins.TakeDamageEffect();
            PlayerController.Ins.currentHealth--;
            DynamicDataManager.Ins.OnHealthChange?.Invoke(PlayerController.Ins.currentHealth);

            immortalCount = damageInvincLength;

            if (PlayerController.Ins.currentHealth <= 0)
            {
                PlayerController.Ins.col.enabled = false;
                PlayerController.Ins.canMove = false;
                StartCoroutine(DelayDead());
                PlayerController.Ins.SetCharacterState("Die");
            }
        }
    }

    

    IEnumerator DelayDead()
    {
        yield return new WaitForSeconds(1f);

        PlayerController.Ins.gameObject.SetActive(false);

        CanvasManager.Ins.OpenUI(UIName.RevivePopup, null);
    }
    public void HealPlayer(int healAmount)
    {
        int curPlayerMaxHP = ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.HP[DynamicDataManager.Ins.CurPlayerHPUpgrade];
        PlayerController.Ins.currentHealth += healAmount;
        if (PlayerController.Ins.currentHealth > curPlayerMaxHP)
        {
            PlayerController.Ins.currentHealth = curPlayerMaxHP;
        }
        PlayerHub.Ins.OnHealthChange(PlayerController.Ins.currentHealth);
    }



    #endregion
}
