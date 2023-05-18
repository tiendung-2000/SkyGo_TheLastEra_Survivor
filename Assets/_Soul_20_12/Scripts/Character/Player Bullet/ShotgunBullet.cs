using DG.Tweening;
using UnityEngine;

public class ShotgunBullet : MonoBehaviour
{
    public float speed = 15f;
    public float knockBack;

    public Rigidbody2D theRB;

    public GameObject impactEffect;

    public int damageToGive = 50;

    // Update is called once per frame

    public TrailRenderer trail;

    private void OnDisable()
    {
        trail.Clear();
    }
    void OnEnable()
    {
        theRB.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Instantiate(impactEffect, transform.position, transform.rotation);

        SmartPool.Ins.Despawn(gameObject);

        //if (other.gameObject.CompareTag("Enemy"))
        //{
        //    Rigidbody2D enemy = other.GetComponent<Rigidbody2D>();
        //    if (enemy != null)
        //    {
        //        //Debug.Log("vaoday");
        //        Vector2 difference = enemy.transform.position - transform.position;
        //        Vector2 endPos = new Vector2(enemy.transform.position.x + difference.x * knockBack, enemy.transform.position.y + difference.y * knockBack);
        //        enemy.DOMove(endPos, .25f).SetEase(Ease.Linear);
        //    }
        //}

        //if (other.tag == "Enemy")
        //{
        //    other.GetComponent<EnemyController>().DamageEnemy(damageToGive);
        //}

        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().DamageEnemy(damageToGive + PlayerController.Ins.playerBaseDamage);

            Rigidbody2D enemy = other.GetComponent<Rigidbody2D>();
            if (enemy != null)
            {
                Vector2 difference = enemy.transform.position - transform.position;
                Vector2 endPos = new Vector2(enemy.transform.position.x + difference.x * knockBack, enemy.transform.position.y + difference.y * knockBack);
                enemy.DOMove(endPos, .25f).SetEase(Ease.Linear);
            }
        }


        if (other.CompareTag("Boss"))
        {
            BossController.Ins.TakeDamage(damageToGive + PlayerController.Ins.playerBaseDamage);

            SmartPool.Ins.Spawn(BossController.Ins.hitEffect, transform.position, transform.rotation);
        }
    }
}
