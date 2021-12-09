using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SunCycleBehavior : MonoBehaviour
{
    public bool activateCycle = true;

    public enum statesDay { Day, Night };
    public enum statesSeason { Winter, Spring, Summer, Autumn }

    public Light light;
    public Gradient colorLightDaySpring, colorLightNightSpring, colorLightDaySummer, colorLightNightSummer, colorLightDayAutumn, colorLightNightAutumn, colorLightDayWinter, colorLightNightWinter;
    private Gradient actualGradientDay, actualGradientNight;

    public float timeOfDay, timeOfNight;
    public int numberOfDayInASeason = 10;

    private float totalTimeOfADay, timerDayCount;
    private int dayCount;

    public List<Material> matTopToBottom = new List<Material>();
    public List<Color> springColors, summerColors, autumnColors, winterColors = new List<Color>();

    public GameObject progressionBarGlobal, progressionBarActualPeriod, actualPeriodOfDayDisplay, numberOfDaysPassedDisplay, currentSeasonDisplay, daysBeforeNextSeason;

    [Header("OnReadOnly")]
    [SerializeField] private float hourDaytimeCurrentlyIn = 0f;
    [SerializeField] private float hourInCurrentDayPeriod = 0f;
    [SerializeField] private int numberOfDaysPassed = 0;
    [SerializeField] private statesDay periodOfDay = statesDay.Day;
    [SerializeField] private statesSeason currentSeason = statesSeason.Spring;



    // Start is called before the first frame update
    void Start()
    {
        totalTimeOfADay = timeOfDay + timeOfNight;
        ChangeColor();
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

                ChangeColor();
            }
        }

        timerDayCount += Time.deltaTime;

        if (timerDayCount < timeOfDay)
        {
            if (activateCycle)
                light.color = actualGradientDay.Evaluate(timerDayCount / timeOfDay);

            periodOfDay = statesDay.Day;
            hourInCurrentDayPeriod = timerDayCount / timeOfDay;
        }
        else
        {
            if (activateCycle)
                light.color = actualGradientNight.Evaluate((timerDayCount - timeOfDay) / timeOfNight);

            periodOfDay = statesDay.Night;
            hourInCurrentDayPeriod = (timerDayCount - timeOfDay) / timeOfNight;
        }

        hourDaytimeCurrentlyIn = timerDayCount / totalTimeOfADay;

        TemporaryDisplay(); 
    }

    public void ChangeColor()
    {
        List<Color> currentListColors = new List<Color>();
        switch(currentSeason)
        {
            case statesSeason.Spring :
                currentListColors = springColors;
                actualGradientDay = colorLightDaySpring;
                actualGradientNight = colorLightNightSpring;
                break;

            case statesSeason.Summer:
                currentListColors = summerColors;
                actualGradientDay = colorLightDaySummer;
                actualGradientNight = colorLightNightSummer;
                break;

            case statesSeason.Autumn:
                currentListColors = autumnColors;
                actualGradientDay = colorLightDayAutumn;
                actualGradientNight = colorLightNightAutumn;
                break;

            case statesSeason.Winter:
                currentListColors = winterColors;
                actualGradientDay = colorLightDayWinter;
                actualGradientNight = colorLightNightWinter;
                break;
        }

        for (int i = 0; i < matTopToBottom.Count; i++)
        {
            matTopToBottom[i].color = currentListColors[i];
        }
    }

    public void TemporaryDisplay()
    {
        progressionBarGlobal.GetComponent<HealthBar>().SetHealth(hourDaytimeCurrentlyIn);
        progressionBarActualPeriod.GetComponent<HealthBar>().SetHealth(hourInCurrentDayPeriod);
        actualPeriodOfDayDisplay.transform.GetChild(0).GetComponent<Text>().text = periodOfDay.ToString();
        numberOfDaysPassedDisplay.transform.GetChild(0).GetComponent<Text>().text = numberOfDaysPassed.ToString();
        currentSeasonDisplay.transform.GetChild(0).GetComponent<Text>().text = currentSeason.ToString();
        daysBeforeNextSeason.transform.GetChild(0).GetComponent<Text>().text = (numberOfDayInASeason - (numberOfDaysPassed % numberOfDayInASeason)).ToString();
    }


}
