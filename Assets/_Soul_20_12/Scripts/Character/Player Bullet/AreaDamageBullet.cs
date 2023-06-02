using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamageBullet : MonoBehaviour
{
    public float speed = 15f;
    public Rigidbody2D theRB;

    public GameObject areaDamage;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Spawn impact effect
        Instantiate(impactEffect, transform.position, transform.rotation);
        Instantiate(areaDamage, transform.position, transform.rotation);

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
        SmartPool.Ins.Despawn(gameObject);
    }
}
