using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EnemyBomb : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void OnEnable()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(WaitForSecond());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            DataManager.Ins.DamagePlayer();
            SmartPool.Ins.Despawn(gameObject);
        }
    }

    IEnumerator WaitForSecond()
    {
        yield return new WaitForSeconds(1.4f);
        anim.enabled = true;
        DOVirtual.DelayedCall(1, () =>
        {
            SmartPool.Ins.Despawn(gameObject);
        });
    }
}
