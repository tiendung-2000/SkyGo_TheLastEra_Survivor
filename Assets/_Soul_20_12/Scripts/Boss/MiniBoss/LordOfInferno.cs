using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LordOfInferno : MonoBehaviour
{
    public static LordOfInferno Ins;

    BossController bossController;

    //public SkeletonAnimation ske;

    [Header("Shooting")]
    public float xAngle;
    public float yAngle;
    public Transform[] shotPointsFirst;
    public Transform[] shotPointsSecond;
    public GameObject bulletFirst;
    public GameObject bulletSecond;
    public float fireRateFirst;
    public float fireRateSecond;
    public float shootCounter;
    public bool shootFirst = false;
    public bool shootSecond = false;
    private void Awake()
    {
        if (Ins == null)
        {
            Ins = this;
        }
    }

    private void Start()
    {
        bossController = GetComponent<BossController>();
        bossController.ske.AnimationState.Complete += AnimationState_Complete;
        StartCoroutine(IEInitAnim());
    }

    IEnumerator IEInitAnim()
    {
        yield return new WaitForSeconds(1f);
        bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.MN4_IDLE, false);
    }
    private void AnimationState_Complete(Spine.TrackEntry trackEntry)
    {
        switch (trackEntry.Animation.Name)
        {
            case AnimationKeys.MN4_IDLE:
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.MN4_SKILL_1_READY, false);
                break;
            case AnimationKeys.MN4_SKILL_1_READY:
                shootFirst = true;
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.MN4_SKILL_1_ATTACK, false);
                AudioManager.Ins.SoundEffect(7);
                break;
            case AnimationKeys.MN4_SKILL_1_ATTACK:
                shootFirst = false;
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.MN4_SKILL_2_READY, false);
                break;
            case AnimationKeys.MN4_SKILL_2_READY:
                shootSecond = true;
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.MN4_SKILL_2_ATTACK, false);
                AudioManager.Ins.SoundEffect(7);
                break;
            case AnimationKeys.MN4_SKILL_2_ATTACK:
                shootSecond = false;
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.MN4_IDLE, false);
                break;
            //case AnimationKeys.MN4_DIE:
            //    Destroy(gameObject);
            //    break;
        }
    }

    private void Update()
    {
        shootCounter -= Time.deltaTime;

        if (shootFirst)
        {
            ShootFirst();
        }

        if (shootSecond)
        {
            ShootSecond();
        }
    }

    public void ShootFirst()
    {
        if (shootCounter <= 0 && this.gameObject.activeSelf && bossController.currentHealth > 0)
        {
            shootCounter = fireRateFirst;

            foreach (Transform t in shotPointsFirst)
            {
                var newBullet = SmartPool.Ins.Spawn(bulletFirst, t.position, t.rotation);
                newBullet.transform.Rotate(0f, 0f, Random.Range(-xAngle, yAngle));
            }
        }
    }

    public void ShootSecond()
    {
        if (shootCounter <= 0 && this.gameObject.activeSelf && bossController.currentHealth > 0)
        {
            shootCounter = fireRateSecond;

            foreach (Transform t in shotPointsSecond)
            {
                SmartPool.Ins.Spawn(bulletSecond, t.position, t.rotation);
            }
        }
    }

    ////==================SETING ACTION==================//
    //public void ShootAction(int type)
    //{
    //    // type == 0 => ban 5 vien dan || type == 1 => ban 3 vien dan
    //    if (bossController.currentHealth > 0)
    //    {
    //        if (type == 0)
    //        {
    //            if (shootSt != null) StopCoroutine(shootSt);
    //            shootSt = StartCoroutine(DelayShootFirst());
    //        }
    //        else if (type == 1)
    //        {
    //            if (shootNd != null) StopCoroutine(shootNd);
    //            shootNd = StartCoroutine(DelayShootSecond());
    //        }
    //    }
    //}

    //IEnumerator Starting()
    //{
    //    yield return new WaitForSeconds(delayStarting);
    //    shootFirst = false;
    //    ShootAction(0);
    //}

    //Coroutine shootSt;
    //IEnumerator DelayShootFirst()
    //{
    //    shootFirst = true;
    //    yield return new WaitForSeconds(delayAction);
    //    shootFirst = false;
    //    DOVirtual.DelayedCall(1, () =>
    //    {
    //        ShootAction(1);
    //    });
    //}

    //Coroutine shootNd;
    //IEnumerator DelayShootSecond()
    //{
    //    shootSecond = true;
    //    yield return new WaitForSeconds(delayAction);
    //    shootSecond = false;
    //    DOVirtual.DelayedCall(1, () =>
    //    {
    //        ShootAction(0);
    //    });
    //}
}
