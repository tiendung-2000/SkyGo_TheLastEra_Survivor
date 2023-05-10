using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAimming : MonoBehaviour
{
    public GameObject shootPointParent;
    //public Transform shootPointScale;
    public float rotate = 1f;

    private void Update()
    {
        EnemyAimingSystem();
    }

    public void EnemyAimingSystem()
    {
        GameObject[] gos;
        float distance = Mathf.Infinity;
        gos = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest = null;
        Vector3 position = transform.position;

        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }

        if (closest != null)
        {
            shootPointParent.transform.right = closest.transform.position - transform.position;
            Debug.DrawLine(gameObject.transform.position, closest.transform.position, Color.red);

            if (this.transform.position.x > closest.transform.position.x)
            {
                transform.localScale = Vector3.one;
                shootPointParent.transform.localScale = new Vector3(-1f, -1f, rotate);
            }
            else
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                shootPointParent.transform.localScale = Vector3.one;
            }
        }
    }
}
