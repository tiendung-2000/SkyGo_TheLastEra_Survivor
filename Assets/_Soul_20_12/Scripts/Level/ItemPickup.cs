using System.Collections;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] ItemType itemType;
    public float waitToBeCollected;
    public float waitToBeDisappear = 5f;

    public int coinValue;
    public int healAmount;
    public float time;

    private void Update()
    {
        if (waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(IEDisappear());
    }

    IEnumerator IEDisappear()
    {
        yield return new WaitForSeconds(waitToBeDisappear);
        SmartPool.Ins.Despawn(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && waitToBeCollected <= 0)
        {
            switch (itemType)
            {
                case ItemType.Coin:
                    PlayerController.Ins.coinCollectFX.Play();
                    PlayerHub.Ins.coinCollect += coinValue;
                    SmartPool.Ins.Despawn(gameObject);
                    break;
                case ItemType.Health:
                    PlayerController.Ins.healthBuffFX.Play();
                    DataManager.Ins.HealPlayer(healAmount);
                    SmartPool.Ins.Despawn(gameObject);
                    break;
                case ItemType.Shield:

                    if (ShieldBuff.Ins.hasShield == true)
                    {
                        SmartPool.Ins.Despawn(gameObject);
                    }
                    else
                    {
                        ShieldBuff.Ins.hasShield = true;
                        ShieldBuff.Ins.col.enabled = true;
                        PlayerController.Ins.shieldBuffFX.gameObject.SetActive(true);
                        PlayerController.Ins.shieldBuffFX.Play(true);
                        PlayerController.Ins.StartCoroutine(IEBreakShield());
                        SmartPool.Ins.Despawn(gameObject);
                    }
                    break;
                case ItemType.Bomb:
                    BombActive.Ins.Bomb();
                    SmartPool.Ins.Despawn(gameObject);
                    break;
                case ItemType.None:
                    Debug.Log("None Item Type!");
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator IEBreakShield()
    {
        yield return new WaitForSeconds(time);
        PlayerController.Ins.shieldBuffFX.gameObject.SetActive(false);
        PlayerController.Ins.shiedBreakFX.gameObject.SetActive(true);
        PlayerController.Ins.shiedBreakFX.Play(true);
        ShieldBuff.Ins.hasShield = false;
        ShieldBuff.Ins.col.enabled = false;
    }
}

public enum ItemType
{
    Coin,
    Health,
    Shield,
    Bomb,
    None
}
