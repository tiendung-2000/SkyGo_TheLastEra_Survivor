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

    public bool playerOnZone = false;

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
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            ske.AnimationState.SetAnimation(0, BossAnimKeys.DIE, false);
            StartCoroutine(IEBossDead());
            col.enabled = false;
        }
        bossHubUI.bossHealthBar.value = currentHealth;
    }

    IEnumerator IEBossDead()
    {
        yield return new WaitForSeconds(timeToDead);
        CanvasManager.Ins.CloseUI(UIName.BossHubUI);
        levelExit.SetActive(true);
        Destroy(gameObject);
    }

    bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;

    public void DropItem()
    {
        if (shouldDropItem)
        {
            float dropChance = Random.Range(0f, 100f);

            if (dropChance < itemDropPercent)
            {
                int randomItem = Random.Range(0, itemsToDrop.Length);

                SmartPool.Ins.Spawn(itemsToDrop[randomItem], transform.position, transform.rotation);
            }
        }
    }
}

public class BossAnimKeys
{
    public static readonly string DIE = "Die";
}
