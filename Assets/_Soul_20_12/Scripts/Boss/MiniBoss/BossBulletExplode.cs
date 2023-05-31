using DG.Tweening;
using UnityEngine;

public class BossBulletExplode : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        DOVirtual.DelayedCall(1, () =>
        {
            SmartPool.Ins.Despawn(gameObject);
        });

        //AudioManager.instance.PlaySFX(4);
        AudioManager.Ins.SoundEffect(8);

        if (other.tag == "Player")
        {
            DataManager.Ins.DamagePlayer();
        }
    }
}
