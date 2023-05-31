using API.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : BaseUIMenu
{
    [SerializeField] Button backButton;
    [SerializeField] Transform tabsContent;
    //[SerializeField] GameObject scrollParent;
    [SerializeField] ShopTabs shopTabs;
    [SerializeField] int currentIndex;
    [SerializeField] GameObject shop;
    [SerializeField] Image bg;

    private void Start()
    {
        backButton.onClick.AddListener(OnClickBackButton);
        SetUpShop();
    }

    private void OnEnable()
    {
        CanvasManager.Ins.OpenUI(UIName.CoinBar, null);
        bg.DOFade(.5f, .5f);
        shop.transform.DOScale(new Vector3(1f, 1f, 1f), .5f);

    }
    private void OnDisable()
    {

    }

    private void SetUpShop()
    {
        var ShopTabData = ResourceSystem.Ins.ShopData;
        var index = 0;
        foreach (var item in ShopTabData.shopData)
        {
            var tab = Instantiate(shopTabs, tabsContent);
            tab.index = index;
            tab.SetUp(item.name, item.value.ToString(), item.image, item.price.ToString());
            index++;
        }
    }

    //public void OnClickTabButton(int tabID)
    //{
    //    //int index = 
    //    for (int i = 0; i <= scrollParent.Length - 1; i++)
    //    {
    //        if (i == tabID)
    //        {
    //            scrollParent[i].gameObject.SetActive(true);
    //        }
    //        else
    //        {
    //            scrollParent[i].gameObject.SetActive(false);
    //        }
    //    }

    //}

    public void OnClickBackButton()
    {
        StartCoroutine(FadeOutAndScaleDown());
    }
    private IEnumerator FadeOutAndScaleDown()
    {
        AudioManager.Ins.SoundUIPlay(2);
        yield return null; // Wait for one frame to allow all objects to be properly initialized

        if (bg != null) bg.DOFade(0f, .5f);
        if (shop != null) shop.transform.DOScale(new Vector3(0, 0, 0), .5f);

        yield return new WaitForSeconds(.5f); // Wait for the animation to finish

        Close();
    }
}
