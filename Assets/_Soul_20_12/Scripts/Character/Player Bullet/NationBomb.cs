using UnityEngine;

public class NationBomb : MonoBehaviour
{
    public int damageToGive = 50;

    private void OnEnable()
    {
        CinemachineShake.Instance.ShakeCamera(3f, .5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Enemy":
                EnemyController enemy = other.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(damageToGive);
                }
                break;
            case "Boss":
                BossController boss = other.GetComponent<BossController>();
                if (boss != null)
                {
                    boss.TakeDamage(damageToGive);
                    Instantiate(boss.hitEffect, transform.position, transform.rotation);
                }
                break;
        }
    }
}
