using API.UI;
using UnityEngine.UI;

public class BossHubUI : BaseUIMenu
{
    public static BossHubUI Ins;

    public Slider bossHealthBar;

    private void Awake()
    {
        Ins = this;
    }

    void OnEnable()
    {
        BossController.Ins.bossHubUI = this;
        bossHealthBar.maxValue = BossController.Ins.currentHealth;
        bossHealthBar.value = BossController.Ins.currentHealth;
    }
}
