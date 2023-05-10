using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EliminatedBishopBoss : MonoBehaviour
{
    public static EliminatedBishopBoss Ins;

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
    public Transform[] shotPointsFive;

    public GameObject bulletFirst;
    public GameObject bulletSecond;
    public GameObject bulletFour;
    public GameObject bulletFive;

    public float fireRateFirst;
    public float fireRateSecond;
    public float fireRateFour;
    public float fireRateFive;

    public float shootCounter;

    public bool phaseFirst = false;
    public bool phaseSecond = false;
    public bool phaseFour = false;
    public bool phaseFive = false;
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
    private void AnimationState_Complete(Spine.TrackEntry trackEntry)
    {
        switch (trackEntry.Animation.Name)
        {
            case AnimationKeys.B2_MOVE:
                shouldMove = false;
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.B2_ATTACK_1, false);
                phaseFirst = true;
                break;

            case AnimationKeys.B2_ATTACK_1:
                phaseFirst = false;
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.B2_ATTACK_2, false);
                phaseSecond = true;
                break;

            case AnimationKeys.B2_ATTACK_2:
                phaseSecond = false;
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.B2_ATTACK_3_READY, false);
                break;

            case AnimationKeys.B2_ATTACK_3_READY:
                Dash();
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.B2_ATTACK_3_MOVE, false);
                break;

            case AnimationKeys.B2_ATTACK_3_MOVE:
                phaseFour = true;
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.B2_ATTACK_4, false);
                break;

            case AnimationKeys.B2_ATTACK_4:
                phaseFour = false;
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.B2_ATTACK_5, false);
                phaseFive = true;
                break;

            case AnimationKeys.B2_ATTACK_5:
                phaseFive = false;
                shouldMove = true;
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.B1_MOVE, false);
                break;

            //case AnimationKeys.B2_DIE:
            //    Destroy(gameObject);
            //    break;
        }

    }

    private void Update()
    {
        shootCounter -= Time.deltaTime;

        if (shouldMove)
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

        if (phaseFive)
        {
            PhaseFive();
        }
    }

    public void Moving()
    {
        if (bossController.currentHealth > 0 && shouldMove == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(PlayerController.Ins.transform.position.x - 2f, PlayerController.Ins.transform.position.y - 2f), moveSpeed * Time.deltaTime);
            moveDirection.Normalize();
        }
    }
    public void PhaseFirst()
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
    public void PhaseSecond()
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
            shootCounter = fireRateFour;

            foreach (Transform t in shotPointsFour)
            {
                SmartPool.Ins.Spawn(bulletFour, t.position, t.rotation);
            }
        }
    }
    public void PhaseFive()
    {
        if (shootCounter <= 0 && this.gameObject.activeSelf && bossController.currentHealth > 0)
        {
            shootCounter = fireRateFive;
            foreach (Transform t in shotPointsFive)
            {
                SmartPool.Ins.Spawn(bulletFive, t.position, t.rotation);
            }
        }
    }
}
