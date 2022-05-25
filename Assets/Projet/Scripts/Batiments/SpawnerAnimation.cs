using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerAnimation : MonoBehaviour
{
    public Light lightSource;
    public Renderer rD;
    public float minLight, maxLight, durationTrans = 1;
    public Material materialOne, materialOff;
    private bool activated = false, deactivated = true;
    private float count;


    FMOD.Studio.EventInstance spawnerPlay;


    private void Update()
    {
        if(activated)
        {
            lightSource.intensity = Mathf.Lerp(minLight, maxLight, count / durationTrans);
            rD.material.Lerp(materialOne, materialOff, count / durationTrans);
            count += Time.deltaTime;
        }

        if(deactivated)
        {
            lightSource.intensity = Mathf.Lerp(maxLight, minLight, count / durationTrans);
            rD.material.Lerp(materialOff, materialOne, count / durationTrans);
            count += Time.deltaTime;
        }

        if (count > durationTrans)
            deactivated = false;
    }

    public void StartNight()
    {
        spawnerPlay = FMODUnity.RuntimeManager.CreateInstance("event:/Building/Build_Spawner/Build_Spawn_Rise/Build_Spawn_Rise");
        spawnerPlay.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        spawnerPlay.start();
        activated = true;
        count = 0;
    }

    public void StartSpawn()
    {
        spawnerPlay.setParameterByName("SpawnerStop", 1);
    }

    public void EndNight()
    {
        count = 0;
        if (activated)
        {
            activated = false;
            deactivated = true;
        }
            
    }
}
