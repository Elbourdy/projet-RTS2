using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunCycleBehavior : MonoBehaviour
{
    public enum statesDay { Day, Night };
    public enum statesSeason { Winter, Spring, Summer, Autumn }

    public Light light;
    public Gradient colorLightDay, colorLightNight;
    public float timeOfDay, timeOfNight;
    public int numberOfDayInASeason = 10;

    private float totalTimeOfADay, timerDayCount;
    private int dayCount;

    [Header("OnReadOnly")]
    [SerializeField] private float hourDaytimeCurrentlyIn = 0f;
    [SerializeField] private float hourInCurrentDayPeriod = 0f;
    [SerializeField] private int numberOfDaysPassed = 0;
    [SerializeField] private statesDay periodOfDay = statesDay.Day;
    [SerializeField] private statesSeason currentSeason = statesSeason.Autumn;



    // Start is called before the first frame update
    void Start()
    {
        totalTimeOfADay = timeOfDay + timeOfNight;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerDayCount / totalTimeOfADay > 1)
        {
            timerDayCount = 0;
            numberOfDaysPassed++;

            if (numberOfDaysPassed > 0 && numberOfDaysPassed%numberOfDayInASeason == 0)
            {
                switch(currentSeason)
                {
                    case statesSeason.Winter:
                        currentSeason = statesSeason.Spring;
                        break;

                    case statesSeason.Spring:
                        currentSeason = statesSeason.Summer;
                        break;

                    case statesSeason.Summer:
                        currentSeason = statesSeason.Autumn;
                        break;

                    case statesSeason.Autumn:
                        currentSeason = statesSeason.Winter;
                        break;
                }
            }
        }

        timerDayCount += Time.deltaTime;

        if (timerDayCount < timeOfDay)
        {
            light.color = colorLightDay.Evaluate(timerDayCount / timeOfDay);

            periodOfDay = statesDay.Day;
            hourInCurrentDayPeriod = timerDayCount / timeOfDay;
        }
        else
        {
            light.color = colorLightNight.Evaluate((timerDayCount - timeOfDay) / timeOfNight);

            periodOfDay = statesDay.Night;
            hourInCurrentDayPeriod = (timerDayCount - timeOfDay) / timeOfNight;
        }

        hourDaytimeCurrentlyIn = timerDayCount / totalTimeOfADay;
    }

    


}
