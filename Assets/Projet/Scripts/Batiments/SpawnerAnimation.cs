using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerAnimation : MonoBehaviour
{
    public Animator animator;

    FMOD.Studio.EventInstance spawnerPlay;

    public void StartNight()
    {
        animator.SetBool("Charging", true);
        spawnerPlay = FMODUnity.RuntimeManager.CreateInstance("event:/Building/Build_Spawner/Build_Spawn_Rise/Build_Spawn_Rise");
        spawnerPlay.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        spawnerPlay.start();
    }

    public void StartSpawn()
    {
        spawnerPlay.setParameterByName("SpawnerStop", 0);
    }

    public void EndNight()
    {
        animator.SetBool("Charging", false);
    }
}
