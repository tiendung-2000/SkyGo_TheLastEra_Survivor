using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Chest Animation")]
    public GameObject chestSprite;
    public GameObject chestAnim;
    public GameObject spawnAnim;
    public ParticleSystem spawnEffect;

    [Header("Chest Properties")]
    private bool canOpen, isOpen, canSpawn, canAdsSpawn;
    public int spawnTime;
    private int spawnCount;
    private float timeBetweenSpawn = 0.1f;
    public Transform spawnPoint;
    public Collider2D col;

    public GunPickup[] potentialGuns;
    public GameObject[] itemPickup;
    public GameObject[] adsItemPickup;

    private void OnEnable()
    {
        canOpen = false;
        canSpawn = false;
        canAdsSpawn = false;
        StartCoroutine(ActiveSprite());
    }

    IEnumerator ActiveSprite()
    {
        yield return new WaitForSeconds(.1f);
        chestSprite.SetActive(true);
    }

    private void Update()
    {
        timeBetweenSpawn -= Time.deltaTime;

        if (canOpen && !isOpen)
        {
            int gunSelect = Random.Range(0, potentialGuns.Length);

            Instantiate(potentialGuns[gunSelect], spawnPoint.position, spawnPoint.rotation);
            col.enabled = false;
            isOpen = true;
            Debug.Log("Drop Gun");
        }
        DropItem();
        DropAds();
    }

    public void OpenChest()
    {
        chestAnim.SetActive(true);
        AudioManager.Ins.SoundEffect(5);
        StartCoroutine(IEChestFX());
    }

    void DropItem()
    {
        //calculate:

        //int itemDrop = Random.Range(0, itemPickup.Length);

        if (canSpawn == true && spawnCount <= spawnTime && timeBetweenSpawn <= 0)
        {
            if (spawnCount <= 4)
            {
                SmartPool.Ins.Spawn(itemPickup[0], spawnPoint.position, spawnPoint.rotation);
            }
            else
            {
                SmartPool.Ins.Spawn(itemPickup[1], spawnPoint.position, spawnPoint.rotation);
            }
            spawnCount++;
            timeBetweenSpawn = 0.1f;
            //goto calculate;

            Debug.Log("DropItem");
        }
        else
        {
            return;
        }
    }

    void DropAds()
    {
        if (canAdsSpawn == true && canSpawn == true)
        {
            SmartPool.Ins.Spawn(adsItemPickup[0], spawnPoint.position, spawnPoint.rotation);

            int random = Random.Range(1, adsItemPickup.Length);

            SmartPool.Ins.Spawn(adsItemPickup[random], spawnPoint.position, spawnPoint.rotation);

            canAdsSpawn = false;
            Debug.Log("DropAds");

        }
        return;

    }

    IEnumerator IEChestFX()
    {
        yield return new WaitForSeconds(.1f);
        col.enabled = false;
        chestSprite.SetActive(false);
        spawnEffect.gameObject.SetActive(true);
        spawnEffect.Play();
        canOpen = true;
        canSpawn = true;
        canAdsSpawn = true;
    }
}
