using System.Collections;
using UnityEngine;

public class GunChest : MonoBehaviour
{
    public GunPickup[] potentialGuns;

    public SpriteRenderer theSR;
    public Sprite chestOpen;

    public GameObject notification;

    private bool canOpen, isOpen;

    public Transform spawnPoint;

    public float scaleSpeed = 2f;

    void Update()
    {
        if (canOpen && !isOpen)
        {

            int gunSelect = Random.Range(0, potentialGuns.Length);

            SmartPool.Ins.Spawn(potentialGuns[gunSelect], spawnPoint.position, spawnPoint.rotation);

            theSR.sprite = chestOpen;

            isOpen = true;

            transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

        }

        if (isOpen)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, Time.deltaTime * scaleSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            notification.SetActive(true);

            canOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            notification.SetActive(false);

            canOpen = false;
        }
    }
}
