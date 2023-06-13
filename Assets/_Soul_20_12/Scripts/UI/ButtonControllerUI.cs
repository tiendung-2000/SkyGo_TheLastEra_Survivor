using API.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControllerUI : BaseUIMenu
{
    public static ButtonControllerUI Ins;

    public bool pointerDown;
    public bool buy = false;
    public UltimateJoystick joystick;

    [SerializeField] Button shootButton, buyButton, skillButton, switchButton;

    [Header("ShotButton")]
    public Text bulletText;
    public Image gunImage;
    public Slider bulletCircle;

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
    #region Mobile
    public void OnDisableJoystick()
    {
        //joystick.DisableJoystick();
    }

    public void OnEnableJoyStick()
    {
        //joystick.EnableJoystick();
    }
    #endregion

    private void OnEnable()
    {
        Time.timeScale = 1f;

        StartCoroutine(IESetupGunStatsUI());

    }

    IEnumerator IESetupGunStatsUI()
    {
        yield return new WaitForSeconds(1f);

        SetupGunStats();

    }

    public void SetupGunStats()
    {
        var currentGun = PlayerController.Ins.availableGuns[PlayerController.Ins.currentGun];

        bulletText.text = currentGun.currentClip.ToString();
        gunImage.sprite = currentGun.gunSwitchSprite;

        //float bulletPercent = currentGun.currentClip / currentGun.maxClipSize;

        bulletCircle.maxValue = currentGun.maxClipSize;
        bulletCircle.value = currentGun.currentClip;
    }

    #region Desktop
    private void FixedUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            OnSkill();
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            OnSwitch();
        }
    }
    #endregion



    public void OnPointerDown()
    {
        pointerDown = true;
        shootButton.transform.DOScale(.8f, .1f);
    }

    public void OnPointerUp()
    {
        shootButton.transform.DOScale(1f, .1f);
        pointerDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (pointerDown)
        {
            OnShoot();
        }
        #region Desktop
        if (Input.GetKey(KeyCode.Mouse0))
        {
            pointerDown = true;
        }
        else
        {
            pointerDown = false;
        }
        #endregion
    }

    public void OnSwitch()
    {
        AudioManager.Ins.SoundUIPlay(6);

        if (PlayerController.Ins.availableGuns.Count > 0)
        {
            PlayerController.Ins.currentGun++;
            if (PlayerController.Ins.currentGun >= PlayerController.Ins.availableGuns.Count)
            {
                PlayerController.Ins.currentGun = 0;
            }

            PlayerController.Ins.SwitchGun();
        }
        else
        {
            Debug.LogError("Player has no guns!");
        }
    }

    public void OnShoot()
    {
        if (PlayerController.Ins.availableGuns[PlayerController.Ins.currentGun].canFire == true)
        {
            PlayerController.Ins.availableGuns[PlayerController.Ins.currentGun].GunFire();
            if (PlayerSkillManager.instance.dualChecker == true)
                PlayerController.Ins.availableDupliGuns[PlayerController.Ins.currentGun].GunFire();

        }
        else
        {
            PlayerController.Ins.swordWeapon.SwordAttack();
        }
    }

    public void OnSkill()
    {
        switch (PlayerSkillManager.instance.playerID)
        {
            case 0:
                PlayerSkillManager.instance.Dash();
                AudioManager.Ins.PlaySkillSound(1);
                break;
            case 1:
                PlayerSkillManager.instance.DualGun();
                AudioManager.Ins.PlaySkillSound(0);
                break;
            case 2:
                PlayerSkillManager.instance.HolyShield();
                AudioManager.Ins.PlaySkillSound(2);
                break;
            case 3:
                PlayerSkillManager.instance.BulletGrenade();
                AudioManager.Ins.PlaySkillSound(0);
                break;
            case 4:
                PlayerSkillManager.instance.Speed();
                AudioManager.Ins.PlaySkillSound(1);
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
}
