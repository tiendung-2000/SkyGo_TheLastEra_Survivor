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
        Destroy(this.gameObject, 0.5f);
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
        lr.positionCount = position.Length;
    }

    public void AssignTarget(Vector3 startPosition, Transform newTarget)
    {
        lr.positionCount = 2;
        lr.SetPosition(0, startPosition);
        startPoint = newTarget;
    }


    private void Update()
    {

        LightConnect();
        LightRender();
    }


    void LightConnect()
    {
        //if (startPoint != null)
        //{
        //    lr.SetPosition(0, startPoint.position);

        //    for (int i = 1; i < ePos.Length; i++)
        //    {
        //        if (ePos[i] != null)
        //        {
        //            lr.SetPosition(i, ePos[i].transform.position);
        //        }
        //    }

        //}
        //else
        //{
        //    for (int i = 0; i < ePos.Length; i++)
        //    {
        //        if (ePos[i] != null)
        //        {
        //            lr.SetPosition(i, ePos[i].transform.position);
        //        }
        //    }

        //    //for (int i = 0; i < ePos.Length - 1; i++)
        //    //{
        //    //    if (ePos[i] != null)
        //    //    {
        //    //        lr.SetPosition(0, ePos[i].transform.position);
        //    //        lr.SetPosition(1, ePos[i + 1].transform.position);
        //    //    }
        //    //}
        //}

        

        GenLine(0);

        void GenLine(int index)
        {
            lr.positionCount++;

            if (lr.positionCount == 1)
            {
                lr.SetPosition(0, ePos[index].transform.position);
                GenLine(1);
                return;
            }

            var lastIndex = lr.positionCount - 1;
            var curPos = lr.GetPosition(lastIndex - 1);
            lr.SetPosition(lastIndex, curPos);

            DOVirtual.Vector3(curPos, ePos[index].transform.position, 0.5f, (value) =>
            {
                lr.SetPosition(lastIndex, value);
            }).OnComplete(() =>
            {
                if (index + 1 < ePos.Length)
                {
                    GenLine(index + 1);
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
