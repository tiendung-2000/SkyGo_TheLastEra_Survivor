using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSlash : MonoBehaviour
{
    public int damageToGive;
    public float knockBack;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().DamageEnemy(damageToGive);

            Rigidbody2D enemy = other.GetComponent<Rigidbody2D>();
            if (enemy != null)
            {
                Vector2 difference = enemy.transform.position - transform.position;
                Vector2 endPos = new Vector2(enemy.transform.position.x + difference.x * knockBack, enemy.transform.position.y + difference.y * knockBack);
                enemy.DOMove(endPos, .1f).SetEase(Ease.Linear);
            }
        }
    }
}
