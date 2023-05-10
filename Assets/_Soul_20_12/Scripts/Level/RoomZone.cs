using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomZone : MonoBehaviour
{
    public List<EnemyController> myEnemies;
    public List<BossController> myBoss;

    private void Start()
    {
        myEnemies = GetComponentInParent<RoomCenter>().enemies;
        myBoss = GetComponentInParent<RoomCenter>().bosses;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //foreach (EnemyController enemy in myEnemies)
            //{
            //    //enemy.playerOnZone = true;
            //}
        }

        if (collision.CompareTag("Player"))
        {
            //StartCoroutine(IEBossStartAction());
        }
    }

    //IEnumerator IEBossStartAction()
    //{
    //    yield return new WaitForSeconds(5f);
    //    foreach (BossController boss in myBoss)
    //    {
    //        //BossController.Ins.playerOnZone = true;
    //    }
    //}
}
