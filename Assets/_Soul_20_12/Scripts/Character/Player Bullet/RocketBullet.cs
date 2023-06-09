using DG.Tweening;
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
    public Vector3 triggerPosition;

    Tween impactTween;

    private void OnTriggerEnter2D(Collider2D other)
    {
        triggerPosition = this.transform.position;

        impactTween?.Kill();

        impactTween = DOVirtual.DelayedCall(0, () =>
        {
            Instantiate(explodeEffect, triggerPosition, Quaternion.identity);
        }).OnComplete(() =>
        {
            Destroy(gameObject);
        });
        AudioManager.Ins.SoundEffect(8);

        switch (other.tag)
        {
            case "Block":
                //Debug.Log("Rocket");

                //Destroy(gameObject);
                break;
            case "Enemy":
                EnemyController enemy = other.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(damageToGive + PlayerController.Ins.playerBaseDamage);
                    //SmartPool.Ins.Despawn(gameObject);
                }
                break;
            case "Boss":
                BossController boss = other.GetComponent<BossController>();
                if (boss != null)
                {
                    boss.TakeDamage(damageToGive + PlayerController.Ins.playerBaseDamage);
                    Instantiate(boss.hitEffect, transform.position, transform.rotation);
                    //SmartPool.Ins.Despawn(gameObject);
                }
                break;
        }
    }
}
