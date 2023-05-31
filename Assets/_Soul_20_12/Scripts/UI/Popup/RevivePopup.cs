using API.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RevivePopup : BaseUIMenu
{
    [SerializeField] Button reviveButton;
    [SerializeField] Button noThanksButton;

    [SerializeField] float timeCountDown;

    [SerializeField] Text countDownText;

    [SerializeField] Text titleText;

    [SerializeField] GameObject counter;

    [SerializeField] float timeDelayShowNoThanks;

    Tween OnEnableTween;

    private void Start()
    {
        reviveButton.onClick.AddListener(OnClickReviveButton);
        noThanksButton.onClick.AddListener(OnClickNoThanksButton);
    }

    private void OnEnable()
    {
        AudioManager.Ins.PlayWinLoseSound(1);
    }

    public override void Init(object[] initParams)
    {
        base.Init(initParams);
        OnEnableTween?.Kill();
        if (LevelManager.Ins.reviveCount < 1)
        {
            OnEnableTween = DOVirtual.Float(timeCountDown, 0, timeCountDown, (value) =>
            {
                countDownText.text = Mathf.Round(value).ToString();
            }).SetEase(Ease.Linear).OnComplete(() =>
            {
                LoseGame();
            });

            noThanksButton.transform.localScale = Vector3.zero;

            StartCoroutine(IEShowNoThanksButton());

        }
        else
        {
            LoseGame();
        }
    }

    IEnumerator IEShowNoThanksButton()
    {
        yield return new WaitForSeconds(timeDelayShowNoThanks);
        noThanksButton.transform.DOScale(Vector3.one, 0.5f);
    }

    public void OnClickWatchVideo()
    {
        //Ads
        OnReviveGame();
    }

    void LoseGame()
    {
        Close();
        CanvasManager.Ins.OpenUI(UIName.CompleteUI, null);
    }

    void OnClickReviveButton()
    {
        OnReviveGame();
        LevelManager.Ins.reviveCount++;
    }

    void OnReviveGame()
    {
        OnEnableTween?.Kill();
        Close();
        GamePlayController.Ins.OnRecoverGame();
    }

    void OnClickNoThanksButton()
    {
        OnEnableTween?.Kill();
        LoseGame();
        CinemachineShake.Instance.ResetShakeCamera();
    }



    //IEnumerator IETimeCounter()
    //{
    //    while(timeCountDown >= 0)
    //    {
    //        timeCountDown -= Time.deltaTime;
    //        yield return null;
    //    }
    //}

    //void OnEnable()
    //{
    //    isNo = true;

    //    if (LevelManager.Ins.reviveCount < 1)
    //    {
    //        counter.SetActive(true);
    //        noButton.gameObject.SetActive(true);
    //        //exitButton.gameObject.SetActive(false);

    //        CanvasManager.Ins.CloseUI(UIName.GameplayUI);

    //        OnEnableTween?.Kill();

    //        OnEnableTween = DOVirtual.Float(timeCountDown, 0, timeCountDown, (value) =>
    //        {
    //            countDownText.text = Mathf.Round(value).ToString();
    //        }).SetEase(Ease.Linear).OnComplete(() =>
    //        {
    //            //exitButton.gameObject.SetActive(true);
    //            noButton.gameObject.SetActive(false);
    //            reviveButton.gameObject.SetActive(false);

    //            if (isNo == true)
    //            {
    //                StartCoroutine(IEShowComplete());
    //            }
    //        });
    //    }
    //    else
    //    {
    //        titleText.text = "You Lose!";

    //        //exitButton.gameObject.SetActive(true);
    //        noButton.gameObject.SetActive(false);
    //        reviveButton.gameObject.SetActive(false);
    //        counter.SetActive(false);

    //        StartCoroutine(IEShowComplete());
    //    }
    //}
    //void OnClickExitButton()
    //{
    //    OnNo();
    //    CinemachineShake.Instance.ResetShakeCamera();
    //}

    //void OnNo()
    //{
    //    isNo = false;

    //    SmartPool.Ins.Despawn(ResourceSystem.Ins.CurLevelGameObj);
    //    ResourceSystem.Ins.players[DynamicDataManager.Ins.CurPlayer].transform.position = Vector3.zero;
    //    CanvasManager.Ins.OpenUI(UIName.SelectLevelUI, null);

    //    Close();
    //}

    //IEnumerator IENoThanksButtonClick()
    //{
    //    isNo = false;
    //    yield return new WaitForSeconds(.5f);
    //    //SmartPool.Ins.Despawn(ResourceSystem.Ins.CurLevelGameObj);
    //    //ResourceSystem.Ins.players[DynamicDataManager.Ins.CurPlayer].transform.position = Vector3.zero;
    //    StartCoroutine(IEShowComplete());
    //}
    //IEnumerator IEShowComplete()
    //{
    //    yield return new WaitForSeconds(.5f);
    //    CanvasManager.Ins.OpenUI(UIName.CompleteUI, null);
    //    Close();
    //}

    //void ShowComplete()
    //{
    //    OnEnableTween?.Kill();
    //    OnEnableTween = DOVirtual.DelayedCall(1f, () =>
    //    {

    //    });
    //}
}
