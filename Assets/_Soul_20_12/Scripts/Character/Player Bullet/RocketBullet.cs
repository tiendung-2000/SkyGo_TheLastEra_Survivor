using UnityEngine;

public class RocketBullet : MonoBehaviour
{
    public GameObject explodeEffect;

    public int damageToGive;

    public TrailRenderer trail;

    private void OnDisable()
    {
        trail.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Instantiate(explodeEffect, transform.position, transform.rotation);
        AudioManager.Ins.SoundEffect(8);

        switch (other.tag)
        {
            case "Block":
                //Debug.Log("Rocket");

                SmartPool.Ins.Despawn(gameObject);
                break;
            case "Enemy":
                EnemyController enemy = other.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(damageToGive + PlayerController.Ins.playerBaseDamage);
                    SmartPool.Ins.Despawn(gameObject);
                }
                break;
            case "Boss":
                BossController boss = other.GetComponent<BossController>();
                if (boss != null)
                {
                    boss.TakeDamage(damageToGive + PlayerController.Ins.playerBaseDamage);
                    Instantiate(boss.hitEffect, transform.position, transform.rotation);
                    SmartPool.Ins.Despawn(gameObject);
                }
                break;
        }
    }
}
