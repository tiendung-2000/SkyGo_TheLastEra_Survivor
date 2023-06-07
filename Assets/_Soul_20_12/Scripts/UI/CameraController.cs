using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Ins;

    public Transform cameraMovement;
    public Camera mainCamera;

    private void Awake()
    {
        Ins = this;
    }

    private void FixedUpdate()
    {
        gameObject.transform.position = new Vector3(CharacterSelectManager.Ins.activePlayer.gameObject.transform.position.x,
                                                    CharacterSelectManager.Ins.activePlayer.gameObject.transform.position.y,
                                                    -10);
    }
}
