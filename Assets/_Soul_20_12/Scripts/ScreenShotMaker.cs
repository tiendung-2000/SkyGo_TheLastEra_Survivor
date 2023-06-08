//using System;
//using System.IO;
//using UnityEngine;
 
//#if ODIN_INSPECTOR
//using Sirenix.OdinInspector;
//#endif
 
//public class ScreenShotMaker : MonoBehaviour
//{
//    public GameObject target;
//    public int resolution = 160;
//    public float cameraDistance = -2.0f;
//    public CameraClearFlags backgroundType = CameraClearFlags.SolidColor;
//    public Color backgroundColor = new Color(0.1921569f, 0.3019608f, 0.4745098f, 0f);
//    public bool useUTCDateTime;
 
//    private RenderTexture renderTexture;
//    private Camera renderCamera;
//    private Vector4 bounds;
 
//    private void Start()
//    {
//        // If you want to take screenshot when the scene starts, uncomment the next line.
//        TakeScreenshot();
//    }
 
//#if ODIN_INSPECTOR
//    [Button]
//#endif
//    public void TakeScreenshot()
//    {
//        ProcessScreenshot();
//    }
 
//    private void ProcessScreenshot()
//    {
//        Debug.Log("Initializing camera and stuff...");
 
//        if (!GetComponent<Camera>())
//            gameObject.AddComponent<Camera>();
 
//        renderCamera = GetComponent<Camera>();
//        renderCamera.enabled = true;
//        renderCamera.cameraType = CameraType.Game;
//        renderCamera.forceIntoRenderTexture = true;
//        renderCamera.orthographic = true;
//        renderCamera.orthographicSize = 5;
//        renderCamera.aspect = 1.0f;
//        renderCamera.targetDisplay = 2;
//        renderCamera.clearFlags = backgroundType;
//        renderCamera.backgroundColor = backgroundColor;
 
//        renderTexture = new RenderTexture(resolution, resolution, 24);
 
//        renderCamera.targetTexture = renderTexture;
 
//        bounds = new Vector4();
 
//        Debug.Log("Initialized successfully!");
//        Debug.Log("Computing level boundaries...");
 
//        if (target != null)
//        {
//            Bounds boundaries;
 
//            if (target.GetComponentInChildren<Renderer>() != null)
//            {
//                boundaries = target.GetComponentInChildren<Renderer>().bounds;
//            }
//            else if(target.GetComponentInChildren<Collider2D>() != null)
//            {
//                boundaries = target.GetComponentInChildren<Collider2D>().bounds;
//            }
//            else
//            {
//                Debug.Log("Unfortunately no boundaries could be found :/");
//                return;
//            }
     
//            bounds.w = boundaries.min.x;
//            bounds.x = boundaries.max.x;
//            bounds.y = boundaries.min.y;
//            bounds.z = boundaries.max.y;
//        }
//        else
//        {
//            object[] gameObjects = FindObjectsOfType(typeof(GameObject));
 
//            foreach (object gameObj in gameObjects)
//            {
//                GameObject levelElement = (GameObject)gameObj;
//                Bounds boundaries = new Bounds();
 
//                if (levelElement.GetComponentInChildren<Renderer>() != null)
//                {
//                    boundaries = levelElement.GetComponentInChildren<Renderer>().bounds;
//                }
//                else if (levelElement.GetComponentInChildren<Collider2D>() != null)
//                {
//                    boundaries = levelElement.GetComponentInChildren<Collider2D>().bounds;
//                }
//                else
//                {
//                    continue;
//                }
 
//                if (boundaries != null)
//                {
//                    bounds.w = Mathf.Min(bounds.w, boundaries.min.x);
//                    bounds.x = Mathf.Max(bounds.x, boundaries.max.x);
//                    bounds.y = Mathf.Min(bounds.y, boundaries.min.y);
//                    bounds.z = Mathf.Max(bounds.z, boundaries.max.y);
//                }
//            }
//        }
 
//        Debug.Log("Boundaries computed successfuly! The computed boundaries are " + bounds);
//        Debug.Log("Computing target image resolution and final setup...");
 
//        int xRes = Mathf.RoundToInt(resolution * ((bounds.x - bounds.w) / (renderCamera.aspect * renderCamera.orthographicSize * 2 * renderCamera.aspect)));
//        int yRes = Mathf.RoundToInt(resolution * ((bounds.z - bounds.y) / (renderCamera.aspect * renderCamera.orthographicSize * 2 / renderCamera.aspect)));
 
//        Texture2D virtualPhoto = new Texture2D(xRes, yRes, TextureFormat.RGB24, false);
//        RenderTexture.active = renderTexture;
 
//        Debug.Log("Success! Everything seems ready to render!");
 
//        for (float i = bounds.w, xPos = 0; i < bounds.x; i += renderCamera.aspect * renderCamera.orthographicSize * 2, xPos++)
//        {
//            for (float j = bounds.y, yPos = 0; j < bounds.z; j += renderCamera.aspect * renderCamera.orthographicSize * 2, yPos++)
//            {
//                gameObject.transform.position = new Vector3(i + renderCamera.aspect * renderCamera.orthographicSize, j + renderCamera.aspect * renderCamera.orthographicSize, cameraDistance);
 
//                renderCamera.Render();
 
//                virtualPhoto.ReadPixels(new Rect(0, 0, resolution, resolution), (int)xPos * resolution, (int)yPos * resolution);
 
//                Debug.Log("Rendered and copied chunk " + (xPos + 1) + ":" + (yPos + 1));
//            }
//        }
 
//        Debug.Log("All chunks rendered! Some final adjustments and picture should be saved!");
 
//        RenderTexture.active = null;
//        renderCamera.targetTexture = null;
//        renderCamera.enabled = false;
//        transform.position = Vector3.zero;
 
//        byte[] bytes = virtualPhoto.EncodeToPNG();
 
//        var dateTimeNow = useUTCDateTime ? DateTime.UtcNow : DateTime.Now;
     
//        File.WriteAllBytes($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}" +
//                           $"{Path.DirectorySeparatorChar}ScreenShotTaken_{dateTimeNow.ToString("yyyy-MM-dd-HH-mm-ss")}.png", bytes);
 
//        Debug.Log("All done! Always happy to help you :)");
//    }
//}