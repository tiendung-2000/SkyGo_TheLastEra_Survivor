using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightlingBullet : MonoBehaviour
{
    public float speed = 15f;
    public Rigidbody2D theRB;

    public GameObject impactEffect;
    public int damageToGive = 50;

    //public TrailRenderer trail;

    [SerializeField]
    private LR_Controller line;

    private Transform tf;
    public Transform m_Transform
    {
        get
        {
            if (tf == null)
            {
                tf = transform;
            }
            return tf;
        }
    }

    //private void OnDisable()
    //{
    //    trail.Clear();
    //}

    private void OnEnable()
    {
        nearestEnemies.Clear();
        Array.Clear(enemies, 0, enemies.Length);
    }

    void Update()
    {
        theRB.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Spawn impact effect
        Instantiate(impactEffect, transform.position, transform.rotation);

        // Despawn the object

        //Find all enemy
        enemies = FindObjectsOfType<EnemyController>();
        FindThreeNearestEnemies();

        AudioManager.Ins.SoundEffect(6);

        // Check the tag of the collided object
        switch (other.tag)
        {
            case "Enemy":
                // Apply damage to the enemy
                EnemyController enemy = other.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(damageToGive + PlayerController.Ins.playerBaseDamage);
                    ShockEnemiesAround();
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

    public List<EnemyController> nearestEnemies;
    [SerializeField]
    private EnemyController[] enemies;
    private void ShockEnemiesAround()
    {
        LR_Controller lineSpawn = Instantiate(line);
        lineSpawn.SetUpLine(nearestEnemies.ToArray());
        lineSpawn.GiveDamageToE(damageToGive);
    }

    void FindThreeNearestEnemies()
    {
        int numNearestEnemiesToFind = Mathf.Min(3, enemies.Length);

        for (int i = 0; i < numNearestEnemiesToFind; i++)
        {
            EnemyController nearestEnemy = FindNearestEnemyNotInList();
            if (nearestEnemy != null)
            {
                nearestEnemies.Add(nearestEnemy);
            }
        }
    }

    EnemyController FindNearestEnemyNotInList()
    {
        float minDistance = Mathf.Infinity;
        EnemyController nearestEnemy = null;

        foreach (EnemyController enemy in enemies)
        {
            if (enemy == null || nearestEnemies.Contains(enemy)) continue;

            float distance = Vector2.Distance(m_Transform.position, enemy.transform.position);

            //Debug.LogWarning(m_Transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

}
