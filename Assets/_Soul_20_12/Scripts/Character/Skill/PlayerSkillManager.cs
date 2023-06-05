using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public static PlayerSkillManager instance;

    public PlayerController player;
    public int playerID;

    public List<Skill> skills;
    public List<float> currentDuration;
    public List<float> currentCoolDown;

    public float activeMoveSpeed;

    [Header("DashSkill")]
    public float dashSpeed = 0f;
    public float dashDuration = .5f;
    public float dashCooldown = 5f;
    //public float dashInvincibility = .5f;

    [Header("DualSkill")]
    public bool dualChecker = false;
    public float dualDuration = .5f;
    public float dualCooldown = 5f;

    [Header("HolyShieldSkill")]
    public float holyShieldDuration = .5f;
    public float holyShieldCooldown = 5f;

    [Header("BulletNadeSkill")]
    public float bulletNadeDuration = .5f;
    public float bulletNadeCooldown = 5f;

    [Header("SpeedSkill")]
    public float speedDuration = .5f;
    public float speedCooldown = 5f;

    [HideInInspector]
    public float timeCounter;

    public float coolCounter;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        activeMoveSpeed = ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.Speed[DynamicDataManager.Ins.CurPlayerSpeedUpgrade];

        SetDurationCoolDown();
    }

    public void SetDurationCoolDown()
    {
        currentDuration[0] = dashDuration;
        currentCoolDown[0] = dashCooldown;

        currentDuration[1] = dualDuration;
        currentCoolDown[1] = dualCooldown;

        currentDuration[2] = holyShieldDuration;
        currentCoolDown[2] = holyShieldCooldown;

        currentDuration[3] = bulletNadeDuration;
        currentCoolDown[3] = bulletNadeCooldown;

        currentDuration[4] = speedDuration;
        currentCoolDown[4] = speedCooldown;
    }



    private void Update()
    {
        player = CharacterSelectManager.Ins.activePlayer;
        playerID = DynamicDataManager.Ins.CurPlayer;

        if (timeCounter > 0 && PlayerController.Ins.currentState.Equals("Skill"))
        {
            timeCounter -= Time.deltaTime;
            if (timeCounter <= 0)
            {
                PlayerController.Ins.SetCharacterState("Idle");
                switch (playerID)
                {
                    case 0:
                        player.canMove = true;
                        ButtonControllerUI.Ins.CoolDown();
                        break;
                    case 1:
                        player.canMove = true;
                        player.availableDupliGuns[player.currentGun].gameObject.SetActive(false);
                        ButtonControllerUI.Ins.CoolDown();

                        break;
                    case 2:
                        player.canMove = true;
                        player.col.enabled = true;
                        ButtonControllerUI.Ins.CoolDown();

                        break;
                    case 3:
                        Weapon.Ins.canExplode = false;
                        player.canMove = true;
                        ButtonControllerUI.Ins.CoolDown();

                        break;
                    case 4:
                        player.moveSpeed = Mathf.Round(player.moveSpeed * 0.66666f);
                        player.canMove = true;
                        ButtonControllerUI.Ins.CoolDown();

                        break;
                }

                activeMoveSpeed = player.moveSpeed;
            }
            coolCounter = currentCoolDown[playerID];
        }

        if (coolCounter > 0)
        {
            coolCounter -= Time.deltaTime;
        }
    }

    public void Dash()
    {
        if (coolCounter <= 0 && timeCounter <= 0)
        {
            player.canMove = false;
            PlayerController.Ins.SetCharacterState("Skill");

            activeMoveSpeed = dashSpeed;
            timeCounter = currentDuration[playerID];
        }
    }

    public void DualGun()
    {
        if (coolCounter <= 0 && timeCounter <= 0)
        {
            player.canMove = false;
            PlayerController.Ins.SetCharacterState("Skill");

            timeCounter = currentDuration[playerID];

            dualChecker = true;
            PlayerController.Ins.availableDupliGuns[PlayerController.Ins.currentGun].gameObject.SetActive(true);
        }

    }

    public void HolyShield()
    {
        if (coolCounter <= 0 && timeCounter <= 0)
        {
            player.canMove = false;
            PlayerController.Ins.SetCharacterState("Skill");

            PlayerController.Ins.col.enabled = false;

            timeCounter = currentDuration[playerID];
        }
    }

    public void BulletGrenade()
    {
        if (coolCounter <= 0 && timeCounter <= 0)
        {
            //foreach( Gun gun in PlayerController.Ins.availableGuns)
            //{
            //    gun.canExplode = true;
            //}         

            Weapon.Ins.canExplode = true;

            player.canMove = false;
            PlayerController.Ins.SetCharacterState("Skill");

            timeCounter = currentDuration[playerID];
        }
    }

    public void Speed()
    {
        if (coolCounter <= 0 && timeCounter <= 0)
        {
            player.canMove = false;
            PlayerController.Ins.SetCharacterState("Skill");

            timeCounter = currentDuration[playerID];
            PlayerController.Ins.moveSpeed *= 1.5f;
        }
    }

    public void UpdateSkillCoolDown()
    {
        switch (DynamicDataManager.Ins.CurPlayer)
        {
            case 0:
                dashCooldown = ResourceSystem.Ins.CharactersDatabase.Characters[0].Data.CoolDown[DynamicDataManager.Ins.CurPlayerCooldownUpgrade];
                break;

            case 1:
                dualCooldown = ResourceSystem.Ins.CharactersDatabase.Characters[1].Data.CoolDown[DynamicDataManager.Ins.CurPlayerCooldownUpgrade];
                break;

            case 2:
                holyShieldCooldown = ResourceSystem.Ins.CharactersDatabase.Characters[2].Data.CoolDown[DynamicDataManager.Ins.CurPlayerCooldownUpgrade];
                break;

            case 3:
                bulletNadeCooldown = ResourceSystem.Ins.CharactersDatabase.Characters[3].Data.CoolDown[DynamicDataManager.Ins.CurPlayerCooldownUpgrade];
                break;

            case 4:
                speedCooldown = ResourceSystem.Ins.CharactersDatabase.Characters[4].Data.CoolDown[DynamicDataManager.Ins.CurPlayerCooldownUpgrade];
                break;
            default:
                print("Incorrect intelligence level.");
                break;
        }

        SetDurationCoolDown();
    }
}
