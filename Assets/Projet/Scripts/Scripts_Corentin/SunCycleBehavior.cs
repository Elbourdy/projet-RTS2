using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunCycleBehavior : MonoBehaviour
{
    public Light light;
    public Gradient colorLightDay, colorLightNight;
    public float timeOfDay, timeOfNight;

    private float totalTimeOfADay, timer;


    // Start is called before the first frame update
    void Start()
    {
        totalTimeOfADay = timeOfDay + timeOfNight;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer/totalTimeOfADay > 1)
        {
            timer = 0;
        }

        timer += Time.deltaTime;

        if (timer < timeOfDay)
        {
            light.color = colorLightDay.Evaluate(timer / timeOfDay);
        }
        else
        {
            light.color = colorLightNight.Evaluate((timer - timeOfDay) / timeOfNight);
        }
    }
}
