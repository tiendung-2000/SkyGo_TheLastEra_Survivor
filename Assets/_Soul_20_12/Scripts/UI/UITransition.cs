using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UITransition : Singleton<UITransition>
{
    [SerializeField] Image blackPanel;

    [SerializeField] float timeFade;

    Tween currentTween;
    private void Start()
    {
        blackPanel.gameObject.SetActive(false);
    }
    public void ShowTransition(UnityAction callback)
    {
        currentTween?.Kill();
        blackPanel.gameObject.SetActive(true);
        blackPanel.color = new Color(0, 0, 0, 0);
        currentTween = blackPanel.DOFade(1, timeFade).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            callback?.Invoke();
            currentTween = blackPanel.DOFade(0, timeFade).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                blackPanel.gameObject.SetActive(false);
            });
        });
    }
}
