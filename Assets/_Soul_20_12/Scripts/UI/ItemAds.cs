using API.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAds : MonoBehaviour
{
    [SerializeField] AdsType adsType;
    public float waitToBeCollected;

    //public List<GameObject> items;
    //public List<GameObject> weapons;

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
                    //LevelManager.Ins.RewardAdsItem();
                    CanvasManager.Ins.OpenUI(UIName.WeaponAds5sPopup, null);
                    break;
                case AdsType._10s:
                    //LevelManager.Ins.RewardAdsNormalGun();
                    CanvasManager.Ins.OpenUI(UIName.WeaponAds10sPopup, null);
                    break;
                case AdsType._30s:
                    //LevelManager.Ins.RewardAdsLegendGun();
                    CanvasManager.Ins.OpenUI(UIName.WeaponAds30sPopup, null);
                    break;
            }
        }
    }
}

public enum AdsType
{
    _5s,
    _10s,
    _30s
}
