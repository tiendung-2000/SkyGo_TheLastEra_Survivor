using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Chest Animation")]
    public GameObject chestFX;
    public GameObject ChestAnim;

    [Header("Chest Properties")]
    private bool canOpen, isOpen, canSpawn;
    public int spawnTime;
    private int spawnCount;
    private float timeBetweenSpawn = 0.1f;
    public Transform spawnPoint;
    public Collider2D col;

    public GunPickup[] potentialGuns;
    public GameObject[] itemPickup;

    private void OnEnable()
    {
        canOpen = true;
        canSpawn = true;
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

        }
        DropItem();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ChestAnim.SetActive(true);
            AudioManager.Ins.SoundEffect(5);
            StartCoroutine(IEChestFX());
        }
    }

    void DropItem()
    {
        if (canSpawn == true && spawnCount <= spawnTime && timeBetweenSpawn <= 0)
        {
            Instantiate(itemPickup[0], spawnPoint.position, spawnPoint.rotation);
            spawnCount++;
            timeBetweenSpawn = 0.1f;
        }
    }

    IEnumerator IEChestFX()
    {
        yield return new WaitForSeconds(.1f);
        chestFX.SetActive(true);
        canOpen = true;
    }
}
