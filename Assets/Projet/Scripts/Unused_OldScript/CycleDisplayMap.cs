using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleDisplayMap : MonoBehaviour
{
    //UNUSED
    //Vieux script inutilisé pour gérer les saisons dans une vieille version du proto 

    public GameObject sun, moon;
    public float startingRotationSun, startingRotationMoon;
    private float sunSpeed, moonSpeed;
    private SunCycleBehavior sCB;

    // Start is called before the first frame update
    void Start()
    {
        sCB = GetComponent<SunCycleBehavior>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float nDaysSeasons = sCB.numberOfDayInASeason;
        float nDaysPasses = sCB.numberOfDaysPassed;

        sunSpeed = (90f / sCB.numberOfDayInASeason) / (sCB.timeOfDay + sCB.timeOfNight);
        moonSpeed = sunSpeed * 4f;

        sun.transform.Rotate(-Vector3.forward, sunSpeed * Time.deltaTime);
        moon.transform.Rotate(-Vector3.forward, moonSpeed * Time.deltaTime);
    }
}
