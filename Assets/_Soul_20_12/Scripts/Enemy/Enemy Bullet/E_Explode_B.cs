using UnityEngine;

public class E_Explode_B : MonoBehaviour
{
    [SerializeField] Collider2D col;
    [SerializeField] ParticleSystem particle;

    private void OnEnable()
    {
        CinemachineShake.Instance.ShakeCamera(3f, .5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //AudioManager.instance.PlaySFX(4);

        if (other.tag == "Player")
        {
            DataManager.Ins.DamagePlayer();
            col.enabled = false;
        }

    }

    private void OnBecameInvisible()
    {
        col.enabled = false;
        Destroy(gameObject);
    }
}
