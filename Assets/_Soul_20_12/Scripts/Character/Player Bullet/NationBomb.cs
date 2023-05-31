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
        AudioManager.Ins.SoundEffect(8);

        switch (other.tag)
        {

            case "Enemy":
                AudioManager.Ins.SoundEffect(8);

                EnemyController enemy = other.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(damageToGive + PlayerController.Ins.playerBaseDamage);
                }
                break;
            case "Boss":
                AudioManager.Ins.SoundEffect(8);

                BossController boss = other.GetComponent<BossController>();
                if (boss != null)
                {
                    boss.TakeDamage(damageToGive + PlayerController.Ins.playerBaseDamage);
                    Instantiate(boss.hitEffect, transform.position, transform.rotation);
                }
                break;
        }
    }
}
