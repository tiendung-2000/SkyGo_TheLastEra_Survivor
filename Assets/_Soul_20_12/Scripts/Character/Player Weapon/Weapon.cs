using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using DG.Tweening;

public enum WeaponType
{
    Gun,
    Staff,
    Sword,
}

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

    public int currentClip, maxClipSize = 10, currentAmmo, maxAmmoSize = 100;

    public bool isDupliGun;

    public bool isRocketGun;

    [SerializeField, Range(0, 100)]
    float m_launchIntensity;

    private void Awake()
    {
        if (Ins == null)
        {
            Ins = this;
        }
    }

    void Start()
    {
        if (type == WeaponType.Gun)
        {
            canFire = true;
            canExplode = false;
            currentClip = maxClipSize;
            CooldownUI.instance.fill.fillAmount = (reloadTimeCounter) / reloadTime;
            shotCounter = timeBetweenShots;
        }
    }

    void Update()
    {
        shotCounter -= Time.deltaTime;

        if (currentClip <= 0 && !isFullAmmo && type == WeaponType.Gun)
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
    }

    public void OnDisable()
    {
        if (type == WeaponType.Gun)
        {
            currentClip = maxClipSize;
            CooldownUI.instance.DoFade();
        }
    }

    public void OnEnable()
    {
        if (!isDupliGun && type == WeaponType.Gun)
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
        int reloadAmount = maxClipSize - currentClip; //how many bullets to refill clip
        reloadAmount = (currentAmmo - reloadAmount) >= 0 ? reloadAmount : currentAmmo;
        currentClip += reloadAmount;
        if (PlayerController.Ins.currentGun != 0)
        {
            currentAmmo -= reloadAmount;
        }

        ButtonControllerUI.Ins.bulletCircle.DOValue(maxClipSize, reloadTime)
            .OnComplete(() =>
            {
                ButtonControllerUI.Ins.SetupGunStats();
            });
    }

    public void PickupAmmo(int ammoAmount)
    {
        PlayerController.Ins.availableGuns[PlayerController.Ins.currentGun].currentAmmo += ammoAmount;
        if (PlayerController.Ins.availableGuns[PlayerController.Ins.currentGun].currentAmmo > maxAmmoSize)
        {
            PlayerController.Ins.availableGuns[PlayerController.Ins.currentGun].currentAmmo = maxAmmoSize;
        }
    }

    public void GunFire()
    {
        if (canFire == true && currentClip > 0 && isRocketGun == false && type == WeaponType.Gun)
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

    void RocketFire()
    {
        if (shotCounter < 0 && type == WeaponType.Gun)
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
        }
    }

    IEnumerator IEReload()
    {
        yield return new WaitForSeconds(reloadTime);

        Reload();
    }
}
