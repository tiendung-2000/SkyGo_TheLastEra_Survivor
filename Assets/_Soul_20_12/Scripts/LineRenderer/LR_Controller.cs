using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LR_Controller : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lr;
    [SerializeField]
    private EnemyController[] ePos;

    [Header("Texture")]
    [SerializeField]
    private Texture[] textures;
    private int animationStep;
    private float fps = 30f;
    private float fpsCounter;

    public Vector3 startPoint;

    private void Awake()
    {
        Destroy(this.gameObject, 0.5f);
    }

    public void GiveDamageToE(int damage)
    {
        for (int i = 0; i < ePos.Length; i++)
        {
            ePos[i].DamageEnemy(damage);
        }
    }

    public void SetUpLine(EnemyController[] position)
    {
        this.ePos = position;
        lr.positionCount = position.Length;
    }

    private void Update()
    {
        if (startPoint != null)
        {
            lr.SetPosition(0, startPoint);

            for (int i = 1; i < ePos.Length; i++)
            {
                if (ePos[i] != null)
                {
                    lr.SetPosition(i, ePos[i].transform.position);
                }
            }

        }
        else
        {
            for (int i = 0; i < ePos.Length; i++)
            {
                if (ePos[i] != null)
                {
                    lr.SetPosition(i, ePos[i].transform.position);
                }
            }
        }
        LightRender();
    }

    private void LightRender()
    {
        fpsCounter += Time.deltaTime;
        if (fpsCounter >= 1f / fps)
        {
            animationStep++;
            if (animationStep == textures.Length)
                animationStep = 0;
            lr.material.SetTexture("_MainTex", textures[animationStep]);
            fpsCounter = 0f;
        }
    }
}
