using UnityEngine;

public class EnemyExplodeBullet : MonoBehaviour
{
    public float speed;
    private Vector3 direction;
    public Rigidbody2D theRB;
    public GameObject explodeEffect;

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
        if (other.tag == "Player")
        {
            Instantiate(explodeEffect, transform.position, Quaternion.identity);
            DataManager.Ins.DamagePlayer();
            SmartPool.Ins.Despawn(gameObject);
        }
        if (other.tag == "Block")
        {
            Instantiate(explodeEffect, transform.position, Quaternion.identity);
            SmartPool.Ins.Despawn(gameObject);
        }

        //AudioManager.instance.PlaySFX(4);
    }

    private void OnBecameInvisible()
    {
        SmartPool.Ins.Despawn(gameObject);
    }
}
