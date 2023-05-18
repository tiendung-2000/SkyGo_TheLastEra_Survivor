using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
    public static CooldownUI instance;

    private Weapon gun;

    public CanvasGroup canvasGR;

    public Image fill;

    public bool isCooldown = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        canvasGR = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        canvasGR = GetComponent<CanvasGroup>();
        canvasGR.alpha = 0f;
    }

    private void OnEnable()
    {
        canvasGR = GetComponent<CanvasGroup>();
        canvasGR.alpha = 0f;
    }

    public void DoFade()
    {
        canvasGR.alpha = 0.0f;
    }

    public void DoShow()
    {
        canvasGR.alpha = 1.0f;
    }
}
