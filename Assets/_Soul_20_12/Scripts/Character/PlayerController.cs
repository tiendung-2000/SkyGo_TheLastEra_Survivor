using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Ins;

    public int currentHealth;
    public int curPlayerMaxHP;
    public int playerBaseDamage;
    public float moveSpeed;
    public bool canMove = true;
    public bool isMove = true;

    public Material material;
    public Vector2 moveInput;
    public Collider2D col;
    public Rigidbody2D theRB;
    public Transform theHand;
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset idle, move, skill, die;
    public string currentState;
    public string currentAnimation;

    public Transform swordPoint;
    public Sword swordWeapon;
    public float detectRange;
    public LayerMask whatIsEnemy;

    private Weapon gun;
    public List<Weapon> availableGuns = new List<Weapon>();
    public List<Weapon> availableDupliGuns = new List<Weapon>();
    [HideInInspector]
    public int currentGun;

    public ParticleSystem shieldBuffFX, shiedBreakFX, healthBuffFX, coinCollectFX;

    private void Awake()
    {
        Ins = this;
    }

    void Start()
    {
        SetCharacterState(currentState);

        OnHPUpgrade(DynamicDataManager.Ins.CurPlayerHPUpgrade);
        OnSpeedUpgrade(DynamicDataManager.Ins.CurPlayerSpeedUpgrade);
    }

    private void OnEnable()
    {

        material.SetFloat("_FillPhase", 0f);

        canMove = true;
        isMove = true;

        currentState = "Idle";
        if (col.enabled == false)
        {
            col.enabled = true;
        }

        ResetWeapon();
    }

    private void ResetWeapon()
    {
        availableGuns[0].gameObject.SetActive(true);
        currentGun = 0;
    }

    void Update()
    {
        if (currentHealth > 0 && isMove)
        {
            PlayerMove();
        }
        else
        {
            theRB.velocity = Vector2.zero;
        }

        DetectEnemy();
    }

    public void PlayerMove()
    {
        theRB.velocity = moveInput * PlayerSkillManager.instance.activeMoveSpeed;

        if (canMove || !canMove)
        {
            #region Mobile
            Vector2 dir = new Vector2(UltimateJoystick.GetHorizontalAxis("Player Movement JoyStick"), UltimateJoystick.GetVerticalAxis("Player Movement JoyStick"));

            moveInput.x = dir.x;
            moveInput.y = dir.y;

            if (dir.magnitude > 0f)
            {
                Vector3 movementDir = new Vector3(moveInput.x, moveInput.y, 0f);
                transform.position += movementDir.normalized * moveSpeed * Time.deltaTime;
                moveInput.Normalize();
            }
            #endregion

            #region Desktop
            //moveInput.x = Input.GetAxisRaw("Horizontal");
            //moveInput.y = Input.GetAxisRaw("Vertical");

            //moveInput.Normalize();


            //theRB.velocity = moveInput * PlayerSkillManager.instance.activeMoveSpeed;


            #endregion

        }
        else
        {
            theRB.velocity = Vector2.zero;
            SetCharacterState("Idle");
        }

        if (moveInput != Vector2.zero && canMove && currentHealth > 0)
        {
            SetCharacterState("Move");
        }
        else
        {
            if (canMove && currentHealth > 0)
            {
                SetCharacterState("Idle");
            }
        }
    }

    #region Desktop

    //private void FixedUpdate()
    //{
    //    FlipPlayer();
    //}

    //void FlipPlayer()
    //{
    //    Vector3 mousePos = Input.mousePosition;
    //    Vector3 screenPoint = CameraController.Ins.mainCamera.WorldToScreenPoint(transform.localPosition);

    //    if (mousePos.x < screenPoint.x)
    //    {
    //        transform.localScale = new Vector3(-1f, 1f, 1f);
    //        theHand.localScale = new Vector3(-1f, -1f, 1f);
    //    }
    //    else
    //    {
    //        transform.localScale = Vector3.one;
    //        theHand.localScale = Vector3.one;
    //    }

    //    //rotate gun arm
    //    Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    //    float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
    //    theHand.rotation = Quaternion.Euler(0, 0, (angle));
    //}

    #endregion

    public void PlayerStopMove()
    {
        theRB.velocity = Vector3.zero;
        SetCharacterState("Idle");
        ButtonControllerUI.Ins.OnDisableJoystick();
    }

    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timescale)
    {
        if (animation.name.Equals(currentAnimation))
        {
            return;
        }
        skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timescale;
        currentAnimation = animation.name;
    }

    public void SetCharacterState(string state)
    {
        if (state.Equals("Idle"))
        {
            currentState = "Idle";
            SetAnimation(idle, true, 1f);
        }
        else if (state.Equals("Move"))
        {
            currentState = "Move";
            SetAnimation(move, true, 1f);
        }
        else if (state.Equals("Skill"))
        {
            currentState = "Skill";
            SetAnimation(skill, true, 1f);
        }
        else if (state.Equals("Die"))
        {
            currentState = "Die";
            SetAnimation(die, false, 1f);
        }
    }

    public void ResetPlayer()
    {
        for (int i = theHand.childCount - 1; i >= 3; i--)
        {
            Destroy(theHand.GetChild(i).gameObject);
        }

        for (int i = availableGuns.Count - 1; i > 0; i--)
        {
            availableGuns.RemoveAt(i);
        }

        playerBaseDamage = 0;
    }

    public void SwitchGun()
    {
        foreach (Weapon theGun in availableGuns)
        {
            theGun.gameObject.SetActive(false);
        }
        foreach (Weapon theGun in availableDupliGuns)
        {
            theGun.gameObject.SetActive(false);
        }

        availableGuns[currentGun].gameObject.SetActive(true);
        ButtonControllerUI.Ins.SetupGunStats();
    }

    public void DetectEnemy()
    {
        Collider2D enemy = Physics2D.OverlapCircle(transform.position, detectRange, whatIsEnemy);

        if (enemy != null)
        {
            //Debug.Log("1");
            availableGuns[currentGun].canFire = false;
        }
        else
        {
            //Debug.Log("2");

            availableGuns[currentGun].canFire = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }

    private void OnHPUpgrade(int level)
    {
        int numHP = ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.HP[level];
        PlayerHub.Ins.OnHealthChange(numHP);
        currentHealth = numHP;
        curPlayerMaxHP = numHP;
    }

    private void OnSpeedUpgrade(int level)
    {
        int numSpeed = ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.Speed[level];
        moveSpeed = numSpeed;
    }

    public void TakeDamageEffect()
    {
        StartCoroutine(IETakeDamageEffect());
    }

    IEnumerator IETakeDamageEffect()
    {
        material.SetFloat("_FillPhase", 1f);
        yield return new WaitForSeconds(0.2f);
        material.SetFloat("_FillPhase", 0f);
    }

}
