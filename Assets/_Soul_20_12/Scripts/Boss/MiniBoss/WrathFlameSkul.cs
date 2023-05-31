using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrathFlameSkul : MonoBehaviour
{
    public static WrathFlameSkul Ins;

    BossController bossController;

    //public SkeletonAnimation ske;

    [Header("Moving")]
    public bool shouldMove;
    public Rigidbody2D theRB;
    public float moveSpeed;
    private Vector2 moveDirection;

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
        bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.MN2_MOVE, false);
    }

    private void AnimationState_Complete(Spine.TrackEntry trackEntry)
    {
        switch (trackEntry.Animation.Name)
        {
            case AnimationKeys.MN2_MOVE:
                shouldMove = false;
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.MN2_SKILL_1, false);
                AudioManager.Ins.SoundEffect(7);
                shootFirst = true;
                break;
            case AnimationKeys.MN2_SKILL_1:
                shootFirst = false;
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.MN2_SKILL_2, false);
                AudioManager.Ins.SoundEffect(7);
                shootSecond = true;
                break;
            case AnimationKeys.MN2_SKILL_2:
                shootSecond = false;
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.MN2_SKILL_3_READY, false);
                break;
            case AnimationKeys.MN2_SKILL_3_READY:
                Dash();
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.MN2_SKILL_3_ATTACK, false);
                AudioManager.Ins.SoundEffect(7);
                break;
            case AnimationKeys.MN2_SKILL_3_ATTACK:
                shouldMove = true;
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.MN2_MOVE, false);
                break;
                //case AnimationKeys.MN2_DIE:
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

        if (shootFirst)
        {
            ShootFirst();
        }

        if (shootSecond)
        {
            ShootSecond();
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

    Tween OnEnableTween;

    public void Dash()
    {
        OnEnableTween?.Kill();
        if (bossController.currentHealth > 0 && this.gameObject.activeSelf && bossController.currentHealth > 0)
        {
            var pos = PlayerController.Ins.transform.position;
            OnEnableTween = transform.DOMove(new Vector3(pos.x - 1f, pos.y - 1), .867f);
        }
    }

    //public void ShotRadia()
    //{
    //DOVirtual.DelayedCall(1, () =>
    //{
    //    if (shootCounter <= 0)
    //    {
    //        shootCounter = fireRateSecond;

    //        foreach (Transform t in shotPointRadia)
    //        {
    //            Instantiate(bullet, t.position, t.rotation);
    //        }
    //    }
    //});


    //}

    //==================SETING ACTION==================//
    //public void ShootAction(int type)
    //{
    //    // type == 0 => ban 5 vien dan || type == 1 => ban 3 vien dan
    //    if (bossController.currentHealth > 0)
    //    {
    //        if (type == 0)
    //        {
    //            if (shoot != null) StopCoroutine(shoot);
    //            shoot = StartCoroutine(DelayShootFirst());
    //        }
    //        else if (type == 1)
    //        {
    //            if (shootRadia != null) StopCoroutine(shootRadia);
    //            shootRadia = StartCoroutine(DelayShootSecond());
    //        }
    //    }
    //}

    //public void DashAction()
    //{
    //    if (bossController.currentHealth > 0)
    //    {
    //        if (dash != null) StopCoroutine(dash);

    //        dash = StartCoroutine(DelayDash());
    //    }
    //}

    //IEnumerator Starting()
    //{
    //    yield return new WaitForSeconds(delayStarting);
    //    shootFirst = false;
    //    ShootAction(0);
    //}

    //Coroutine shoot;
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

    //Coroutine shootRadia;
    //IEnumerator DelayShootSecond()
    //{
    //    shootSecond = true;
    //    yield return new WaitForSeconds(delayAction);
    //    shootSecond = false;
    //    DashAction();
    //}

    //Coroutine dash;
    //IEnumerator DelayDash()
    //{
    //    DOVirtual.DelayedCall(1, () =>
    //    {
    //        var pos = PlayerController.Ins.transform.localPosition;
    //        Debug.Log(pos);
    //        transform.DOMove(pos, 1f);
    //    });
    //    yield return new WaitForSeconds(delaySequence + 1);
    //    ShootAction(0);
    //}
}
