using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBullet : MonoBehaviour
{
    public float speed = 15f;
    public Rigidbody2D theRB;
    public BoxCollider2D col;

    public GameObject impactEffect;
    public int damageToGive = 50;

    public TrailRenderer trail;

    public Transform[] point;
    public GameObject bullet;
    public bool isSpawn;
    public bool isDisableCol;

    private void OnDisable()
    {
        trail.Clear();
    }

    private void OnEnable()
    {
        if(isDisableCol== true) 
        {
            StartCoroutine(IEReActiveCol());  
        }
    }

    void Update()
    {
        theRB.velocity = transform.right * speed;
    }

    public void Spawn()
    {
        if (isSpawn == true)
        {
            foreach (Transform t in point)
            {
                SmartPool.Ins.Spawn(bullet, t.position, t.rotation);
            }
            SmartPool.Ins.Despawn(this.gameObject);
        }
    }

    IEnumerator IEReActiveCol()
    {
        col.enabled = false;
        yield return new WaitForSeconds(.3f);
        col.enabled = true;
    }
    public Vector3 triggerPosition;

    Tween impactTween;

    private void OnTriggerEnter2D(Collider2D other)
    {
        triggerPosition = this.transform.position;
        // Spawn impact effect
        
        Spawn();
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

        impactTween?.Kill();

        impactTween = DOVirtual.DelayedCall(0, () =>
        {
            Instantiate(impactEffect, triggerPosition, Quaternion.identity);
        }).OnComplete(() =>
        {
            SmartPool.Ins.Despawn(gameObject);
        });

        //SmartPool.Ins.Despawn(gameObject);
    }
}
