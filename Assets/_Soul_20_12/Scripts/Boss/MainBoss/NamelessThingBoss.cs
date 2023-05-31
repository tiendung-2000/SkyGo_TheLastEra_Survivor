using DG.Tweening;
using Spine;
using System.Collections;
using UnityEngine;

public class NamelessThingBoss : MonoBehaviour
{
    public static NamelessThingBoss Ins;

    BossController bossController;

    //public SkeletonAnimation ske;

    [Header("Shooting")]
    public float xAngle;
    public float yAngle;

    public int dashCount;

    public Transform shootGr;

    public Transform[] pointToMove;

    public Transform[] shotPointsFirst;
    public Transform[] shotPointsThird;
    public Transform[] shotPointsFour;

    public ParticleSystem vfx;

    public Collider2D col;

    public GameObject bulletFirst;
    public GameObject bulletThird;
    public GameObject bulletFour;

    public float fireRateFirst;
    public float fireRateThird;
    public float fireRateFour;

    public float shootCounter;

    public bool phaseFirst = false;
    public bool phaseThird = false;
    public bool phaseFour = false;

    Tween OnEnableTween;

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
        bossController.ske.AnimationState.SetAnimation(0, "Idle", false);
    }
    private void AnimationState_Complete(TrackEntry trackEntry)
    {
        switch (trackEntry.Animation.Name)
        {
            case AnimationKeys.B3_IDLE:
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.B3_SKILL_1_ATTACK, false);
                AudioManager.Ins.SoundEffect(7);
                phaseFirst = true;
                break;

            case AnimationKeys.B3_SKILL_1_ATTACK:
                phaseFirst = false;
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.B3_SKILL_2_READY, false);
                break;

            case AnimationKeys.B3_SKILL_2_READY:
                Dash();
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.B3_SKILL_2_MOVE, false);
                break;

            case AnimationKeys.B3_SKILL_2_MOVE:
                if (dashCount == 3)
                {
                    bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.B3_SKILL_3_ATTACK, false);
                AudioManager.Ins.SoundEffect(7);
                    phaseThird = true;
                }
                else
                {
                    bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.B3_SKILL_2_READY, false);
                }
                break;

            case AnimationKeys.B3_SKILL_3_ATTACK:
                dashCount = 0;
                phaseThird = false;
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.B3_SKILL_4_ATTACK, false);
                AudioManager.Ins.SoundEffect(7);
                phaseFour = true;
                break;

            case AnimationKeys.B3_SKILL_4_ATTACK:
                phaseFour = false;
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.B3_SKILL_5_READY, false);
                col.enabled = false;

                break;

            case AnimationKeys.B3_SKILL_5_READY:
                OnEnableTween?.Kill();
                GetComponent<MeshRenderer>().sortingLayerName = "Default";
                col.enabled = true;
                OnEnableTween = DOVirtual.DelayedCall(2f, () =>
                {
                    bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.B3_SKILL_5_MOVE, false);
                    GetComponent<MeshRenderer>().sortingLayerName = "Enemies";
                });
                break;

            case AnimationKeys.B3_SKILL_5_MOVE:
                OnEnableTween?.Kill();
                bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.B3_IDLE, true);

                vfx.Play();

                OnEnableTween = DOVirtual.DelayedCall(.5f, () =>
                {
                    vfx.Play();


                    DOVirtual.DelayedCall(.5f, () =>
                    {
                        vfx.Play();

                            bossController.ske.AnimationState.SetAnimation(0, AnimationKeys.B3_IDLE, false);
                    });
                });
                break;

            //case AnimationKeys.B3_DIE:
            //    Destroy(gameObject);
            //    break;
        }
    }

    private void Update()
    {
        shootCounter -= Time.deltaTime;

        if (phaseFirst == true)
        {
            PhaseFirst();
        }

        if (phaseThird == true)
        {
            PhaseThird();
        }

        if (phaseFour == true)
        {
            PhaseFour();
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
    public void PhaseThird()
    {
        if (shootCounter <= 0 && this.gameObject.activeSelf && bossController.currentHealth > 0)
        {
            shootCounter = fireRateThird;

            foreach (Transform t in shotPointsThird)
            {
                SmartPool.Ins.Spawn(bulletThird, t.position, t.rotation);
            }
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
    public void Dash()
    {
        //var pos = PlayerController.Ins.transform.localPosition;
        //transform.DOMove(pos, 0f);

        //test
        if (transform.parent.GetComponent<RoomCenter>().checkPoint != null && bossController.currentHealth > 0)
        {
            transform.parent.GetComponent<RoomCenter>().GetTarget();
            transform.localPosition = transform.parent.GetComponent<RoomCenter>().checkPoint.localPosition;
            dashCount++;
        }
    }

    //public void Explode()
    //{
    //    vfx.Play();
    //    DOVirtual.DelayedCall(.5f, () =>
    //    {
    //        vfx.Play();

    //        DOVirtual.DelayedCall(.5f, () =>
    //        {
    //            vfx.Play();
    //        });
    //    });

    //}

    ////==================SETING ACTION==================//

    //public void ShootAction(int type)
    //{
    //    // type == 0 => ban 5 vien dan || type == 1 => ban 3 vien dan
    //    if (bossController.currentHealth > 0)
    //    {
    //        if (type == 1)
    //        {
    //            if (shoot1 != null) StopCoroutine(shoot1);
    //            shoot1 = StartCoroutine(DelayShootFirst());
    //        }
    //        else if (type == 2)
    //        {
    //            if (dash != null) StopCoroutine(dash);
    //            dash = StartCoroutine(DelayDash());
    //        }
    //        else if (type == 3)
    //        {
    //            if (shoot3 != null) StopCoroutine(shoot3);
    //            shoot3 = StartCoroutine(DelayShootThird());
    //        }
    //        else if (type == 4)
    //        {
    //            if (shoot4 != null) StopCoroutine(shoot4);
    //            shoot4 = StartCoroutine(DelayShootFour());
    //        }
    //        else if (type == 5)
    //        {
    //            if (tele != null) StopCoroutine(tele);
    //            tele = StartCoroutine(DelayTele());
    //        }
    //    }
    //}

    //IEnumerator Starting()
    //{
    //    yield return new WaitForSeconds(delayStarting);
    //    phaseFirst = false;
    //    ShootAction(1);
    //}

    //Coroutine shoot1;
    //IEnumerator DelayShootFirst()
    //{
    //    phaseFirst = true;
    //    yield return new WaitForSeconds(2f);
    //    phaseFirst = false;
    //    DOVirtual.DelayedCall(1, () =>
    //    {
    //        ShootAction(2);
    //    });
    //}

    //Coroutine dash;
    //IEnumerator DelayDash()
    //{
    //    DOVirtual.DelayedCall(.5f, () =>
    //    {
    //        Dash();
    //        DOVirtual.DelayedCall(.5f, () =>
    //        {
    //            Dash();
    //            DOVirtual.DelayedCall(.5f, () =>
    //            {
    //                Dash();
    //            });
    //        });
    //    });

    //    //for (int i = 0; i <= dashLoop; i++)
    //    //{
    //    //    Debug.Log("vao day");
    //    //    DOVirtual.DelayedCall(.5f, () =>
    //    //        {
    //    //            Dash();
    //    //        });
    //    //}
    //    yield return new WaitForSeconds(delayAction);
    //    DOVirtual.DelayedCall(2, () =>
    //    {
    //        ShootAction(3);
    //    });
    //}

    //Coroutine shoot3;
    //IEnumerator DelayShootThird()
    //{
    //    phaseThird = true;
    //    yield return new WaitForSeconds(delayAction);
    //    phaseThird = false;
    //    DOVirtual.DelayedCall(1, () =>
    //    {
    //        ShootAction(4);
    //    });
    //}

    //Coroutine shoot4;
    //IEnumerator DelayShootFour()
    //{
    //    phaseFour = true;
    //    yield return new WaitForSeconds(3f);
    //    phaseFour = false;
    //    DOVirtual.DelayedCall(1, () =>
    //    {
    //        ShootAction(5);
    //    });
    //}

    //Coroutine tele;
    //IEnumerator DelayTele()
    //{
    //    Teleport();
    //    yield return new WaitForSeconds(3f);
    //    DOVirtual.DelayedCall(2, () =>
    //    {
    //        ShootAction(1);
    //    });

    //}
}
