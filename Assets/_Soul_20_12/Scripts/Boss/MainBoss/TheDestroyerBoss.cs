using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheDestroyerBoss : MonoBehaviour
{
    public static TheDestroyerBoss Ins;

    BossController bossController;

    //public SkeletonAnimation ske;

    [Header("Moving")]
    public bool shouldMove;
    public float moveSpeed;
    public Rigidbody2D theRB;
    private Vector2 moveDirection;

    [Header("Shooting")]
    public float xAngle;
    public float yAngle;

    public Transform[] shotPointsFirst;
    public Transform[] shotPointsSecond;
    public Transform[] shotPointsFour;

    public GameObject bulletFirst;
    public GameObject bulletSecond;
    public GameObject bulletFour;

    public float fireRateFirst;
    public float fireRateSecond;
    public float fireRateFour;

    public float shootCounter;

    public bool phaseFirst = false;
    public bool phaseSecond = false;
    public bool phaseFour = false;
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
        if (shouldMove == true)
        {
            bossController.ske.AnimationState.SetAnimation(0, "Move", false);
        }
    }

    private void Update()
    {
        shootCounter -= Time.deltaTime;

        if (shouldMove == true)
        {
            Moving();
        }

        if (phaseFirst)
        {
            PhaseFirst();
        }

        if (phaseSecond)
        {
            PhaseSecond();
        }

        if (phaseFour)
        {
            PhaseFour();
        }
    }

    private void AnimationState_Complete(Spine.TrackEntry trackEntry)
    {
        switch (trackEntry.Animation.Name)
        {
            case AnimationKeys.B1_MOVE:
                shouldMove = false;
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.B1_SKILL_1_READY, false);
                break;

            //ban 10 vien dan
            case AnimationKeys.B1_SKILL_1_READY:
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.B1_SKILL_1_ATTACK, false);
                phaseFirst = true;
                break;

            case AnimationKeys.B1_SKILL_1_ATTACK:
                phaseFirst = false;
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.B1_SKILL_2, false);
                phaseSecond = true;
                break;

            case AnimationKeys.B1_SKILL_2:
                phaseSecond = false;
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.B1_SKILL_3_READY, false);
                break;

            case AnimationKeys.B1_SKILL_3_READY:
                Dash();
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.B1_SKILL_3_ATTACK, false);
                break;

            case AnimationKeys.B1_SKILL_3_ATTACK:
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.B1_SKILL_4_READY, false);
                break;

            case AnimationKeys.B1_SKILL_4_READY:
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.B1_SKILL_4_ATTACK, false);
                phaseFour = true;
                break;

            case AnimationKeys.B1_SKILL_4_ATTACK:
                phaseFour = false;
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.B1_MOVE, false);
                shouldMove = true;
                break;

            //case AnimationKeys.B1_DIE:
            //    Destroy(gameObject);
            //    break;

                //default:
                //    ske.AnimationState.SetAnimation(0, AnimationKeys.B1_MOVE, false);
                //    break;
        }
    }


    public void Moving()
    {
        if (bossController.currentHealth > 0 && shouldMove == true && bossController.currentHealth > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(PlayerController.Ins.transform.position.x - 2f, PlayerController.Ins.transform.position.y - 2f), moveSpeed * Time.deltaTime);
            moveDirection.Normalize();
        }
    }
    public void PhaseFirst()
    {
        if (shootCounter <= 0 && this.gameObject.activeSelf && bossController.currentHealth > 0)
        {
            shouldMove = false;

            shootCounter = fireRateFirst;

            foreach (Transform point in shotPointsFirst)
            {
                SmartPool.Ins.Spawn(bulletFirst, point.position, point.rotation);
            }
        }
    }
    public void PhaseSecond()
    {
        if (shootCounter <= 0 && this.gameObject.activeSelf && bossController.currentHealth > 0)
        {
            shouldMove = false;

            shootCounter = fireRateSecond;

            foreach (Transform point in shotPointsSecond)
            {
                var newBullet = SmartPool.Ins.Spawn(bulletSecond, point.position, point.rotation);
                //newBullet.transform.Rotate(0f, 0f, Random.Range(-xAngle, yAngle));
            }
        }
    }

    Tween OnEnableTween;

    public void Dash()
    {
        OnEnableTween?.Kill();
        if (bossController.currentHealth > 0 && this.gameObject.activeSelf && bossController.currentHealth > 0)
        {
            var pos = PlayerController.Ins.transform.position;
            OnEnableTween = transform.DOMove(new Vector3(pos.x - 1f, pos.y - 1), .6f);
        }
    }

    public void PhaseFour()
    {
        if (shootCounter <= 0 && this.gameObject.activeSelf && bossController.currentHealth > 0)
        {
            shouldMove = false;

            shootCounter = fireRateFour;

            foreach (Transform point in shotPointsFour)
            {
                SmartPool.Ins.Spawn(bulletFour, point.position, point.rotation);
            }
        }
    }
    //public void PhaseFive()
    //{
    //    if (shootCounter <= 0)
    //    {
    //        shouldMove = false;

    //        shootCounter = fireRateFive;

    //        foreach (Transform t in shotPointsFive)
    //        {
    //            SmartPool.Ins.Spawn(bulletFive, t.position, t.rotation);
    //        }
    //    }
    //}
}
