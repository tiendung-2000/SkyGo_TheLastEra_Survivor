using UnityEngine;

public class ExplosionBullet : MonoBehaviour
{
    public static ExplosionBullet instance;

    public float speed = 15f;
    public Rigidbody2D theRB;

    public GameObject impactEffect;

    public GameObject explodeEffect;

    public int damageToGive;

    public TrailRenderer trail;

    private void Awake()
    {
        instance = this;
    }

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
        //var tempPos = transform.position;
        //var tempRot = transform.rotation;
        //DOVirtual.DelayedCall(0.5f, () =>
        //{
        //    SmartPool.Ins.Spawn(explodeEffect, tempPos, tempRot);
        //});

        if (other.tag == "Block")
        {
            Instantiate(explodeEffect, transform.position, transform.rotation);
            SmartPool.Ins.Despawn(gameObject);
        }

        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyController>().DamageEnemy(damageToGive);
            Instantiate(explodeEffect, transform.position, transform.rotation);
            SmartPool.Ins.Despawn(gameObject);
        }

        if (other.tag == "Boss")
        {
            BossController.Ins.TakeDamage(damageToGive);

            Instantiate(BossController.Ins.hitEffect, transform.position, transform.rotation);
            SmartPool.Ins.Despawn(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        SmartPool.Ins.Despawn(gameObject);
    }
}
