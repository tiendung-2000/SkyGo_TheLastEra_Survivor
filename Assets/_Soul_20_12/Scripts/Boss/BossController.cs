using API.UI;
using Spine.Unity;
using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public static BossController Ins;
    public SkeletonAnimation ske;

    [Header("Boss")]
    public float currentHealth;
    //public GameObject deathEffect;
    public GameObject hitEffect;
    public Collider2D col;
    public BossHubUI bossHubUI;
    public GameObject levelExit;
    public float timeToDead;
    [SerializeField] SkeletonRendererCustomMaterials customMaterial;
    public bool playerOnZone = false;

    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public GameObject cointToDrop;
    public float itemDropPercent;
    private void Awake()
    {
        if (Ins == null)
        {
            Ins = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerOnZone = false;
    }

    //private void OnEnable()
    //{
    //    BossHubUI.Ins.bossHealthBar.maxValue = currentHealth;
    //    BossHubUI.Ins.bossHealthBar.value = currentHealth;
    //}

    public void TakeDamage(int damageAmount)
    {
        #region Desktop
        //currentHealth -= Mathf.RoundToInt(damageAmount / 2);
        #endregion

        #region Mobile
        currentHealth -= damageAmount;
        #endregion

        StartCoroutine(IETakeDamageEffect());
        if (currentHealth <= 0)
        {
            DropItem();
            ske.AnimationState.SetAnimation(0, BossAnimKeys.DIE, false);
            StartCoroutine(IEBossDead());
            col.enabled = false;

        }
        bossHubUI.bossHealthBar.value = currentHealth;
    }

    IEnumerator IETakeDamageEffect()
    {
        customMaterial.SetMaterialOverride();
        yield return new WaitForSeconds(0.2f);
        customMaterial.SetMaterialDefault();
    }

    IEnumerator IEBossDead()
    {
        yield return new WaitForSeconds(timeToDead);
        //CanvasManager.Ins.CloseUI(UIName.BossHubUI);
        LevelManager.Ins.bossHubUI.SetActive(false);

        levelExit.SetActive(true);
        Destroy(gameObject);
    }


    public void DropItem()
    {
        if (shouldDropItem)
        {
            Debug.Log("Drop Item");
            float dropChance = Random.Range(0f, 100f);

            if (dropChance < itemDropPercent)
            {
                int randomItem = Random.Range(0, itemsToDrop.Length);

                SmartPool.Ins.Spawn(itemsToDrop[randomItem], transform.position, transform.rotation);
            }
            AudioManager.Ins.SoundEffect(3);
            SmartPool.Ins.Spawn(cointToDrop, transform.position, transform.rotation);
        }
    }
}

public class BossAnimKeys
{
    public static readonly string DIE = "Die";
}
