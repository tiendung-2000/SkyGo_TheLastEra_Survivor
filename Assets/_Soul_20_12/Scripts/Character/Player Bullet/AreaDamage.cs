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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x);

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
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x);
    }
}
