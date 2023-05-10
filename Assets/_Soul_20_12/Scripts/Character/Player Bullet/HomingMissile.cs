using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 180f;
    public float homingRange = 5f;
    public float homingAngle = 30f;

    private Transform target;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        FindTarget();
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            float angle = Vector2.Angle(transform.up, direction);

            if (angle < homingAngle)
            {
                rb.rotation -= rotationSpeed * Time.fixedDeltaTime;
            }
            else
            {
                rb.rotation += rotationSpeed * Time.fixedDeltaTime;
            }

            rb.velocity = transform.up * speed;
        }
        else
        {
            FindTarget();
            rb.velocity = transform.right * speed;
        }
    }

    void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance < homingRange && distance < closestDistance)
            {
                target = enemy.transform;
                closestDistance = distance;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
}