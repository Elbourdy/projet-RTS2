using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFeedbacks : MonoBehaviour
{


    [FMODUnity.EventRef]
    public string onAttackSound;

    [FMODUnity.EventRef]
    public string onSelectedSound;


    

    public void LaunchAttackSound ()
    {
        if(onAttackSound != null)
        FMODUnity.RuntimeManager.PlayOneShot(onAttackSound, transform.position);
    }


    public void LaunchSelectedSound()
    {
        if (onSelectedSound != null)
        FMODUnity.RuntimeManager.PlayOneShot(onSelectedSound, transform.position);
    }

    public string GetSoundNameSelection()
    {
        return onSelectedSound;
    }



}
