using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeathSound : MonoBehaviour
{
    private HealthSystem myHealthSystem;

    [FMODUnity.EventRef]
    public string onDeathSound;

    private void OnEnable()
    {

        if (GetComponent<HealthSystem>() != null) myHealthSystem = GetComponent<HealthSystem>();


        if (myHealthSystem != null)
        {
            myHealthSystem.onDeathEvent += LaunchSoundOnDeath;
        }
    }



    private void LaunchSoundOnDeath()
    {
        Debug.Log("Soun death");
        FMODUnity.RuntimeManager.PlayOneShot(onDeathSound, transform.position);
    }
}
