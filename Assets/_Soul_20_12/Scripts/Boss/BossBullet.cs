using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed;
    private Vector3 direction;
    public GameObject impactEffect;
    public bool hasSpawn;

    // Start is called before the first frame update
    void OnEnable()
    {
        direction = transform.right;

        if (hasSpawn)
        {
            DOVirtual.DelayedCall(1, () =>
            {
                transform.GetComponent<BossBulletSpawn>().Spawn();
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        //if (!BossController.Ins.gameObject.activeInHierarchy)
        //{
        //    SmartPool.Ins.Despawn(gameObject);
        //}
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        SmartPool.Ins.Spawn(impactEffect, transform.position, transform.rotation);

        if (other.tag == "Player")
        {
            DataManager.Ins.DamagePlayer();
        }

        SmartPool.Ins.Despawn(gameObject);

        //AudioManager.instance.PlaySFX(4);
    }
}
