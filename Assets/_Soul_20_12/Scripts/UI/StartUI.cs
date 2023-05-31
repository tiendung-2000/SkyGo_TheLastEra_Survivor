using API.UI;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : BaseUIMenu
{
    [SerializeField] GameObject start;
    [SerializeField] Button startButton;

    private void Start()
    {
        startButton.onClick.AddListener(OnStart);
    }

    public void OnStart()
    {
        if (DynamicDataManager.Ins.CurTutorialStep == 0)
        {
            ResourceSystem.Ins.SpawnLevel(3);
            CanvasManager.Ins.OpenUI(UIName.LoadingUI, null);
            CanvasManager.Ins.OpenUI(UIName.GameplayUI, null);

            ResourceSystem.Ins.players[DynamicDataManager.Ins.CurPlayer].gameObject.SetActive(true);
            CharacterSelectManager.Ins.activePlayer = ResourceSystem.Ins.players[DynamicDataManager.Ins.CurPlayer];
            GamePlayController.Ins.ResetPlayerStats();
            start.gameObject.SetActive(false);

            AudioManager.Ins.MusicOff();

            AudioManager.Ins.PlayIngameBGM(0);
        }
        else
        {
            AudioManager.Ins.SoundUIPlay(0);
            UITransition.Ins.ShowTransition(() =>
            {
                start.gameObject.SetActive(false);

                CanvasManager.Ins.OpenUI(UIName.SelectLevelUI, null);
            });
        }
    }
}
