using MagneticScrollView;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragCheck : MonoBehaviour/*, IBeginDragHandler, IDragHandler, IEndDragHandler*/
{
    [SerializeField] MagneticScrollRect scroll;

    [SerializeField] bool isLevel;

    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    CheckLevel();
    //}

    //public void OnDrag(PointerEventData eventData)
    //{
    //    CheckLevel();
    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    CheckLevel();
    //}

    public void CheckLevel()
    {
        if (isLevel == true)
        {
            DynamicDataManager.Ins.CurLevel = scroll.GetComponent<MagneticScrollRect>().m_currentSelected;

            SelectLevelUI.Ins.levelName.text = ResourceSystem.Ins.levels[DynamicDataManager.Ins.CurLevel].levelName.ToString();
            SelectLevelUI.Ins.levelUnlockPrice.text = ResourceSystem.Ins.levels[DynamicDataManager.Ins.CurLevel].priceToUnlock.ToString();
            SelectLevelUI.Ins.CheckLevelUnlocked();
        }
        else
        {
            DynamicDataManager.Ins.CurPlayer = scroll.GetComponent<MagneticScrollRect>().m_currentSelected;

            SelectCharacterUI.Ins.characterPriceText.text = ResourceSystem.Ins.CharactersDatabase.Characters[DynamicDataManager.Ins.CurPlayer].Data.priceToUnlock.ToString();
            SelectCharacterUI.Ins.CheckPlayerUnlocked();
            SelectCharacterUI.Ins.OnSwitchPlayer();
            SelectCharacterUI.Ins.PlayerAnimation();
        }
    }
}
