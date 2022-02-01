using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(HealthSystem))]
public class NexusHitSound : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string SoundHit;




    private void Awake()
    {
        if (SoundHit != null) GetComponent<HealthSystem>().onHealthEvent += LaunchHitSound;
    }

    private void LaunchHitSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(SoundHit, transform.position);
    }

}
