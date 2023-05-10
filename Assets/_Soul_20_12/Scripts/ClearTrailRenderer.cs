using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearTrailRenderer : MonoBehaviour
{
    public TrailRenderer[] trail;

    private void OnDisable()
    {
        for (int i = 0; i < trail.Length; i++)
        {
            trail[i].Clear();
        }
    }
}
