using System.Collections;
using UnityEngine;

public class ShieldPickup : MonoBehaviour
{
    public float waitToBeCollected;
    public float time;

    void Update()
    {
        if (waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }
        else
        {
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerController.Ins.shieldBuffFX.isStopped)
            {
                ShieldBuff.Ins.col.enabled = true;
                PlayerController.Ins.shieldBuffFX.gameObject.SetActive(true);
                PlayerController.Ins.shieldBuffFX.Play(true);
                PlayerController.Ins.StartCoroutine(IEBreakShield());

                SmartPool.Ins.Despawn(gameObject);
            }
        }
    }

    IEnumerator IEBreakShield()
    {
        yield return new WaitForSeconds(time);
        PlayerController.Ins.shieldBuffFX.gameObject.SetActive(false);
        PlayerController.Ins.shiedBreakFX.gameObject.SetActive(true);
        PlayerController.Ins.shiedBreakFX.Play(true);
    }
}
