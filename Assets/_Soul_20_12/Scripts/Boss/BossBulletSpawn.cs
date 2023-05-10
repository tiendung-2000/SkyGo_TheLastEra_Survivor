using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletSpawn : MonoBehaviour
{
    public Transform[] point;
    public GameObject bullet;

    public void Spawn()
    {
        foreach (Transform t in point)
        {
            SmartPool.Ins.Spawn(bullet, t.position, t.rotation);
        }
        SmartPool.Ins.Despawn(this.gameObject);
    }
}
