using System.Collections.Generic;
using UnityEngine;

public class ResourceSystem : StaticInstance<ResourceSystem>
{
    public int rewards;

    public ScriptableRewardsLevel RewardsLevel;
    public ScriptableShopData ShopData;

    public GroupCharactersbase CharactersDatabase;

    public List<PlayerController> players;

    public List<LevelSO> levels;

    public GameObject tutorialLevel;

    public GameObject CurLevelGameObj;

    public bool IsDataLoaded;

    //protected override void Awake()
    //{
    //    base.Awake();
    //    Sequence LoadData = DOTween.Sequence();
    //    LoadData.AppendCallback(AssembleResources)
    //            //.AppendCallback(Pooling)
    //            .OnComplete(() => IsDataLoaded = true);
    //}

    //private void AssembleResources()
    //{
    //    //ExampleEnemies = Resources.LoadAll<ScriptableExampleEnemy>("ExampleEnemies").ToList();
    //    //_ExampleEnemiesDict = ExampleEnemies.ToDictionary(r => r.EnemyType, r => r);
    //    //print("Enemies Loaded?");
    //}

    public void SpawnLevel(int spawnLevel)
    {
        int curLevel = DynamicDataManager.Ins.CurLevel;

        switch (spawnLevel)
        {
            case 3:
                CurLevelGameObj = SmartPool.Ins.Spawn(tutorialLevel, transform.position, transform.rotation);
                break;

            case 2: //Play
                CurLevelGameObj = SmartPool.Ins.Spawn(levels[curLevel].levelPrefab, new UnityEngine.Vector3(0, 0, 0), new UnityEngine.Quaternion());

                LevelManager.Ins.ReviveReset();

                break;
            case 1: //Replay
                SmartPool.Ins.Despawn(CurLevelGameObj);
                CurLevelGameObj = SmartPool.Ins.Spawn(levels[curLevel].levelPrefab, new UnityEngine.Vector3(0, 0, 0), new UnityEngine.Quaternion());

                LevelManager.Ins.ReviveReset();

                break;
            case 0: //Next Level
                SmartPool.Ins.Despawn(CurLevelGameObj);
                curLevel++;
                CurLevelGameObj = SmartPool.Ins.Spawn(levels[curLevel].levelPrefab, new UnityEngine.Vector3(0, 0, 0), new UnityEngine.Quaternion());

                LevelManager.Ins.ReviveReset();

                break;
        }
    }
}