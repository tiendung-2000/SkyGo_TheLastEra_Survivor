using UnityEngine;
using API.UI;
using System.Collections.Generic;

public enum GameState
{
    Tutorial,
    Menu,
    Gameplay,
    Complete
}

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private GameState state;

    public GameObject bossShowUI;

    public int reviveCount = 0;
    public bool isPaused;
    public bool isTestLevel = false;

    public GameObject chestSpawn;
    public List<GameObject> enemyPrefabs;
    public List<int> enemySpawnPos;

    #region Enemy Scale

    #region Enemy Fire Rate
    public float CaculateFireRateMin(float fireRateMin)
    {
        Debug.Log("Caculate Fire Rate Min");
        switch (DynamicDataManager.Ins.CurLevel)
        {
            case 0:
                fireRateMin *= 1.5f;
                break;
            case 1:
                fireRateMin *= 1.4f;
                break;
            case 2:
                fireRateMin *= 1.3f;
                break;
            case 3:
                fireRateMin *= 1.2f;
                break;
            case 4:
                fireRateMin *= 1.1f;
                break;
            case 5:
                fireRateMin *= 1.0f;
                break;
            case 6:
                fireRateMin *= .9f;
                break;
            case 7:
                fireRateMin *= .8f;
                break;
            case 8:
                fireRateMin *= .7f;
                break;
            case 9:
                fireRateMin *= .6f;
                break;
            default:
                break;
        }
        return fireRateMin;
    }
    public float CaculateFireRateMax(float fireRateMax)
    {
        Debug.Log("Caculate Fire Rate Max");
        switch (DynamicDataManager.Ins.CurLevel)
        {
            case 0:
                fireRateMax *= 1.5f;
                break;
            case 1:
                fireRateMax *= 1.4f;
                break;
            case 2:
                fireRateMax *= 1.3f;
                break;
            case 3:
                fireRateMax *= 1.2f;
                break;
            case 4:
                fireRateMax *= 1.1f;
                break;
            case 5:
                fireRateMax *= 1.0f;
                break;
            case 6:
                fireRateMax *= .9f;
                break;
            case 7:
                fireRateMax *= .8f;
                break;
            case 8:
                fireRateMax *= .7f;
                break;
            case 9:
                fireRateMax *= .6f;
                break;
            default:
                break;
        }

        return fireRateMax;
    }
    #endregion

    #region Enemy Health
    public int CaculateHealth(int health)
    {
        Debug.Log("Caculate Health");
        switch (DynamicDataManager.Ins.CurLevel)
        {
            case 0:
                health += 0;
                break;
            case 1:
                health += 10;
                break;
            case 2:
                health += 20;
                break;
            case 3:
                health += 30;
                break;
            case 4:
                health += 40;
                break;
            case 5:
                health += 50;
                break;
            case 6:
                health += 60;
                break;
            case 7:
                health += 70;
                break;
            case 8:
                health += 80;
                break;
            case 9:
                health += 90;
                break;
            default:
                //return health;
                break;
        }
        return health;
    }
    #endregion

    #region Number Of Enemy Spawn
    public int CaculateEnemySpawn(int enemyNumber)
    {
        Debug.Log("Caculate Enemy Count");
        switch (DynamicDataManager.Ins.CurLevel)
        {
            case 0:
                enemyNumber += 0;
                break;
            case 1:
                enemyNumber += 2;
                break;
            case 2:
                enemyNumber += 4;
                break;
            case 3:
                enemyNumber += 6;
                break;
            case 4:
                enemyNumber += 8;
                break;
            case 5:
                enemyNumber += 10;
                break;
            case 6:
                enemyNumber += 12;
                break;
            case 7:
                enemyNumber += 14;
                break;
            case 8:
                enemyNumber += 16;
                break;
            case 9:
                enemyNumber += 18;
                break;
            default:
                return enemyNumber;
        }
        return enemyNumber;
    }
    #endregion
    #endregion
    public void ReviveReset()
    {
        reviveCount = 0;
    }

    public void PauseUnpause()
    {
        if (!isPaused)
        {
            CanvasManager.Ins.OpenUI(UIName.PauseSettingUI, null);
            isPaused = true;

            Time.timeScale = 0f;
        }
        else
        {
            CanvasManager.Ins.CloseUI(UIName.PauseSettingUI);
            isPaused = false;

            Time.timeScale = 1f;
        }
    }

    #region GameState

    private void Start()
    {
        ChangeState(GameState.Menu);
    }

    public void ChangeState(GameState gameState)
    {
        this.state = gameState;
    }

    public bool IsState(GameState gameState)
    {
        return this.state == gameState;
    }
    #endregion

    #region Deleted
    //    public static LevelManager instance;   

    //    //public float waitToLoad = 4f;

    //    //public string nextLevel;

    //    //public bool isPaused;

    //    //public int currentCoins;

    //    //public Transform startPoint;

    //    private void Awake()
    //    {
    //        instance = this;
    //    }

    //    //// Start is called before the first frame update
    //    void Start()
    //    {
    //        //PlayerController.instance.transform.position = startPoint.position;
    //        //PlayerController.instance.canMove = true;

    //        //currentCoins = CharacterTracker.instance.currentCoins;

    //        //Time.timeScale = 1f;

    //        //PlayerHub.Ins.coinText.text = currentCoins.ToString();

    //    }

    //    //// Update is called once per frame
    //    //void Update()
    //    //{

    //    //}

    //    //public IEnumerator LevelEnd()
    //    //{
    //    //    AudioManager.instance.PlayLevelWin();

    //    //    PlayerController.instance.canMove = false;

    //    //    yield return new WaitForSeconds(waitToLoad);

    //    //    //DataManager.Ins.currentCoins = currentCoins;
    //    //    //DataManager.Ins.currentHealth = PlayerHealthController.instance.currentHealth;
    //    //    //DataManager.Ins.maxHealth = PlayerHealthController.instance.maxHealth;

    //    //    SceneManager.LoadScene(nextLevel);
    //    //}

    //    //public void PauseUnpause()
    //    //{
    //    //    if (!isPaused)
    //    //    {
    //    //        UIController.instance.pauseMenu.SetActive(true);

    //    //        isPaused = true;

    //    //        Time.timeScale = 0f;
    //    //    }
    //    //    else
    //    //    {
    //    //        UIController.instance.pauseMenu.SetActive(false);

    //    //        isPaused = false;

    //    //        Time.timeScale = 1f;
    //    //    }
    //    //}

    //    //public void GetCoins(int amount)
    //    //{
    //    //    currentCoins += amount;

    //    //    //UIController.instance.coinText.text = currentCoins.ToString();
    //    //    //PlayerHub.Ins.coinText.text = amount.ToString();
    //    //}

    //    //public void SpendCoins(int amount)
    //    //{
    //    //    currentCoins -= amount;

    //    //    if (currentCoins < 0)
    //    //    {
    //    //        currentCoins = 0;
    //    //    }

    //    //    //UIController.instance.coinText.text = currentCoins.ToString();
    //    //    //PlayerHub.Ins.coinText.text = amount.ToString();

    //    //}

    #endregion
}
