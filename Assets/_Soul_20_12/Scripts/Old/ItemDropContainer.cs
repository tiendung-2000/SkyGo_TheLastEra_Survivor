using System.Collections.Generic;
using UnityEngine;

public class ItemDropContainer : MonoBehaviour
{
    private static ItemDropContainer _instance;
    public static ItemDropContainer Instance
    {
        get
        {
            return _instance;
        }
    }

    public List<ItemDropRate> itemDropRates = new List<ItemDropRate>();

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    public void DropItem(Vector3 pos)
    {
        itemDropRates.ForEach(i =>
        {
            if (Random.Range(0, 100) < i.rate)
            {
                var randomPos = new Vector3(Random.Range(pos.x - .5f, pos.x + .5f), Random.Range(pos.y - 1, pos.y + 1));
                var item = SmartPool.Ins.Spawn(i.item, randomPos, Quaternion.identity);
            }
        });
    }

}

[System.Serializable]
public class ItemDropRate
{
    public GameObject item;
    public float rate;
}

