using Spine.Unity;
using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController Ins;

    private EnemyMovement enemyMovement;
    private EnemyAttack enemyAttack;

    public SkeletonAnimation Ske;
    [SerializeField] float animationDuration;
    //public Renderer materialRender;

    [SerializeField] Collider2D col;

    [Header("Variables")]
    //public Material material;
    public Rigidbody2D theRB;
    public float moveSpeed;

    public int health = 150;

    //public GameObject[] deathSplatters;
    public GameObject hitEffect;

    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;

    public bool playerOnZone = false;
    public bool enemyCanMove;
    //public bool enemyCanSpawn;

    private void Awake()
    {
        Ins = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerOnZone = false;

        enemyMovement = GetComponent<EnemyMovement>();
        enemyAttack = GetComponent<EnemyAttack>();

        health = LevelManager.Ins.CaculateHealth(health);

        Ske.AnimationState.Complete += AnimationState_Complete;

        if (enemyMovement.isMoving == true)
        {
            Ske.AnimationState.SetAnimation(0, Constant.ANIM_MOVE, true);
        }

        if (enemyMovement.isWander)
        {
            enemyMovement.pauseWanderCounter = Random.Range(enemyMovement.pauseLength * .75f, enemyMovement.pauseLength * 1.25f);
        }
    }

    private void AnimationState_Complete(Spine.TrackEntry trackEntry)
    {
        switch (trackEntry.Animation.Name)
        {
            case Constant.ANIM_ATTACK:
                if (enemyCanMove == true)
                {
                    enemyMovement.isMoving = true;
                    if (enemyMovement.isMoving == true)
                    {
                        Ske.AnimationState.SetAnimation(0, Constant.ANIM_MOVE, true);
                    }
                }
                else
                {
                    if (enemyMovement.isMoving == false)
                    {
                        Ske.AnimationState.SetAnimation(0, Constant.ANIM_IDLE, true);
                    }
                }
                break;
        }
    }

    void Update()
    {
        if (playerOnZone == true && health > 0 && PlayerController.Ins.currentHealth > 0)
        {
            enemyMovement.EnemyMoving();

            enemyMovement.EnemyWander();

            enemyAttack.EnemyShooting();

            enemyAttack.EnemyAimingSystem();
        }
        else
        {
            theRB.velocity = Vector2.zero;
        }
    }

    private void OnEnable()
    {
        //this.materialRender.material.SetColor("_Color", Color.black);

        StartCoroutine(IEActive());
    }

    IEnumerator IEActive()
    {
        yield return new WaitForSeconds(1f);
        playerOnZone = true;
    }

    public void DamageEnemy(int damage)
    {
        #region Desktop
        health -= Mathf.RoundToInt(damage / 2f);
        #endregion
        StartCoroutine(IETakeDamageEffect());
        //AudioManager.instance.PlaySFX(2);

        SmartPool.Ins.Spawn(hitEffect, transform.position, transform.rotation);

        if (health <= 0)
        {
            col.enabled = false;
            AudioManager.Ins.SoundEffect(10);
            Ske.AnimationState.SetAnimation(0, Constant.ANIM_DIE, false);


            //int rotation = Random.Range(0, 4);

            if (shouldDropItem)
            {
                float dropChance = Random.Range(0f, 100f);

                if (dropChance < itemDropPercent)
                {
                    int randomItem = Random.Range(0, itemsToDrop.Length);

                    SmartPool.Ins.Spawn(itemsToDrop[randomItem], transform.position, transform.rotation);
                }
            }
            StartCoroutine(IEDestroy());
        }
    }
    IEnumerator IETakeDamageEffect()
    {
        //this.material.SetFloat("_FillPhase", 1f);
        yield return new WaitForSeconds(0.2f);
        //this.material.SetFloat("_FillPhase", 0f);
    }

    IEnumerator IEDestroy()
    {
        yield return new WaitForSeconds(animationDuration);
        Destroy(gameObject);
    }
}

public class Constant
{
    public const string ANIM_IDLE = "Idel";
    public const string ANIM_MOVE = "Move";
    public const string ANIM_ATTACK = "Attack";
    public const string ANIM_DIE = "Die";
}
