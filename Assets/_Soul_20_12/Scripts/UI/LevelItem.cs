using MagneticScrollView;
using UnityEngine;
using UnityEngine.UI;

public class LevelItem : MonoBehaviour
{
    public int id;

    [SerializeField] SelectLevelUI selectLevelUI;
    [SerializeField] GameObject scroll;
    [SerializeField] Image image;

    public GameObject locked;
    public bool isUnlock = false;

    public void Select()
    {
        AudioManager.Ins.SoundUIPlay(2);

        MagneticScrollRect scrollRect = scroll.GetComponent<MagneticScrollRect>();

        if (scroll.transform.childCount != 0)
        {
            scrollRect.ResetScroll();
            scrollRect.StartAutoArranging();
        }

        if (scrollRect.m_currentSelected != id/* && isUnlock == false*/)
        {
            scrollRect.m_currentSelected = id;
            scrollRect.StartAutoArranging();

            scrollRect.StartScrolling();

            DynamicDataManager.Ins.CurLevel = id;

            SelectLevelUI.Ins.levelName.text = ResourceSystem.Ins.levels[DynamicDataManager.Ins.CurLevel].levelName.ToString();

            if (DynamicDataManager.IsLevelUnlocked(id) == true)
            {
                selectLevelUI.selectLevelButton.gameObject.SetActive(true);
                selectLevelUI.unlockLevelButton.gameObject.SetActive(false);
                selectLevelUI.watchAdsToTestButton.gameObject.SetActive(false);

            }
            else
            {
                selectLevelUI.selectLevelButton.gameObject.SetActive(false);
                selectLevelUI.unlockLevelButton.gameObject.SetActive(true);
                selectLevelUI.watchAdsToTestButton.gameObject.SetActive(true);
                selectLevelUI.levelUnlockPrice.text = ResourceSystem.Ins.levels[DynamicDataManager.Ins.CurLevel].priceToUnlock.ToString();
            }
        }
        else
        {
            if (DynamicDataManager.IsLevelUnlocked(id) == true)
            {
                selectLevelUI.OnSelectLevel();
            }
        }
    }
}
