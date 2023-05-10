using UnityEngine;

public class ShieldBuff : MonoBehaviour
{
    public static ShieldBuff Ins;

    public Collider2D col;

    public bool hasShield = false;

    private void Awake()
    {
        Ins = this;
    }

    private void Start()
    {
        col.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 11)
        {
            //Debug.Log(PlayerController.Ins.shieldBuffFX.isPlaying);
            PlayerController.Ins.shieldBuffFX.gameObject.SetActive(false);
            PlayerController.Ins.shiedBreakFX.gameObject.SetActive(true);
            
            PlayerController.Ins.shiedBreakFX.Play(true);
            PlayerController.Ins.StopAllCoroutines();
            col.enabled = false;
            hasShield = false;
        }
    }
}
