using API.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControllerUI : BaseUIMenu
{
    public static ButtonControllerUI Ins;

    public bool pointerDown;

    [SerializeField] Button shootButton, buyButton, skillButton, switchButton;

    public bool buy = false;
    public UltimateJoystick joystick;
    public Image gunImage;
    [Header("SkillButton")]
    public Image skillCDImage;

    Tween OnEnableTween;

    private void Awake()
    {
        Ins = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        shootButton.onClick.AddListener(OnShoot);
        //buyButton.onClick.AddListener(OnBuy);
        skillButton.onClick.AddListener(OnSkill);
        switchButton.onClick.AddListener(OnSwitch);
    }

    public void OnDisableJoystick()
    {
        joystick.DisableJoystick();
    }

    public void OnEnableJoyStick()
    {
        joystick.EnableJoystick();
    }

    private void OnEnable()
    {
        Time.timeScale = 1f;

        //gunImage.sprite = PlayerController.Ins.availableGuns[PlayerController.Ins.currentGun].gunSwitchSprite;
    }


//#if UNITY_EDITOR
//    private void FixedUpdate()
//    {
//        if (Input.GetKeyUp(KeyCode.Space))
//        {
//            OnSkill();
//        }
//    }

//#endif

    public void OnPointerDown()
    {
        pointerDown = true;
    }

    public void OnPointerUp()
    {
        pointerDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (pointerDown)
        {
            OnShoot();
        }
    }

    public void OnSwitch()
    {
        if (PlayerController.Ins.availableGuns.Count > 0)
        {
            PlayerController.Ins.currentGun++;
            if (PlayerController.Ins.currentGun >= PlayerController.Ins.availableGuns.Count)
            {
                PlayerController.Ins.currentGun = 0;
            }

            PlayerController.Ins.SwitchGun();
            gunImage.sprite = PlayerController.Ins.availableGuns[PlayerController.Ins.currentGun].gunSwitchSprite;
        }
        else
        {
            Debug.LogError("Player has no guns!");
        }
    }

    public void OnShoot()
    {
        if(PlayerController.Ins.availableGuns[PlayerController.Ins.currentGun].canFire == true)
        {
        PlayerController.Ins.availableGuns[PlayerController.Ins.currentGun].GunFire();
        if (PlayerSkillManager.instance.dualChecker == true)
            PlayerController.Ins.availableDupliGuns[PlayerController.Ins.currentGun].GunFire();

        }
        else
        {
            PlayerController.Ins.Sword();
        }
    }

    public void OnSkill()
    {
        switch (PlayerSkillManager.instance.playerID)
        {
            case 0:
                PlayerSkillManager.instance.Dash();
                break;
            case 1:
                PlayerSkillManager.instance.DualGun();
                break;
            case 2:
                PlayerSkillManager.instance.HolyShield();
                break;
            case 3:
                PlayerSkillManager.instance.BulletGrenade();
                break;
            case 4:
                PlayerSkillManager.instance.Speed();
                break;
            default:
                print("NIg");
                break;
        }
    }

    public void CoolDown()
    {
        skillCDImage.fillAmount = 0f;

        OnEnableTween?.Kill();
        OnEnableTween = skillCDImage.DOFillAmount(1f, PlayerSkillManager.instance.coolCounter);
    }
    //public void OnBuy()
    //{
    //    Debug.Log("isBuy");
    //    if (RoomCenter.Ins.typeItem == 1)
    //    {
    //        buy = true;
    //    }
    //    else if (RoomCenter.Ins.typeItem == 2)
    //    {
    //        buy = true;
    //    }
    //    else if (RoomCenter.Ins.typeItem == 3)
    //    {
    //        buy = true;
    //    }
    //}
}
