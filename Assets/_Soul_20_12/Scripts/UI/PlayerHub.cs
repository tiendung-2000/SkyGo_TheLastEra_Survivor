using UnityEngine;
using UnityEngine.UI;

public class PlayerHub : MonoBehaviour
{
    public static PlayerHub Ins;

    public Text healthText;
    public Text coinText;
    public int coinCollect;
    public Image healthSlider;

    private void Awake()
    {
        Ins = this;
    }

    void Start()
    {
        OnHealthChange(PlayerController.Ins.currentHealth);
        DynamicDataManager.Ins.OnHealthChange += OnHealthChange;
    }

    private void Update()
    {
        CoinInLevel();
    }

    public void OnHealthChange(int health)
    {
        int curPlayerMaxHP = ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.HP[DynamicDataManager.Ins.CurPlayerHPUpgrade];
        healthSlider.fillAmount = (float)health / curPlayerMaxHP;
        healthText.text = health.ToString() + "/" + curPlayerMaxHP;
    }

    public void CoinInLevel()
    {
        coinText.text = coinCollect.ToString();
    }
}
