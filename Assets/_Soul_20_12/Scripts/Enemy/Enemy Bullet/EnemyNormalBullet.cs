using UnityEngine;

public class EnemyNormalBullet : MonoBehaviour
{
    public static EnemyNormalBullet instance;

    public float speed;
    private Vector3 direction;
    public Rigidbody2D theRB;
    public GameObject impactEffect;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        direction = PlayerController.Ins.transform.position - transform.position;
        direction.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += direction * speed * Time.deltaTime;
        theRB.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        SmartPool.Ins.Spawn(impactEffect, transform.position, transform.rotation);
        if (other.tag == "Player")
        {
            DataManager.Ins.DamagePlayer();
        }

        if (other.tag == "Shield")
        {
            SmartPool.Ins.Despawn(gameObject);
        }

        SmartPool.Ins.Despawn(gameObject);
        //AudioManager.instance.PlaySFX(4);
    }
}
