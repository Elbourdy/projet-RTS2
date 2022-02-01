using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCreateSound : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string onCreateSound;


    public void LaunchSoundCreation ()
    {
        FMODUnity.RuntimeManager.PlayOneShot(onCreateSound, transform.position);
    }
}
