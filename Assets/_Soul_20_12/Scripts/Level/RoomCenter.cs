﻿using API.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenter : MonoBehaviour
{
    public static RoomCenter Ins;

    //public ShopItem[] shopItem;

    //public int typeItem;

    public bool openWhenEnemiesCleared;
    public int enemyCount;
    public List<EnemyController> enemies;
    public List<EnemyController> enemiesSpawn = new List<EnemyController>();
    public List<BossController> bosses;

    public GameObject enemyGroup;

    [SerializeField] Collider2D colCheckPlayer;

    public Room theRoom;
    public Chest chest;

    public SpriteRenderer roomLayout;
    public Sprite[] roomIcon;

    [SerializeField] bool isEnemyCenter;
    public int numberOfEnemiesToSpawn = 10;
    public bool isBossCenter;
    public GameObject theBoss;
    bool isFirstTime = true;
    public bool spawnChest = false;

    public Transform checkPoint;
    public int maxX;
    public int minX;
    public int maxY;
    public int minY;


    private void Awake()
    {
        Ins = this;

        if (roomLayout != null)
        {
            roomLayout.sprite = roomIcon[0];

        }
    }
    void Start()
    {
        maxX = LevelManager.Ins.enemySpawnPos[0];
        minX = LevelManager.Ins.enemySpawnPos[1];
        maxY = LevelManager.Ins.enemySpawnPos[2];
        minY = LevelManager.Ins.enemySpawnPos[3];

        if (isEnemyCenter)
        {
            numberOfEnemiesToSpawn = LevelManager.Ins.CaculateEnemySpawn(numberOfEnemiesToSpawn);
            for (int i = 0; i <= numberOfEnemiesToSpawn - 1; i++)
            {
                GetTarget();

                int randomNum = Random.Range(0, LevelManager.Ins.enemyPrefabs.Count);

                GameObject enemySpawn = Instantiate(LevelManager.Ins.enemyPrefabs[randomNum], checkPoint.position, Quaternion.identity);

                enemySpawn.transform.parent = enemyGroup.transform;
            }
        }

        if (isEnemyCenter)
        {
            EnemyController[] enemyGr = enemyGroup.GetComponentsInChildren<EnemyController>();

            foreach (EnemyController enemy in enemyGr)
            {
                enemies.Add(enemy);

                enemy.gameObject.SetActive(false);
                enemyCount++;
            }
        }

        if (openWhenEnemiesCleared)
        {
            theRoom.closeWhenEntered = true;
        }
    }

    void Update()
    {
        if (enemiesSpawn.Count > 0 /*&& theRoom.roomActive && openWhenEnemiesCleared*/)
        {
            for (int i = 0; i < enemiesSpawn.Count; i++)
            {
                if (enemiesSpawn[i] == null)
                {
                    enemiesSpawn.RemoveAt(i);

                    i--;
                    enemyCount--;

                    GetRandomEnemies();
                }
            }

            if (enemyCount == 0)
            {
                theRoom.OpenDoors();
                spawnChest = true;
                chest.OpenChest();

                roomLayout.sprite = roomIcon[2];
                //SpawnChest();
            }
        }

        if (bosses.Count > 0/* && theRoom.roomActive && openWhenEnemiesCleared*/)
        {
            for (int i = 0; i < bosses.Count; i++)
            {
                if (bosses[i] == null)
                {
                    bosses.RemoveAt(i);
                    i--;
                }
            }

            if (bosses.Count == 0)
            {
                theRoom.OpenDoors();
                spawnChest = true;
                chest.OpenChest();

                roomLayout.sprite = roomIcon[2];
                //SpawnChest();
            }
        }
    }

    //public void SpawnChest()
    //{
    //    roomLayout.sprite = roomIcon[2];
    //    //Debug.Log("SpawmChest");
    //    if (isEnemyCenter || isBossCenter && spawnChest == true)
    //    {
    //        GetTarget();
    //        GameObject chestSpawn = Instantiate(LevelManager.Ins.chestSpawn, checkPoint.position, Quaternion.identity);
    //        chestSpawn.transform.parent = checkPoint.transform;
    //        spawnChest = false;
    //    }
    //}

    public void GetTarget()
    {
        if (checkPoint != null)
        {
            checkPoint.localPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0f);
            //Debug.Log(checkPoint.localPosition);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isBossCenter)
            {
                chest.gameObject.SetActive(true);
                if (bosses.Count > 0)
                {
                    if (isFirstTime == true)
                    {
                        LevelManager.Ins.bossShowUI.SetActive(true);
                        CanvasManager.Ins.CloseUI(UIName.GameplayUI);
                        PlayerController.Ins.isMove = false;
                        PlayerController.Ins.PlayerStopMove();
                        StartCoroutine(IEBossShowDisable());
                        isFirstTime = false;
                    }
                    roomLayout.sprite = roomIcon[1];
                    //colCheckPlayer.enabled = false;
                }
            }
            if (isEnemyCenter)
            {
                chest.gameObject.SetActive(true);
                roomLayout.sprite = roomIcon[1];

                GetRandomEnemies();
            }
        }
    }

    IEnumerator IEBossShowDisable()
    {
        yield return new WaitForSeconds(3f);
        CanvasManager.Ins.OpenUI(UIName.GameplayUI, null);
        ButtonControllerUI.Ins.OnEnableJoyStick();
        theBoss.SetActive(true);
        //CanvasManager.Ins.OpenUI(UIName.BossHubUI, null);
        LevelManager.Ins.bossHubUI.SetActive(true);
        PlayerController.Ins.isMove = true;
        LevelManager.Ins.bossShowUI.SetActive(false);
    }

    public void GetRandomEnemies()
    {
        if (isFirstTime == true)
        {
            for (int i = 0; i < 4; i++)
            {
                int randomIndex = Random.Range(0, enemies.Count);
                EnemyController randomElement = enemies[randomIndex];

                enemiesSpawn.Add(randomElement);
                enemies.RemoveAt(randomIndex);

                enemiesSpawn[i].gameObject.SetActive(true);
                isFirstTime = false;
            }
        }
        else
        {
            if (enemies.Count > 0)
            {
                int randomIndex = Random.Range(0, enemies.Count);
                EnemyController randomElement = enemies[randomIndex];

                enemiesSpawn.Add(randomElement);
                enemies.RemoveAt(randomIndex);

                enemiesSpawn[enemiesSpawn.Count - 1].gameObject.SetActive(true);
            }
        }
    }
}
