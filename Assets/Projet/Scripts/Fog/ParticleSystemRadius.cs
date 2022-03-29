using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemRadius : MonoBehaviour
{
    [Header("Radius Data")]
    public float radius = 10f;
    public bool activateRadius = true;


    private ParticleSystem mySystem;


    private void Awake()
    {
        mySystem = GetComponent<ParticleSystem>();
        if (activateRadius && mySystem != null)
        {
            ParticleSystem.MainModule main = mySystem.main;
            main.startSizeMultiplier = radius;
        }
    }

}
