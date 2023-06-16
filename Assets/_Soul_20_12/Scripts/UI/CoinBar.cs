using API.UI;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinBar : BaseUIMenu
{
    public static CoinBar Ins;
    public TMP_Text coinText;
    public int currentCoin;
    public TextScroll textScroll;
    [SerializeField] Button addCoinButton;

    private void Awake()
    {
        Ins = this;
    }

    void OnEnable()
    {
        addCoinButton.onClick.AddListener(OnClickAddCoinButton);

        OnCoinChange(DynamicDataManager.Ins.CurNumCoin);
        DynamicDataManager.Ins.OnCoinNumChange += OnCoinChange;
        PlayChangeGoldEffect(coinText);
    }
    public void OnCoinChange(int num)//value
    {
        if (this.gameObject.activeSelf)
        {
        PlayChangeGoldEffect(coinText);
        }
        currentCoin = DynamicDataManager.Ins.CurNumCoin;
        //coinText.text = num.ToString();
    }

    public void OnClickAddCoinButton()
    {
        OnAddCoin();
    }

    public void OnAddCoin()
    {
        AudioManager.Ins.SoundUIPlay(2);

        CanvasManager.Ins.OpenUI(UIName.ShopUI, null);
    }

    public void PlayChangeGoldEffect(TMP_Text txtGold, System.Action callback = null)
    {
        IEnumerator IPlayChangeGoldEffect()
        {
            var gold = DynamicDataManager.Ins.CurNumCoin;
            var goldBefore = currentCoin;
            bool increase = gold > goldBefore;
            float goldBf = goldBefore;
            var distance = increase ? gold - goldBefore : goldBefore - gold;
            var perFrame = distance * Time.deltaTime / .5f;
            while (increase ? goldBf < gold : gold < goldBf)
            {
                if (increase)
                    goldBf += perFrame;
                else
                    goldBf -= perFrame;
                int goldShow = (int)goldBf;
                txtGold.text = goldShow.ToString();
                yield return null;
            }
            txtGold.text = gold.ToString();
            callback?.Invoke();
        }
        StartCoroutine(IPlayChangeGoldEffect());

        textScroll.OnInit();
    }
}
