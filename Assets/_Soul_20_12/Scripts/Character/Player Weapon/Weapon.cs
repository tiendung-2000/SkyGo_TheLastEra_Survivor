using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using DG.Tweening;

#region WeaponType
public enum WeaponType
{
    Pistol,
    Machine,
    Shotgun,
    Akm,
    Galting,
    Bazoka,
    Rocket,
    Shuriken,
    Halo,
    SuperShotgun,
    Heaven,
    Crossbow,
    ThunderBow,
    FireStaff,
    ToxicStaff,
    ArcadeSword,
}
#endregion

public class Weapon : MonoBehaviour
{
    public static Weapon Ins;
    public WeaponType type;

    public SkeletonAnimation ske;

    public GameObject bulletToFire;

    public bool canFire;
    public bool canExplode = false;
    [SerializeField] GameObject explodeBullet;

    public List<Transform> firePoint;

    public int xAngle, yAngle;

    public float reloadTime;
    public float reloadTimeCounter;

    public float reload;

    public bool isFullAmmo;

    public float timeBetweenShots;
    public float shotCounter;

    public string weaponName;

    public int itemCost;
    public Sprite gunSprite;
    public Sprite gunSwitchSprite;

    public int currentClip, maxClipSize = 10/*, currentAmmo, maxAmmoSize = 100*/;

    public bool isDupliGun;

    [Header("Rocket")]
    public bool isRocketGun;
    [SerializeField, Range(0, 100)]
    float m_launchIntensity;

    [Header("Sword")]
    [SerializeField] Vector2 weaponOffset;  // Recommended: (1, 0, 0)
    [SerializeField] float weaponRot;                                  // Recommended: 135
    [SerializeField] float swingSpeed;                                          // Recommended: 10
    int swing = 1;
    GameObject anchor;
    Vector3 target;
    float swingAngle;
    bool swinging;

    private void Awake()
    {
        if (Ins == null)
        {
            Ins = this;
        }
    }

    void Start()
    {
        canFire = true;
        canExplode = false;
        currentClip = maxClipSize;
        CooldownUI.instance.fill.fillAmount = (reloadTimeCounter) / reloadTime;
        shotCounter = timeBetweenShots;

        anchor = transform.parent.gameObject;

    }

    void Update()
    {
        shotCounter -= Time.deltaTime;

        if (currentClip <= 0 && !isFullAmmo)
        {
            CooldownUI.instance.DoShow();
            reloadTimeCounter += Time.deltaTime;

            CooldownUI.instance.fill.fillAmount = (reloadTimeCounter) / reloadTime;

            if (reloadTimeCounter >= reloadTime)
            {
                reloadTimeCounter = 0;
                CooldownUI.instance.fill.fillAmount = (reloadTimeCounter) / reloadTime;
                Debug.Log(CooldownUI.instance.canvasGR);

                CooldownUI.instance.DoFade();
                isFullAmmo = true;
            }
        }


        if (type == WeaponType.ArcadeSword)
        {
            //Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            Vector3 rot = anchor.transform.eulerAngles;
            swingAngle = Mathf.Lerp(swingAngle, swing * 90, Time.deltaTime * swingSpeed);
            //rot.z = angle + swingAngle;
            anchor.transform.eulerAngles = rot;

            // Weapon rotation
            float t = swing == 1 ? 0 : 180;
            target.z = Mathf.Lerp(target.z, t, Time.deltaTime * swingSpeed);
            if (Mathf.Abs(t - target.z) < 5) swinging = false;
            transform.localRotation = Quaternion.Euler(target);
        }
    }

    public void OnDisable()
    {
        currentClip = maxClipSize;
        CooldownUI.instance.DoFade();
    }

    public void OnEnable()
    {
        //ske.AnimationState.SetAnimation(0, "neutral", true);

        if (type == WeaponType.ArcadeSword)
        {
            firePoint[0] = PlayerController.Ins.swordPoint;
        }

        if (!isDupliGun)
        {
            CooldownUI.instance.DoFade();
            currentClip = maxClipSize;
            reloadTimeCounter = 0;
            CooldownUI.instance.fill.fillAmount = (reloadTimeCounter) / reloadTime;
            shotCounter = timeBetweenShots;
        }
    }

    public void Reload()
    {
        //int reloadAmount = maxClipSize - currentClip;
        //reloadAmount = (currentAmmo - reloadAmount) >= 0 ? reloadAmount : currentAmmo;
        //if (PlayerController.Ins.currentGun != 0)
        //{
        //    currentAmmo -= reloadAmount;
        //}

        currentClip += maxClipSize;
        ButtonControllerUI.Ins.bulletCircle.DOValue(maxClipSize, reloadTime)
            .OnComplete(() =>
            {
                ButtonControllerUI.Ins.SetupGunStats();
            });
    }

    //public void PickupAmmo(int ammoAmount)
    //{
    //    //PlayerController.Ins.availableGuns[PlayerController.Ins.currentGun].currentAmmo += ammoAmount;
    //    //if (PlayerController.Ins.availableGuns[PlayerController.Ins.currentGun].currentAmmo > maxAmmoSize)
    //    //{
    //    //    PlayerController.Ins.availableGuns[PlayerController.Ins.currentGun].currentAmmo = maxAmmoSize;
    //    //}
    //}

