using API.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClearRoomPopup : BaseUIMenu
{
    //[SerializeField] GameObject clear;
    //[SerializeField] Image clearImg;

    //private void OnEnable()
    //{
    //    clearImg.DOFillAmount(1f, .3f).SetEase(Ease.Linear);

    //    clear.transform.DOScale(1.5f, 0.3f).OnComplete(() =>
    //    {
    //        clear.transform.DOScale(1f, 0.3f).OnComplete(() =>
    //        {
    //            StartCoroutine(IEDisable());
    //        });

    //    });
    //}

    //private void OnDisable()
    //{
    //    clearImg.fillAmount = 0f;
    //    clear.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
    //}

    private void OnEnable()
    {
        StartCoroutine(IEDisable());
    }

    IEnumerator IEDisable()
    {
        yield return new WaitForSeconds(2f);
        Close();
    }
}
