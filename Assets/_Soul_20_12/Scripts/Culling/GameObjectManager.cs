using UnityEngine;

public class GameObjectManager : MonoBehaviour
{
    public Camera castingCamera;
    public DeactivateOrActivateGameObject[] deactivateOrActivateGameObject;

    void OnDrawGizmos()
    {
        foreach (DeactivateOrActivateGameObject d in deactivateOrActivateGameObject)
        {
            if (d.deactivateOrActivate)
            {
                d.title = d.deactivateOrActivate.name;

                if (d.showBorders)
                {
                    Gizmos.color = d.DrawColor;
                    Gizmos.DrawLine(d.TopRight, d.TopLeft);
                    Gizmos.DrawLine(d.TopLeft, d.BottomLeft);
                    Gizmos.DrawLine(d.BottomLeft, d.BottomRight);
                    Gizmos.DrawLine(d.BottomRight, d.TopRight);
                }
            }
        }
    }

    void Update()
    {
        //Debug.Log(gameObject.GetComponent<Renderer>().isVisible);
        //Debug.Log(gameObject.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().isVisible);
        //gameObject.SetActive(gameObject.GetComponent<Renderer>().isVisible);

        foreach (DeactivateOrActivateGameObject d in deactivateOrActivateGameObject)
        {
            if (d.deactivateOrActivate)
            {
                float cameraHalfWidth = castingCamera.orthographicSize * ((float)Screen.width / (float)Screen.height);
                d.deactivateOrActivate.SetActive(IsObjectVisibleOnCastingCamera(d.center, d.size, d.size.x > castingCamera.orthographicSize + cameraHalfWidth | d.size.y > castingCamera.orthographicSize));
                //d.deactivateOrActivate.SetActive(IsObjectVisibleOnCastingCamera(d.center, d.size, true));
                //Debug.Log(IsObjectVisibleOnCastingCamera(d.center, d.size, true));
            }
        }
    }

    bool IsObjectVisibleOnCastingCamera(Vector2 checkCenter, Vector2 checkSize, bool isObjectSizeBiggerThanCamera)
    {
        float cameraHalfWidth = castingCamera.orthographicSize * ((float)Screen.width / (float)Screen.height);
        //Vector2 halfObjSize = checkSize / 2f;

        if (!isObjectSizeBiggerThanCamera)
            return checkCenter.x + checkSize.x > castingCamera.transform.position.x - cameraHalfWidth & checkCenter.x - checkSize.x < castingCamera.transform.position.x + cameraHalfWidth & checkCenter.y + checkSize.y > castingCamera.transform.position.y - castingCamera.orthographicSize & checkCenter.y - checkSize.y < castingCamera.transform.position.y + castingCamera.orthographicSize;
        else
            return castingCamera.transform.position.x - cameraHalfWidth < checkCenter.x + checkSize.x & castingCamera.transform.position.x + cameraHalfWidth > checkCenter.x - checkSize.x & castingCamera.transform.position.y - castingCamera.orthographicSize < checkCenter.y + checkSize.y & castingCamera.transform.position.y + castingCamera.orthographicSize > checkCenter.y - checkSize.y;
    }




    // these are not used
    /*private void OnBecameVisible() // this works for both Sprite Renderer and Sprite Shape Renderer
    {
        //enabled = true;
        //gameObject.SetActive(true);
        //Debug.Log("true");
    }

    private void OnBecameInvisible()
    {
        //enabled = false;
        //gameObject.SetActive(false);
        //Debug.Log("false");
    }*/
    //

}