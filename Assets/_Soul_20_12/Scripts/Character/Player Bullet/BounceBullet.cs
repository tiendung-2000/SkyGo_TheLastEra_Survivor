using System.Collections;
using UnityEngine;

public class BounceBullet : MonoBehaviour
{

    public float speed = 7.5f;
    public Rigidbody2D theRB;

    public GameObject impactEffect;

    public int damageToGive = 50;

    public int bounceCount;
    public int currentBounceCount;

    public float waitToBeDisappear = 5f;

    Vector3 lastVelocity;

    private void Awake()
    {
        theRB = GetComponent<Rigidbody2D>();
    }

    public TrailRenderer trail;

    private void OnDisable()
    {
        trail.Clear();
    }

    private void OnEnable()
    {
        currentBounceCount = 0;
        StartCoroutine(IEDisappear());
    }

    IEnumerator IEDisappear()
    {
        yield return new WaitForSeconds(waitToBeDisappear);
        SmartPool.Ins.Despawn(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        theRB.velocity = transform.right * speed;
    }

    private void FixedUpdate()
    {
        lastVelocity = theRB.velocity;
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    Instantiate(impactEffect, transform.position, transform.rotation);

    //    if (other.tag == "Enemy")
    //    {
    //        other.GetComponent<EnemyController>().DamageEnemy(damageToGive);

    //    }

    //    if (other.tag == "Boss")
    //    {
    //        BossController.Ins.TakeDamage(damageToGive);

    //        Instantiate(BossController.Ins.hitEffect, transform.position, transform.rotation);
    //    }
    //}

    //private void OnCollisionEnter2D(Collision2D other)
    //{
    //    if (currentBounceCount == bounceCount)
    //    {
    //        Instantiate(impactEffect, transform.position, transform.rotation);
    //        SmartPool.Ins.Despawn(gameObject);
    //    }
    //    else
    //    {
    //        if (other.gameObject.CompareTag("Block") /*|| other.gameObject.CompareTag("BlockBullet")*/)
    //        {
    //            var directionValue = Vector2.Reflect(lastVelocity.normalized, other.contacts[0].normal);
    //            transform.right = directionValue;
    //            lastVelocity = directionValue * Mathf.Max(speed, 0f);
    //            theRB.AddForce(new Vector2(Random.Range(-5, 5), Random.Range(-5, 5)));

    //            currentBounceCount++;
    //        }

    //        if (other.gameObject.CompareTag("Enemy"))
    //        {
    //            other.gameObject.GetComponent<EnemyController>().DamageEnemy(damageToGive);
    //            Instantiate(impactEffect, transform.position, transform.rotation);
    //            SmartPool.Ins.Despawn(gameObject);
    //        }

    //        if (other.gameObject.CompareTag("Boss"))
    //        {
    //            other.gameObject.GetComponent<BossController>().TakeDamage(damageToGive);
    //            Instantiate(impactEffect, transform.position, transform.rotation);
    //            SmartPool.Ins.Despawn(gameObject);
    //        }
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (currentBounceCount == bounceCount)
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
            SmartPool.Ins.Despawn(gameObject);
        }

        if (other.gameObject.CompareTag("Block"))
        {
            var directionValue = Vector2.Reflect(lastVelocity.normalized, other.contacts[0].normal);
            transform.right = directionValue;
            lastVelocity = directionValue * Mathf.Max(speed, 0f);
            theRB.AddForce(new Vector2(Random.Range(-5, 5), Random.Range(-5, 5)));

            currentBounceCount++;
        }
        //switch (other.gameObject.tag)
        //{
        //    //case "Block":
        //    //    var directionValue = Vector2.Reflect(lastVelocity.normalized, other.contacts[0].normal);
        //    //    transform.right = directionValue;
        //    //    lastVelocity = directionValue * Mathf.Max(speed, 0f);
        //    //    theRB.AddForce(new Vector2(Random.Range(-5, 5), Random.Range(-5, 5)));

        //    //    currentBounceCount++;
        //    //    break;

        //    case "Enemy":
        //        other.gameObject.GetComponent<EnemyController>().DamageEnemy(damageToGive);
        //        Instantiate(impactEffect, transform.position, transform.rotation);
        //        SmartPool.Ins.Despawn(gameObject);
        //        Debug.Log("Dame");
        //        break;

        //    case "Boss":
        //        other.gameObject.GetComponent<BossController>().TakeDamage(damageToGive);
        //        Instantiate(impactEffect, transform.position, transform.rotation);
        //        SmartPool.Ins.Despawn(gameObject);
        //        break;

        //    default:
        //        break;
        //}
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            //case "Block":
            //    var directionValue = Vector2.Reflect(lastVelocity.normalized, other.contacts[0].normal);
            //    transform.right = directionValue;
            //    lastVelocity = directionValue * Mathf.Max(speed, 0f);
            //    theRB.AddForce(new Vector2(Random.Range(-5, 5), Random.Range(-5, 5)));

            //    currentBounceCount++;
            //    break;

            case "Enemy":
                other.gameObject.GetComponent<EnemyController>().DamageEnemy(damageToGive + PlayerController.Ins.playerBaseDamage);
                Instantiate(impactEffect, transform.position, transform.rotation);
                SmartPool.Ins.Despawn(gameObject);
                Debug.Log("Dame");
                break;

            case "Boss":
                other.gameObject.GetComponent<BossController>().TakeDamage(damageToGive + PlayerController.Ins.playerBaseDamage);
                Instantiate(impactEffect, transform.position, transform.rotation);
                SmartPool.Ins.Despawn(gameObject);
                break;

            default:
                break;
        }
    }
}