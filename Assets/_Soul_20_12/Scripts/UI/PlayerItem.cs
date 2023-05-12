using MagneticScrollView;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerItem : MonoBehaviour
{
    public int id;

    [SerializeField] SelectCharacterUI selectCharacterUI;
    [SerializeField] GameObject scroll;
    [SerializeField] Image image;

    public GameObject locked;
    public bool isUnlock = false;

    public void Select()
    {
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

            if (DynamicDataManager.IsCharacterUnlocked(id) == true)
            {
                //SelectCharacterUI.Ins.PlayerAnimation();

                selectCharacterUI.startButton.gameObject.SetActive(true);
                selectCharacterUI.buyCharacterButton.gameObject.SetActive(false);
            }
            else
            {
                selectCharacterUI.startButton.gameObject.SetActive(false);
                selectCharacterUI.buyCharacterButton.gameObject.SetActive(true);
                selectCharacterUI.characterPriceText.text = ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.priceToUnlock.ToString();
            }
        }
        else
        {
            if (DynamicDataManager.IsCharacterUnlocked(id) == true)
            {
                selectCharacterUI.OnStart();
            }
        }

        selectCharacterUI.PlayerAnimation();
        //selectCharacterUI.OnSwitchPlayer();
    }
}
