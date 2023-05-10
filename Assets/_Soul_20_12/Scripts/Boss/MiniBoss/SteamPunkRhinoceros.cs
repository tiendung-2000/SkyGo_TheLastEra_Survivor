using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamPunkRhinoceros : MonoBehaviour
{
    public static SteamPunkRhinoceros Ins;
    BossController bossController;
    //public SkeletonAnimation ske;

    [Header("Moving")]
    public bool shouldMove;
    public Rigidbody2D theRB;
    public float moveSpeed;
    private Vector2 moveDirection;

    [Header("Shooting")]
    public Transform[] shotPoints;
    public Transform[] shotPointRadia;
    public GameObject bullet1, bullet2;
    public float timeBetweenShoots; //fireRate
    public float shootCounter;

    public bool shoot;
    public bool shootRadia;

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
            bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.MN1_MOVE, false);
        }
    }
    private void AnimationState_Complete(Spine.TrackEntry trackEntry)
    {
        switch (trackEntry.Animation.Name)
        {
            case AnimationKeys.MN1_MOVE:
                shouldMove = false;
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.MN1_SKILL_1, false);
                shoot = true;
                break;
            case AnimationKeys.MN1_SKILL_1:
                shoot = false;
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.MN1_SKILL_2, false);
                shootRadia = true;
                break;
            case AnimationKeys.MN1_SKILL_2:
                shootRadia = false;
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.MN1_SKILL_3_READY, false);
                break;
            case AnimationKeys.MN1_SKILL_3_READY:
                Dash();
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.MN1_SKILL_3_ATTACK, false);
                break;
            case AnimationKeys.MN1_SKILL_3_ATTACK:
                shouldMove = true;
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.MN1_MOVE, false);
                break;
                //case AnimationKeys.MN1_DIE:
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

        if (shoot == true)
        {
            Shoot();
        }

        if (shootRadia == true)
        {
            ShootRadia();
        }
    }

    public void Moving()
    {
        if (bossController.currentHealth > 0 && shouldMove == true && bossController.currentHealth > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position,
                new Vector2(PlayerController.Ins.transform.position.x - 2f, PlayerController.Ins.transform.position.y - 2f)
                , moveSpeed * Time.deltaTime);
            moveDirection.Normalize();
        }
    }

    Tween OnEnableTween;

    public void Dash()
    {
        OnEnableTween?.Kill();
        if (bossController.currentHealth > 0 && this.gameObject.activeSelf && bossController.currentHealth > 0)
        {
            var pos = PlayerController.Ins.transform.position;
            OnEnableTween = transform.DOMove(new Vector3(pos.x - 1f, pos.y - 1), .833f);
        }
    }

    public void Shoot()
    {
        shouldMove = false;
        if (shootCounter <= 0 && this.gameObject.activeSelf && bossController.currentHealth > 0)
        {
            shootCounter = timeBetweenShoots;
            foreach (Transform t in shotPoints)
            {
                SmartPool.Ins.Spawn(bullet1, t.position, t.rotation);
            }
        }
    }

    public void ShootRadia()
    {
        shouldMove = false;
        if (shootCounter <= 0 && this.gameObject.activeSelf && bossController.currentHealth > 0)
        {
            shootCounter = timeBetweenShoots;
            foreach (Transform t in shotPointRadia)
            {
                SmartPool.Ins.Spawn(bullet2, t.position, t.rotation);
            }
        }
    }
}
