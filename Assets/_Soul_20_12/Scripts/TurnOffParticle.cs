using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffParticle : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(IEVFX());
    }

    IEnumerator IEVFX()
    {
        yield return new WaitForSeconds(GetComponent<ParticleSystem>().main.duration);
        GetComponent<ParticleSystem>().Stop();
    }
}
