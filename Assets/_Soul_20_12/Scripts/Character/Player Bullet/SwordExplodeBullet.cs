using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordExplodeBullet : MonoBehaviour
{
    public static SwordExplodeBullet Ins;

    public float speed = 15f;
    public Rigidbody2D theRB;

    public GameObject impactEffect;

    public GameObject explodeEffect;

    public int damageToGive;

    public TrailRenderer trail;

    private void Awake()
    {
        Ins = this;
    }

    private void OnDisable()
    {
        trail.Clear();
    }

    void Update()
    {
        theRB.velocity = transform.right * speed;
    }
    public Vector3 triggerPosition;

    private void OnTriggerEnter2D(Collider2D other)
    {
        triggerPosition = this.transform.position;

        Instantiate(impactEffect, triggerPosition, Quaternion.identity);

        AudioManager.Ins.SoundEffect(8);

        if (other.tag == "Block")
        {
            Instantiate(explodeEffect, triggerPosition, transform.rotation);
            SmartPool.Ins.Despawn(gameObject);
        }

        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyController>().DamageEnemy(damageToGive + PlayerController.Ins.playerBaseDamage);
            Instantiate(explodeEffect, triggerPosition, transform.rotation);
        }

        if (other.tag == "Boss")
        {
            BossController.Ins.TakeDamage(damageToGive + PlayerController.Ins.playerBaseDamage);
            Instantiate(explodeEffect, triggerPosition, transform.rotation);
            Instantiate(BossController.Ins.hitEffect, triggerPosition, transform.rotation);
        }
    }
}
