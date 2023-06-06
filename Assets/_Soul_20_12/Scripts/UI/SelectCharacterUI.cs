using API.UI;
using MagneticScrollView;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacterUI : BaseUIMenu
{
    public static SelectCharacterUI Ins;

    public ParticleSystem upgradeStats, upgradeSkill;

    public Button startButton;
    public Button buyCharacterButton;
    public Text characterPriceText;
    [SerializeField] Button backButton;
    [SerializeField] Button upgradePlayerButton;
    [SerializeField] Button upgradeSkillButton;

    [SerializeField] List<SkeletonDataAsset> playerImage;
    [SerializeField] SkeletonGraphic playerSprite;
    [SerializeField] GameObject scroll;

    [SerializeField] Text healthText;
    [SerializeField] Text speedText;
    [SerializeField] Text priceUpgradeHealth;

    [SerializeField] Image skillImg;
    [SerializeField] Text skillDetail;
    [SerializeField] Text cooldownText;
    [SerializeField] Text priceUpgradeSkill;

    [SerializeField] List<GameObject> healthStar;
    [SerializeField] List<GameObject> skillStar;

    public List<PlayerItem> listPlayer = new List<PlayerItem>();

    private void Awake()
    {
        Ins = this;
    }

    private void OnEnable()
    {
        StartCoroutine(LoadData());
        SetUpPlayer();
        PlayerAnimation();

        if (scroll.transform.childCount != 0)
        {
            scroll.GetComponent<MagneticScrollRect>().ResetScroll();
            scroll.GetComponent<MagneticScrollRect>().StartAutoArranging();
        }

        scroll.GetComponent<MagneticScrollRect>().StartAutoArranging();

        scroll.GetComponent<MagneticScrollRect>().m_currentSelected = DynamicDataManager.Ins.CurPlayer;
        scroll.GetComponent<MagneticScrollRect>().StartScrolling();

        CheckPlayerUnlocked();
        OnSwitchPlayer();

    }

    public void SetUpPlayer()
    {
        foreach (PlayerItem player in listPlayer)
        {
            if (DynamicDataManager.IsCharacterUnlocked(player.id) == true)
            {
                DynamicDataManager.Ins.CurPlayer = player.id;
                player.locked.SetActive(false);
            }
            else
            {
                player.locked.SetActive(true);
            }
        }
    }

    public void CheckPlayerUnlocked()
    {
        if (DynamicDataManager.IsCharacterUnlocked(DynamicDataManager.Ins.CurPlayer) == true)
        {
            startButton.gameObject.SetActive(true);
            buyCharacterButton.gameObject.SetActive(false);
        }
        else
        {
            startButton.gameObject.SetActive(false);
            buyCharacterButton.gameObject.SetActive(true);
        }
    }

    public void OnBuyCharacter()
    {
        int priceToUnlock = ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.priceToUnlock;
        if (DynamicDataManager.Ins.CurNumCoin >= priceToUnlock)
        {
            AudioManager.Ins.SoundUIPlay(3);

            DynamicDataManager.Ins.CurNumCoin -= priceToUnlock;
            DynamicDataManager.AddNewCharacterUnlocked(DynamicDataManager.Ins.CurPlayer);
            startButton.gameObject.SetActive(true);
            buyCharacterButton.gameObject.SetActive(false);
            SetUpPlayer();
            OnSwitchPlayer();
        }
        else
        {
            CanvasManager.Ins.OpenUI(UIName.ShopUI, null);
        }
    }

    private void Start()
    {
        backButton.onClick.AddListener(OnClickBackButton);
        startButton.onClick.AddListener(OnClickStartButton);
        upgradePlayerButton.onClick.AddListener(OnClickUpgradePlayerButton);
        upgradeSkillButton.onClick.AddListener(OnClickUpgradeSkillButton);

        buyCharacterButton.onClick.AddListener(OnBuyCharacter);
    }

    IEnumerator LoadData()
    {
        yield return new WaitUntil(() => ResourceSystem.Ins && ResourceSystem.Ins.IsDataLoaded);

        OnHealthChange(ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.HP[DynamicDataManager.Ins.CurPlayerHPUpgrade]);
        DynamicDataManager.Ins.OnHealthChange += OnHealthChange;

        OnSpeedChange(ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.Speed[DynamicDataManager.Ins.CurPlayerSpeedUpgrade]);
        DynamicDataManager.Ins.OnSpeedUpgrade += OnSpeedChange;

        OnCooldownChange(ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.CoolDown[DynamicDataManager.Ins.CurPlayerCooldownUpgrade]);
        DynamicDataManager.Ins.OnCooldownUpgrade += OnCooldownChange;

        if (DynamicDataManager.Ins.CurPlayerHPUpgrade < 4)
        {
            OnHealthPriceChange(ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].UpgradeData.HPUpgradePrice[DynamicDataManager.Ins.CurPlayerHPUpgrade]);
            DynamicDataManager.Ins.OnHealthPrice += OnHealthPriceChange;
        }
        if (DynamicDataManager.Ins.CurPlayerCooldownUpgrade < 4)
        {
            OnSkillPriceChange(ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].UpgradeData.CoolDownUpgradePrice[DynamicDataManager.Ins.CurPlayerCooldownUpgrade]);
            DynamicDataManager.Ins.OnSkillPrice += OnSkillPriceChange;
        }
    }

    public void OnClickStartButton()
    {
        OnStart();
        //CanvasManager.Ins.CloseUI(UIName.CoinBar);
    }

    public void OnClickBackButton()
    {
        OnBack();
    }

    public void OnClickUpgradePlayerButton()
    {
        OnUpgradeHPPlayer();
    }

    public void OnClickUpgradeSkillButton()
    {
        OnUpgradeCooldownPlayer();
    }

    public void OnBack()
    {
        AudioManager.Ins.SoundUIPlay(2);

        UITransition.Ins.ShowTransition(() =>
        {
            CanvasManager.Ins.OpenUI(UIName.SelectLevelUI, null);
            Close();
        });
    }

    public void OnSwitchPlayer()
    {
        scroll.GetComponent<MagneticScrollRect>().m_currentSelected = DynamicDataManager.Ins.CurPlayer;

        int curLevelHP = ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.HP.Count;
        int curLevelCD = ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.CoolDown.Count;

        upgradePlayerButton.gameObject.SetActive(DynamicDataManager.Ins.CurPlayerHPUpgrade == curLevelHP - 1 ? false : true);

        upgradeSkillButton.gameObject.SetActive(DynamicDataManager.Ins.CurPlayerCooldownUpgrade == curLevelCD - 1 ? false : true);

        int curPlayerHP = ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.HP[DynamicDataManager.Ins.CurPlayerHPUpgrade];
        int curPlayerSPD = ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.Speed[DynamicDataManager.Ins.CurPlayerSpeedUpgrade];
        int curPlayerCD = ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.CoolDown[DynamicDataManager.Ins.CurPlayerCooldownUpgrade];

        OnHealthChange(curPlayerHP);
        DynamicDataManager.Ins.OnHPUpgrade = OnHealthChange;

        OnSpeedChange(curPlayerSPD);
        DynamicDataManager.Ins.OnSpeedUpgrade = OnSpeedChange;

        OnCooldownChange(curPlayerCD);
        DynamicDataManager.Ins.OnCooldownUpgrade = OnCooldownChange;

        if (DynamicDataManager.Ins.CurPlayerHPUpgrade < 4)
        {
            int upgradeHPPrice = ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].UpgradeData.HPUpgradePrice[DynamicDataManager.Ins.CurPlayerHPUpgrade];
            OnHealthPriceChange(upgradeHPPrice);
            DynamicDataManager.Ins.OnHealthPrice = OnHealthPriceChange;
        }

        if (DynamicDataManager.Ins.CurPlayerCooldownUpgrade < 4)
        {
            int upgradePriceCD = ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].UpgradeData.CoolDownUpgradePrice[DynamicDataManager.Ins.CurPlayerCooldownUpgrade];
            OnSkillPriceChange(upgradePriceCD);
            DynamicDataManager.Ins.OnSkillPrice = OnSkillPriceChange;
        }
        skillImg.sprite = ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.skillImg;
        skillDetail.text = ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.skillDetail;

        if (DynamicDataManager.IsCharacterUnlocked(DynamicDataManager.Ins.CurPlayer) == true)
        {
            upgradePlayerButton.gameObject.SetActive(true);
            upgradeSkillButton.gameObject.SetActive(true);
        }
        else
        {
            upgradePlayerButton.gameObject.SetActive(false);
            upgradeSkillButton.gameObject.SetActive(false);
        }


        if (DynamicDataManager.Ins.CurPlayerHPUpgrade == ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.HP.Count - 1)
        {
            upgradePlayerButton.gameObject.SetActive(false);
        }
        //else
        //{
        //    upgradePlayerButton.gameObject.SetActive(true);
        //}

        if (DynamicDataManager.Ins.CurPlayerCooldownUpgrade == ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.CoolDown.Count - 1)
        {
            upgradeSkillButton.gameObject.SetActive(false);
        }

        UpgradeLevel();
        PlayerAnimation();
    }

    public void OnUpgradeHPPlayer()
    {
        upgradeStats.Play();

        int maxHpLevel = ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.HP.Count - 1;
        int upgradeHPPrice = ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].UpgradeData.HPUpgradePrice[DynamicDataManager.Ins.CurPlayerHPUpgrade];

        int maxSPDLevel = ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.Speed.Count - 1;

        if (DynamicDataManager.Ins.CurPlayerHPUpgrade < maxHpLevel && DynamicDataManager.Ins.CurNumCoin >= upgradeHPPrice)
        {
            AudioManager.Ins.SoundUIPlay(5);

            DynamicDataManager.Ins.CurNumCoin -= upgradeHPPrice;
            DynamicDataManager.Ins.CurPlayerHPUpgrade++;
            DynamicDataManager.Ins.CurPlayerSpeedUpgrade++;
            OnHealthChange(ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.HP[DynamicDataManager.Ins.CurPlayerHPUpgrade]);
            OnSpeedChange(ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.Speed[DynamicDataManager.Ins.CurPlayerHPUpgrade]);
            if (DynamicDataManager.Ins.CurPlayerHPUpgrade < 4)
            {
                OnHealthPriceChange(ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].UpgradeData.HPUpgradePrice[DynamicDataManager.Ins.CurPlayerHPUpgrade]);
            }
        }
        else
        {
            AudioManager.Ins.SoundUIPlay(2);

            CanvasManager.Ins.OpenUI(UIName.ShopUI, null);
        }

        if (DynamicDataManager.Ins.CurPlayerHPUpgrade == maxHpLevel)
        {
            upgradePlayerButton.gameObject.SetActive(false);
        }

        for (int i = 1; i <= maxHpLevel; i++)
        {
            if (i <= DynamicDataManager.Ins.CurPlayerHPUpgrade)
            {
                healthStar[i].SetActive(true);
            }
            else
            {
                healthStar[i].SetActive(false);
            }
        }
    }

    public void OnUpgradeCooldownPlayer()
    {
        upgradeSkill.Play();

        int maxCooldownLevel = ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.CoolDown.Count - 1;
        int upgradePriceCD = ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].UpgradeData.CoolDownUpgradePrice[DynamicDataManager.Ins.CurPlayerCooldownUpgrade];

        if (DynamicDataManager.Ins.CurPlayerCooldownUpgrade < maxCooldownLevel && DynamicDataManager.Ins.CurNumCoin >= upgradePriceCD)
        {
            AudioManager.Ins.SoundUIPlay(5);

            DynamicDataManager.Ins.CurNumCoin -= upgradePriceCD;
            DynamicDataManager.Ins.CurPlayerCooldownUpgrade++;
            OnCooldownChange(ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.CoolDown[DynamicDataManager.Ins.CurPlayerCooldownUpgrade]);
            if (DynamicDataManager.Ins.CurPlayerCooldownUpgrade < 4)
            {
                OnSkillPriceChange(ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].UpgradeData.CoolDownUpgradePrice[DynamicDataManager.Ins.CurPlayerCooldownUpgrade]);
            }
        }
        else
        {
            AudioManager.Ins.SoundUIPlay(2);

            CanvasManager.Ins.OpenUI(UIName.ShopUI, null);
        }

        if (DynamicDataManager.Ins.CurPlayerCooldownUpgrade == maxCooldownLevel)
        {
            upgradeSkillButton.gameObject.SetActive(false);
        }

        for (int i = 1; i <= maxCooldownLevel; i++)
        {
            if (i <= DynamicDataManager.Ins.CurPlayerCooldownUpgrade)
            {
                skillStar[i].SetActive(true);
            }
            else
            {
                skillStar[i].SetActive(false);
            }
        }
    }

    public void OnHealthChange(int health)
    {
        healthText.text = health.ToString();
    }

    public void OnSpeedChange(int speed)
    {
        speedText.text = speed.ToString();
    }

    public void OnCooldownChange(int cooldown)
    {
        cooldownText.text = "Cooldown: " + cooldown.ToString();
    }

    public void OnHealthPriceChange(int healthPrice)
    {
        priceUpgradeHealth.text = healthPrice.ToString() + "$";
    }

    public void OnSkillPriceChange(int skillPrice)
    {
        priceUpgradeSkill.text = skillPrice.ToString() + "$";
    }

    public void OnStart()
    {
        AudioManager.Ins.SoundUIPlay(1);

        CanvasManager.Ins.OpenUI(UIName.LoadingUI, null);
        CanvasManager.Ins.CloseUI(UIName.CoinBar);
        StartCoroutine(IESpawnLevel());
        //StartCoroutine(IEPlaySound());

        int openPopup = 5;
        if (Random.Range(1, 10) < openPopup)
        {
            CanvasManager.Ins.OpenUI(UIName.WeaponPopup, null);
        }

        CanvasManager.Ins.OpenUI(UIName.GameplayUI, null);
        ResourceSystem.Ins.players[DynamicDataManager.Ins.CurPlayer].gameObject.SetActive(true);
        CharacterSelectManager.Ins.activePlayer = ResourceSystem.Ins.players[DynamicDataManager.Ins.CurPlayer];

        GamePlayController.Ins.ResetPlayerStats();
        GamePlayController.Ins.GunSetUp();
    }

    IEnumerator IESpawnLevel()
    {
        yield return new WaitForSeconds(.1f);

        ResourceSystem.Ins.SpawnLevel(2);
        Close();

    }

    public void PlayerAnimation()
    {
        playerSprite.skeletonDataAsset = playerImage[DynamicDataManager.Ins.CurPlayer];
        playerSprite.skeletonDataAsset.GetSkeletonData(true);
        playerSprite.Initialize(true);
    }

    public void UpgradeLevel()
    {
        int maxHpLevel = ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.HP.Count - 1;
        int maxCooldownLevel = ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.CoolDown.Count - 1;

        for (int i = 1; i <= maxHpLevel; i++)
        {
            if (i <= DynamicDataManager.Ins.CurPlayerHPUpgrade)
            {
                healthStar[i].SetActive(true);
            }
            else
            {
                healthStar[i].SetActive(false);
            }
        }
        for (int i = 1; i <= maxCooldownLevel; i++)
        {
            if (i <= DynamicDataManager.Ins.CurPlayerCooldownUpgrade)
            {
                skillStar[i].SetActive(true);
            }
            else
            {
                skillStar[i].SetActive(false);
            }
        }
    }
}
