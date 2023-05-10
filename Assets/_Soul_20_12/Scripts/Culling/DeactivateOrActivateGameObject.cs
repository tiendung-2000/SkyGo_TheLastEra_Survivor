using UnityEngine;

[System.Serializable]
public class DeactivateOrActivateGameObject
{
    public string title;
    public Vector2 ChangeSize = Vector2.one;
    public Vector2 Offset = Vector2.zero;
    public GameObject deactivateOrActivate;
    public bool multiplySizeWithTransformScale = true;
    public Vector2 size { get { return ChangeSize * (multiplySizeWithTransformScale ? new Vector2(Mathf.Abs(deactivateOrActivate.transform.localScale.x), Mathf.Abs(deactivateOrActivate.transform.localScale.y)) : Vector2.one); } }
    public Vector2 center { get { return (Vector2)deactivateOrActivate.transform.position + Offset; } }

    public Vector2 TopRight { get { return new Vector2(center.x + size.x, center.y + size.y); } }
    public Vector2 TopLeft { get { return new Vector2(center.x - size.x, center.y + size.y); } }
    public Vector2 BottomLeft { get { return new Vector2(center.x - size.x, center.y - size.y); } }
    public Vector2 BottomRight { get { return new Vector2(center.x + size.x, center.y - size.y); } }

    public Vector2 min { get { return BottomLeft; } }
    public Vector2 max { get { return TopRight; } }

    public Color DrawColor = Color.white;

    public bool showBorders = true;
}