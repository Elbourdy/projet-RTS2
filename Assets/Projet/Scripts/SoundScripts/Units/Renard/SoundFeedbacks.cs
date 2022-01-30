using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFeedbacks : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string onAttackSound;

    public void LaunchAttackSound ()
    {
        FMODUnity.RuntimeManager.PlayOneShot(onAttackSound, transform.position);
    }
}
