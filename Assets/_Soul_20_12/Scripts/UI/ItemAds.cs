using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAds : MonoBehaviour
{
    [SerializeField] AdsType adsType;
    public float waitToBeCollected;

    private void Update()
    {
        if (waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }

        if (LevelManager.Ins.IsState(GameState.Menu))
        {
            SmartPool.Ins.Despawn(gameObject);

            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            switch (adsType)
            {
                case AdsType._5s:
                    LevelManager.Ins.RewardAdsItem();
                    break;
                case AdsType._15s:
                    LevelManager.Ins.RewardAdsNormalGun();
                    break;
                case AdsType._30s:
                    LevelManager.Ins.RewardAdsLegendGun();
                    break;
            }

        }
    }
}

public enum AdsType
{
    _5s,
    _15s,
    _30s
}
