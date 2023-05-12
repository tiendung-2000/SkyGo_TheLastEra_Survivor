using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Ins;

    public Transform cameraMovement;

    private void FixedUpdate()
    {
        gameObject.transform.position = new Vector3(CharacterSelectManager.Ins.activePlayer.gameObject.transform.position.x,
                                                    CharacterSelectManager.Ins.activePlayer.gameObject.transform.position.y,
                                                    -10);
    }
}