    public void GunFire()
    {
        if (type != WeaponType.ArcadeSword)
        {
            if (canFire == true && currentClip > 0 && isRocketGun == false)
            {
                if (shotCounter < 0)
                {
                    ske.AnimationState.SetAnimation(0, "fire", false);

                    if (canExplode)
                    {
                        for (int i = 0; i < firePoint.Count; i++)
                        {
                            var newBullet = SmartPool.Ins.Spawn(explodeBullet, firePoint[i].position, firePoint[i].rotation);
                            newBullet.transform.Rotate(0f, 0f, Random.Range(-xAngle, yAngle));
                            shotCounter = timeBetweenShots;
                            currentClip--;

                            ButtonControllerUI.Ins.bulletText.text = currentClip.ToString();
                            ButtonControllerUI.Ins.bulletCircle.value--;
                        }

                        //GunSound();
                    }
                    else
                    {
                        for (int i = 0; i < firePoint.Count; i++)
                        {
                            var newBullet = SmartPool.Ins.Spawn(bulletToFire, firePoint[i].position, firePoint[i].rotation);
                            newBullet.transform.Rotate(0f, 0f, Random.Range(-xAngle, yAngle));
                            shotCounter = timeBetweenShots;
                            currentClip--;

                            ButtonControllerUI.Ins.bulletText.text = currentClip.ToString();
                            ButtonControllerUI.Ins.bulletCircle.value--;
                        }

                        //GunSound();
                    }
                    if (currentClip <= 0)
                    {
                        isFullAmmo = false;
                        StartCoroutine(IEReload());
                    }
                }
            }
            else if (isRocketGun == true)
            {
                RocketFire();
            }
        }
        else
        {
            if (canFire == true && currentClip > 0 && isRocketGun == false)
            {
                if (shotCounter < 0)
                {
                    if (swinging) return;
                    // Attack
                    var newBullet = SmartPool.Ins.Spawn(bulletToFire, firePoint[0].position, firePoint[0].rotation);
                    newBullet.transform.Rotate(0f, 0f, Random.Range(-xAngle, yAngle));

                    swing *= -1;
                    swinging = true;

                    shotCounter = timeBetweenShots;
                    currentClip--;

                    ButtonControllerUI.Ins.bulletText.text = currentClip.ToString();
                    ButtonControllerUI.Ins.bulletCircle.value--;
                    //GunSound();

                    if (currentClip <= 0)
                    {
                        isFullAmmo = false;
                        StartCoroutine(IEReload());
                    }
                }
            }
        }
    }

    void RocketFire()
    {
        if (shotCounter < 0)
        {
            for (int i = 0; i < firePoint.Count; i++)
            {
                GameObject newProjectile = SmartPool.Ins.Spawn(bulletToFire, firePoint[i].position, firePoint[i].rotation) as GameObject;
                shotCounter = timeBetweenShots;
                currentClip--;

                if (newProjectile.GetComponent<Rigidbody2D>())
                    newProjectile.GetComponent<Rigidbody2D>().AddForce(transform.right * m_launchIntensity, ForceMode2D.Impulse);

                ButtonControllerUI.Ins.bulletText.text = currentClip.ToString();
                ButtonControllerUI.Ins.bulletCircle.value--;
            }
            //GunSound();
        }
    }

    IEnumerator IEReload()
    {
        yield return new WaitForSeconds(reloadTime);
        AudioManager.Ins.PlayGunDrawSound();
        Reload();
    }

    public void GunSound()
    {
        switch (type)
        {
            case WeaponType.Pistol:
                AudioManager.Ins.PlayGunSound(0);
                break;
            case WeaponType.Machine:
                AudioManager.Ins.PlayGunSound(1);
                break;
            case WeaponType.Shotgun:
                AudioManager.Ins.PlayGunSound(2);
                break;
            case WeaponType.Akm:
                AudioManager.Ins.PlayGunSound(3);
                break;
            case WeaponType.Galting:
                AudioManager.Ins.PlayGunSound(4);
                break;
            case WeaponType.Bazoka:
                AudioManager.Ins.PlayGunSound(5);
                break;
            case WeaponType.Rocket:
                AudioManager.Ins.PlayGunSound(6);
                break;
            case WeaponType.Shuriken:
                AudioManager.Ins.PlayGunSound(7);
                break;
            case WeaponType.Halo:
                AudioManager.Ins.PlayGunSound(8);
                break;
            case WeaponType.SuperShotgun:
                AudioManager.Ins.PlayGunSound(2);
                break;
            case WeaponType.Heaven:
                AudioManager.Ins.PlayGunSound(8);
                break;
            case WeaponType.Crossbow:
                AudioManager.Ins.PlayGunSound(9);
                break;
            case WeaponType.ThunderBow:
                AudioManager.Ins.PlayGunSound(9);
                break;
            case WeaponType.FireStaff:
                AudioManager.Ins.PlayGunSound(10);
                break;
            case WeaponType.ToxicStaff:
                AudioManager.Ins.PlayGunSound(10);
                break;
            case WeaponType.ArcadeSword:
                AudioManager.Ins.PlayGunSound(11);
                break;
        }
    }
}
