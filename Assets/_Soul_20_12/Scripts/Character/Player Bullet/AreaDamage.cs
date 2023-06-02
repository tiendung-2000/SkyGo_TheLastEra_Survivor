using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamage : MonoBehaviour
{
    [SerializeField] float timeCounter;
    [SerializeField] float timeToDamage;
    [SerializeField] int damage;

    private void Update()
    {
        timeCounter += Time.deltaTime;
        if (timeCounter >= 1f)
        {
            timeCounter = 0;
            ApplyDamage();
        }
    }

    void ApplyDamage()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x / 2);

        foreach (Collider2D collider in colliders)
        {
            EnemyController enemy = collider.GetComponent<EnemyController>();

            if (enemy != null)
            {
                enemy.DamageEnemy(damage);
            }

            BossController boss = collider.GetComponent<BossController>();

            if(boss != null)
            {
                boss.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Display the range of the area damage in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x / 2);
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (timeCounter < 0)
    //    {
    //        switch (other.tag)
    //        {
    //            case "Enemy":
    //                // Apply damage to the enemy
    //                EnemyController enemy = other.GetComponent<EnemyController>();
    //                if (enemy != null)
    //                {
    //                    enemy.DamageEnemy(damage + Mathf.RoundToInt(PlayerController.Ins.playerBaseDamage / 2));
    //                }
    //                break;
    //            case "Boss":
    //                // Apply damage to the boss and spawn hit effect
    //                BossController boss = other.GetComponent<BossController>();
    //                if (boss != null)
    //                {
    //                    boss.TakeDamage(damage + Mathf.RoundToInt(PlayerController.Ins.playerBaseDamage / 2));
    //                    Instantiate(boss.hitEffect, transform.position, transform.rotation);
    //                }
    //                break;
    //        }
    //    }
    //}
}
