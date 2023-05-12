using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Ins;

    public bool canDrag = false;

    [SerializeField] float speed = 2f;

    [SerializeField]
    private Camera cam;
    private Vector3 dragOrigin;


    [SerializeField]

    private float mapMinX, mapMaxX, mapMinY, mapMaxY;

    private void Awake()
    {
        Ins = this;
    }
    private void FixedUpdate()
    {
        if (canDrag == true)
        {
            PanCamera();
        }
    }
    private void OnDisable()
    {
        canDrag = false;
    }

    

    public void ResetOrigin()
    {
        dragOrigin = new Vector3(CharacterSelectManager.Ins.activePlayer.gameObject.transform.position.x,
                                                    CharacterSelectManager.Ins.activePlayer.gameObject.transform.position.y,
                                                    -10);
    }

    private void PanCamera()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            //Debug.Log(cam.ScreenToWorldPoint(Input.mousePosition));
            Vector3 difference = new Vector3(dragOrigin.x * speed, dragOrigin.y * speed, dragOrigin.z)
               - new Vector3(cam.ScreenToWorldPoint(Input.mousePosition).x * speed,
                           cam.ScreenToWorldPoint(Input.mousePosition).y * speed,
                           cam.ScreenToWorldPoint(Input.mousePosition).z);
            //cam.transform.position += difference;

            cam.transform.position = ClampCamera(cam.transform.position + difference);
        }
    }

    private Vector3 ClampCamera(Vector3 targetPos)
    {
        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;

        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;
        float minY = mapMinY + camHeight;
        float maxY = mapMaxY - camHeight;

        float newX = Mathf.Clamp(targetPos.x, minX, maxX);
        float newY = Mathf.Clamp(targetPos.y, minY, maxY);

        return new Vector3(newX, newY, targetPos.z);
    }
}
