using API.UI;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool closeWhenEntered /* , openWhenEnemiesCleared */;

    public GameObject[] doors;

    [HideInInspector]
    public bool roomActive;

    //public GameObject mapHider;

    //void Start()
    //{
    //    mapHider.SetActive(false);
    //}

    public void OpenDoors()
    {
        foreach (GameObject door in doors)
        {
            door.SetActive(false);

            closeWhenEntered = false;
        }
        AudioManager.Ins.SoundEffect(2);
        CanvasManager.Ins.OpenUI(UIName.ClearRoomPopup, null);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //CameraController.instance.ChangeTarget(transform);

            if (closeWhenEntered)
            {
                foreach (GameObject door in doors)
                {
                    door.SetActive(true);
                }

                AudioManager.Ins.SoundEffect(2);
            }
            roomActive = true;

            //mapHider.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            roomActive = false;
        }
    }
}
