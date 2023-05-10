using UnityEngine;

public class IgnoreBullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BlockBullet"))
        {
            SmartPool.Ins.Despawn(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("BlockBullet"))
        {
            SmartPool.Ins.Despawn(gameObject);
        }
    }
}
