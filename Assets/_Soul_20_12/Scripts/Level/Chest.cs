using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Chest Animation")]
    public GameObject chestFX;
    public GameObject ChestAnim;

    [Header("Chest Properties")]
    public GunPickup[] potentialGuns;
    private bool canOpen, isOpen;
    public Transform spawnPoint;
    public Collider2D col;

    private void OnEnable()
    {
        canOpen = true;
    }

    private void Update()
    {
        if (canOpen && !isOpen)
        {
            int gunSelect = Random.Range(0, potentialGuns.Length);

            Instantiate(potentialGuns[gunSelect], spawnPoint.position, spawnPoint.rotation);
            col.enabled = false;
            isOpen = true;
        }
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

    IEnumerator IEChestFX()
    {
        yield return new WaitForSeconds(.1f);
        chestFX.SetActive(true);
        canOpen = true;
    }
}
