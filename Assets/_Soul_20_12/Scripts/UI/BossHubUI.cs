using API.UI;
using UnityEngine.UI;

public class BossHubUI : BaseUIMenu
{
    public static BossHubUI Ins;

    public Slider bossHealthBar;

    public Text bossName;

    private void Awake()
    {
        Ins = this;
    }

    void OnEnable()
    {
        bossName.text = BossShow.Ins.bossName.text;
        BossController.Ins.bossHubUI = this;
        bossHealthBar.maxValue = BossController.Ins.currentHealth;
        bossHealthBar.value = BossController.Ins.currentHealth;
    }
}
