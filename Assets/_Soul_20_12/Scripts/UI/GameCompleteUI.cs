using API.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameCompleteUI : BaseUIMenu
{
    [SerializeField] Button homeButton;

    [SerializeField] Text coinBonusText;
    [SerializeField] Text coinCollectText;
    [SerializeField] Text coinTotalText;

    [SerializeField] GameObject panel;
    [SerializeField] Image imageBG;

    int coinBonus;
    int coinCollect;
    int coinTotal;

    int coinDefault = 0;

    [SerializeField] Button watchAdsButton;

    private void Start()
    {
        homeButton.onClick.AddListener(OnClickHomeButton);
        watchAdsButton.onClick.AddListener(OnClickWatchAdsButton);
        //replayButton.onClick.AddListener(OnClickReplayButton);
        //nextButton.onClick.AddListener(OnClickNextButton);
    }

    private void OnEnable()
    {
        imageBG.DOFade(.5f, 1.5f);
        panel.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 1.5f);
        StartCoroutine(IEShowComplete());
        StartCoroutine(IEDelayBackHomeScene());
    }

    private void OnDisable()
    {
        imageBG.DOFade(0f, 0f);
        panel.transform.DOScale(new Vector3(0f, 0f, 0f), 0f);
        homeButton.transform.DOScale(new Vector3(0f, 0f, 0f), 0f);
    }

    IEnumerator IEShowComplete()
    {
        yield return new WaitForSeconds(1.5f);

        Sequence getCoin = DOTween.Sequence();

        getCoin.AppendCallback(() =>
        {
            //Debug.Log("Coin Caculate");
            if (LevelGate.Ins.isComplete == true)
            {
                //coinBonusText.text = coinBonus.ToString();
                coinBonus = LevelGate.Ins.coinBonus;
            }
            else
            {
                coinBonus = 0;
            }
            coinCollect = PlayerHub.Ins.coinCollect;
            coinTotal = (coinBonus + PlayerHub.Ins.coinCollect);
        }).OnComplete(() =>
        {
            ShowCoin();
        });
    }

    public void ShowCoin()
    {
        PlayChangeGoldEffect(coinTotalText);
        coinBonusText.text = coinBonus.ToString();
        coinCollectText.text = coinCollect.ToString();
        coinTotalText.text = coinTotal.ToString();
    }

    public void OnClickHomeButton()
    {
        OnHome();
        LevelGate.Ins.isComplete = false;

        DynamicDataManager.Ins.CurNumCoin += coinTotal;
    }


    public void OnClickWatchAdsButton()
    {
        OnClaimX2();
    }

    void OnHome()
    {
        DynamicDataManager.Ins.CurNumCoin += coinTotal;
        CanvasManager.Ins.OpenUI(UIName.LoadingUI, null);
        CanvasManager.Ins.OpenUI(UIName.SelectLevelUI, null);
        GamePlayController.Ins.ResetGamePlay();
        Close();
    }

    //public void OnClickReplayButton()
    //{
    //    OnReplay();
    //    LevelGate.Ins.isComplete = false;
    //}

    //public void OnClickNextButton()
    //{
    //    OnNext();
    //    LevelGate.Ins.isComplete = false;
    //}
    //void OnReplay()
    //{
    //    CanvasManager.Ins.OpenUI(UIName.LoadingUI, null);
    //    CanvasManager.Ins.OpenUI(UIName.GameplayUI, null);
    //    Close();

    //    GamePlayController.Ins.Replay();
    //}

    //void OnNext()
    //{
    //    CanvasManager.Ins.OpenUI(UIName.LoadingUI, null);
    //    CanvasManager.Ins.OpenUI(UIName.GameplayUI, null);
    //    Close();

    //    GamePlayController.Ins.NextLevel();
    //}

    void OnClaimX2()
    {
        coinTotal = coinTotal * 2;
        coinTotalText.text = coinTotal.ToString();
        watchAdsButton.gameObject.SetActive(false);
        PlayChangeGoldEffect(coinTotalText);
        
    }

    IEnumerator IEDelayBackHomeScene()
    {
        yield return new WaitForSeconds(3.5f);
        homeButton.gameObject.SetActive(true);
        homeButton.transform.DOScale(new Vector3(1f, 1f, 1f), 1f);
    }

    public void PlayChangeGoldEffect(Text txtGold, System.Action callback = null)
    {
        IEnumerator IPlayChangeGoldEffect()
        {
            var goldAfter = coinTotal;
            var goldBefore = coinDefault;
            bool increase = goldAfter > goldBefore;
            float goldBf = goldBefore;
            var distance = increase ? goldAfter - goldBefore : goldBefore - goldAfter;
            var perFrame = distance * Time.deltaTime / 2f;
            while (increase ? goldBf < goldAfter : goldAfter < goldBf)
            {
                if (increase)
                    goldBf += perFrame;
                else
                    goldBf -= perFrame;
                int goldShow = (int)goldBf;
                txtGold.text = goldShow.ToString();
                yield return null;
            }
            txtGold.text = goldAfter.ToString();
            callback?.Invoke();
        }
        StartCoroutine(IPlayChangeGoldEffect());
    }
}
