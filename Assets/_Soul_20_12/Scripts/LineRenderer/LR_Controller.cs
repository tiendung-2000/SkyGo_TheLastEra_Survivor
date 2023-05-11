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


    private void Awake()
    {
        Destroy(this.gameObject, 0.5f);
    }

    public void SetUpLine(EnemyController[] position)
    {
        this.ePos = position;
        lr.positionCount = position.Length;
    }

    private void Update()
    {
        LightRender();
        //print(ePos.Length);
        for (int i = 0; i < ePos.Length; i++)
        {
            lr.SetPosition(i, ePos[i].transform.position);
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
