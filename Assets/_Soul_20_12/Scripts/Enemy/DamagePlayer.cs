using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            DataManager.Ins.DamagePlayer();
            //DynamicDataManager.Ins.IsStayDmg = true;
            //DynamicDataManager.Ins.damagePlayerCooldown = DynamicDataManager.Ins.DamagePlayerCooldownMaxValue;
        }
    }

    //private void OnTriggerStay2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player") && DynamicDataManager.Ins.damagePlayerCooldown < 0f)
    //    {
    //        DataManager.Ins.DamagePlayer();
    //        DynamicDataManager.Ins.damagePlayerCooldown = DynamicDataManager.Ins.DamagePlayerCooldownMaxValue;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player"))
    //        DynamicDataManager.Ins.IsStayDmg = false;
    //}
}
