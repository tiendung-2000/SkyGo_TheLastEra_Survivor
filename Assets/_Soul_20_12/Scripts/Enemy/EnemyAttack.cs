using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    EnemyController enemyController;
    EnemyMovement enemyMovement;

    public float xAngle, yAngle;
    [Header("Shooting")]
    public GameObject bullet;
    public List<Transform> firePoint;

    [SerializeField] public float fireRateMin;
    [SerializeField] public float fireRateMax;

    public float fireRate;
    private float fireCounter;

    public float delayLength;//delay between next fire
    public float delayCounter;

    public bool fireDone = false;

    private float waitForShoot = 1.5f;

    //public float spread;

    public float shootRange;
    public int amountOfBullet;

    public Transform target;
    public Transform theHand;

    public GameObject spawnObject;

    [Header("Active Mode")]
    public bool isCanShot;
    public bool isNormalMode; //shot one bullet
    public bool isBurstMode; //shot x(amount of bullet) bullet 1 time

    // Start is called before the first frame update
    void Start()
    {
        target = this.transform;

        enemyController = GetComponent<EnemyController>();
        enemyMovement = GetComponent<EnemyMovement>();

        //LevelManager.Ins.CaculateFireRate(fireRateMin);

        fireRateMin = LevelManager.Ins.CaculateFireRateMin(fireRateMin);
        fireRateMax = LevelManager.Ins.CaculateFireRateMax(fireRateMax);
    }

    private void Update()
    {
        waitForShoot -= Time.deltaTime;
    }

    public void EnemyAimingSystem()
    {
        GameObject[] gos;
        float distance = 500f;
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
            isCanShot = true;
            theHand.right = closest.transform.position - transform.position;
            Debug.DrawLine(gameObject.transform.position, closest.transform.position, Color.red);

            if (this.transform.position.x > closest.transform.position.x)
            {
                transform.localScale = Vector3.one;
                theHand.localScale = new Vector3(-1f, -1f, 1f);
            }
            else
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                theHand.localScale = Vector3.one;
            }
        }
        else
        {
            isCanShot = false;
        }
    }

    public void EnemyShooting()
    {
        fireCounter -= Time.deltaTime;

        if (isCanShot && Vector3.Distance(transform.position, PlayerController.Ins.transform.position) < shootRange && waitForShoot <= 0)
        {
            if (fireDone)
            {
                delayCounter -= Time.deltaTime;
            }

            if (isNormalMode)
            {
                EnemyNormalShoot();
            }

            if (isBurstMode)
            {
                EnemyBurstShoot();
            }
        }
    }

    public void EnemySpawnObject()
    {
        for (int i = 0; i < firePoint.Count; i++)
        {
            SmartPool.Ins.Spawn(spawnObject, firePoint[i].position, Quaternion.identity);
        }
    }

    public void EnemyNormalShoot()
    {
        if (isCanShot && fireCounter <= 0)//normal shoot
        {
            fireRate = Random.Range(fireRateMin, fireRateMax);

            fireCounter = fireRate;
            enemyMovement.isMoving = false;
            enemyController.Ske.AnimationState.SetAnimation(0, Constant.ANIM_ATTACK, false);
            for (int i = 0; i < firePoint.Count; i++)
            {
                var newBullet = SmartPool.Ins.Spawn(bullet, firePoint[i].position, firePoint[i].rotation).AddComponent<PoolIdentify>();
                newBullet.transform.Rotate(0f, 0f, Random.Range(-xAngle, yAngle));
            }

        AudioManager.Ins.PlayGunSound(2);

        }
        //else
        //{
        //    enemyMovement.isMoving = true;
        //}
    }

    public void EnemyBurstShoot()
    {
        if (isBurstMode && delayCounter <= 0 && fireCounter <= 0)//shoot 5 bullet one time, delay 3s
        {
            delayCounter = delayLength;
            fireRate = Random.Range(fireRateMin, fireRateMax);
            fireCounter = fireRate;
            enemyMovement.isMoving = false;

            DelayBulletCoroutine();
            //AudioManager.instance.PlaySFX(13);       
        }
        //else
        //{
        //    enemyMovement.isMoving = true;
        //}
    }

    Coroutine delayBullet;
    public void DelayBulletCoroutine()
    {
        if (delayBullet != null)
        {
            StopCoroutine(delayBullet);
        }
        delayBullet = StartCoroutine(DelayBullet());
    }

    IEnumerator DelayBullet()//delay spawn bullet in burst mode
    {
        fireDone = false;
        enemyController.Ske.AnimationState.SetAnimation(0, Constant.ANIM_ATTACK, false);
        AudioManager.Ins.PlayGunSound(2);

        for (int i = 0; i < amountOfBullet; i++)
        {
            for (int j = 0; j < firePoint.Count; j++)
            {
                var newBullet = SmartPool.Ins.Spawn(bullet, firePoint[j].position, firePoint[j].rotation);
                newBullet.transform.Rotate(0f, 0f, Random.Range(-xAngle, yAngle));
            }
            yield return new WaitForSeconds(fireRate);
        }
        fireDone = true;
    }
}
