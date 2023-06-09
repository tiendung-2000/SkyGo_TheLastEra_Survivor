using API.UI;
using UnityEngine;
using UnityEngine.UI;

public class BigMapUI : BaseUIMenu
{
    [SerializeField] Button closeButton;
    [SerializeField] GameObject miniMap;

    public void OnClickCloseButton()
    {
        miniMap.SetActive(true);
        //CameraMovement.Ins.canDrag = false;
        AudioManager.Ins.SoundUIPlay(2);

        Close();
    }

    public void OnEnable()
    {
        CameraMovement.Ins.ResetOrigin();
        CameraMovement.Ins.transform.position = new Vector3(CharacterSelectManager.Ins.activePlayer.gameObject.transform.position.x,
                                            CharacterSelectManager.Ins.activePlayer.gameObject.transform.position.y,
                                            -10);
    }

    #region Desktop
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.M))
    //    {
    //        OnClickCloseButton();
    //    }
    //}
    #endregion
}
