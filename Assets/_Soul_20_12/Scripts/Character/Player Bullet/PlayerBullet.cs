using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 15f;
    public Rigidbody2D theRB;

    public GameObject impactEffect;
    public int damageToGive = 50;

    public TrailRenderer trail;

    private void OnDisable()
    {
        trail.Clear();
    }

    void Update()
    {
        theRB.velocity = transform.right * speed;
    }

    public Vector3 triggerPosition;

    Tween impactTween;

    private void OnTriggerEnter2D(Collider2D other)
    {
        triggerPosition = this.transform.position;
        // Spawn impact effect
        impactTween?.Kill();

        impactTween = DOVirtual.DelayedCall(0, () =>
        {
            Instantiate(impactEffect, triggerPosition, Quaternion.identity);
        }).OnComplete(() =>
        {
            SmartPool.Ins.Despawn(gameObject);
        });
        // Check the tag of the collided object
        switch (other.tag)
        {
            case "Enemy":
                // Apply damage to the enemy
                EnemyController enemy = other.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(damageToGive + PlayerController.Ins.playerBaseDamage);
                }
                break;
            case "Boss":
                // Apply damage to the boss and spawn hit effect
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
