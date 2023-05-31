using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public GameObject sword;

    public void SwordAttack()
    {
        sword.SetActive(true);

        StartCoroutine(IESword());
    }

    IEnumerator IESword()
    {
        yield return new WaitForSeconds(.3f);
        sword.SetActive(false);
    }
}
