using DG.Tweening;
using Spine.Unity;
using System.Collections;
using UnityEngine;

public class StardustGuardian : MonoBehaviour
{
    public static StardustGuardian Ins;

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
        bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.MN5_IDLE, false);
    }

    private void AnimationState_Complete(Spine.TrackEntry trackEntry)
    {
        switch (trackEntry.Animation.Name)
        {
            case AnimationKeys.MN5_IDLE:
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.MN5_SKILL_1, false);
                shootFirst = true;
                break;
            case AnimationKeys.MN5_SKILL_1:
                shootFirst = false;
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.MN5_SKILL_2, false);
                shootSecond = true;
                break;
            case AnimationKeys.MN5_SKILL_2:
                shootSecond = false;
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.MN5_IDLE, false);
                break;
            //case AnimationKeys.MN5_DIE:
            //    Destroy(gameObject);
            //    break;
        }
    }

    private void Update()
    {
        shootCounter -= Time.deltaTime;

        if (shootFirst == true)
        {
            ShootFirst();
        }

        if (shootSecond == true)
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
                SmartPool.Ins.Spawn(bulletFirst, t.position, t.rotation);
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
}
