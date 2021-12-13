using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleDisplayMap : MonoBehaviour
{
    public GameObject sun, moon;
    public float startingRotationSun, startingRotationMoon;
    private SunCycleBehavior sCB;

    // Start is called before the first frame update
    void Start()
    {
        sCB = GetComponent<SunCycleBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        float nDaysSeasons = sCB.numberOfDayInASeason;
        float nDaysPasses = sCB.numberOfDaysPassed;

        sun.transform.RotateAround(transform.forward, 1 * Time.deltaTime);
    }
}
