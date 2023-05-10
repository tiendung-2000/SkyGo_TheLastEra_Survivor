using UnityEngine;
using DG.Tweening;

public class CharacterSelector : MonoBehaviour
{
    private bool canSelect;

    public GameObject playerToSpawn;
    //public GameObject message;


    //public bool shouldUnlock;

    //void Start()
    //{
    //    //if (shouldUnlock)
    //    //{
    //    //    if (PlayerPrefs.HasKey(playerToSpawn.name))
    //    //    {
    //    //        if (PlayerPrefs.GetInt(playerToSpawn.name) == 1)
    //    //        {
    //    //            gameObject.SetActive(true);
    //    //        }
    //    //        else
    //    //        {
    //    //            gameObject.SetActive(false);
    //    //        }
    //    //    }
    //    //    else
    //    //    {
    //    //        gameObject.SetActive(false);
    //    //    }
    //    //}
    //}

    void Update()
    {
        if (canSelect)
        {
            //Debug.Log("vao day");

            //CharacterSelectManager.Ins.cooldown = true;
            
            Sequence selectCharacter = DOTween.Sequence();
            selectCharacter?.Kill();
            selectCharacter.AppendCallback(() =>
            {
                Vector3 playerPos = PlayerController.Ins.transform.position;

                PlayerController.Ins.gameObject.SetActive(false);

                playerToSpawn.SetActive(true);
                playerToSpawn.transform.position = playerPos;
                PlayerController newPlayer = playerToSpawn.GetComponent<PlayerController>();
                PlayerController.Ins = newPlayer;

                CharacterSelectManager.Ins.activePlayer = newPlayer;
                //CharacterSelectManager.Ins.activeCharSelect.gameObject.SetActive(true);
                //CharacterSelectManager.Ins.activeCharSelect = this;

            }).OnComplete(() =>
            {
                PlayerSkillManager.instance.player = CharacterSelectManager.Ins.activePlayer;
                gameObject.SetActive(false);
                return;
            });
        }
    }
}
