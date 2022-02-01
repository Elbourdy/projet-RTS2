using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(HealthSystem))]
public class NexusHitSound : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string SoundHit;

    private HealthSystem myHeathSystem;


    private void Awake()
    {
        myHeathSystem = GetComponent<HealthSystem>();
        if (SoundHit != null) myHeathSystem.onHealthEvent += LaunchHitSound;
    }

    private void LaunchHitSound()
    {
        if (myHeathSystem.GetCurrentDamage() < 0)
        FMODUnity.RuntimeManager.PlayOneShot(SoundHit, transform.position);
    }

}
