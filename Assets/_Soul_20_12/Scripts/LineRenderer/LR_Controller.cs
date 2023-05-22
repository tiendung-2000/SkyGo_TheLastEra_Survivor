using DG.Tweening;
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

    public Transform startPoint;

    private void Awake()
    {
        Destroy(this.gameObject, 5f);
    }

    public void GiveDamageToE(int damage)
    {
        for (int i = 0; i < ePos.Length; i++)
        {
            ePos[i].DamageEnemy(damage + PlayerController.Ins.playerBaseDamage);
        }
    }

    public void SetUpLine(EnemyController[] position)
    {
        this.ePos = position;
        LightConnect();
    }

    public void AssignTarget(Vector3 startPosition, Transform newTarget)
    {
        lr.positionCount = 2;
        lr.SetPosition(0, startPosition);
        startPoint = newTarget;
    }


    private void Update()
    {
        LightRender();
    }


    void LightConnect()
    {
        lr.positionCount = 0;

        List<EnemyController> enemy = new List<EnemyController>();
        foreach (EnemyController e in ePos)
        {
            enemy.Add(e);
        }

        GenLine(0);

        void GenLine(int index)
        {
            lr.positionCount++;

            if (lr.positionCount == 1)
            {
                lr.SetPosition(0, enemy[index].transform.position);
                GenLine(1);
                return;
            }

            var lastIndex = lr.positionCount - 1;
            var curPos = lr.GetPosition(lastIndex - 1);
            lr.SetPosition(lastIndex, curPos);

            DOVirtual.Vector3(curPos, enemy[index].transform.position, 0.1f, (value) =>
            {
                lr.SetPosition(lastIndex, value);
            }).OnComplete(() =>
            {
                if (index + 1 < enemy.Count)
                {
                    GenLine(index + 1);
                }
                else
                {
                    Destroy(this.gameObject);
                }
            });
        }
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
