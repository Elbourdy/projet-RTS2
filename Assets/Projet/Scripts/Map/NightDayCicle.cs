using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightDayCicle : MonoBehaviour
{
    //script qui gère le visuel du cycle jour/nuit
    public TickManager.statesDay currentState = TickManager.statesDay.Day;

    [Header("Parameters for the day/night cycle")]
    [SerializeField] private Light light;
    [SerializeField] private AnimationCurve curveLightIntensity;
    [SerializeField] private Gradient gradientColorLight;
    [SerializeField] private float maxIntensity = 2.25f;
    [SerializeField] private float threhsoldCurveDawn = 0.25f;
    [SerializeField] private float thresoldCurveDusk = 0.75f;

    private float durationDawnAndDuskTransition;
    private float timeCount;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        durationDawnAndDuskTransition = TickManager.instance.timeTransitionDuskAndDawn;
    }

    // Update is called once per frame
    void Update()
    {
        currentState = TickManager.instance.dayState;
        SetFeedbackStatesDay();
    }

    private void SetFeedbackStatesDay()
    {
        float newIntensity = 0f;
        Color newColor = Color.white;

        switch(currentState)
        {
            case TickManager.statesDay.Day:
                newIntensity = curveLightIntensity.Evaluate(threhsoldCurveDawn + TickManager.instance.GetDayAvancement()/2) * maxIntensity;
                newColor = gradientColorLight.Evaluate(threhsoldCurveDawn + TickManager.instance.GetDayAvancement()/2);
                timeCount = 0;
                break;

            case TickManager.statesDay.Dusk:
                timeCount += Time.deltaTime;
                newIntensity = curveLightIntensity.Evaluate(thresoldCurveDusk + (timeCount / durationDawnAndDuskTransition)/4) * maxIntensity;
                newColor = gradientColorLight.Evaluate(thresoldCurveDusk + (timeCount / durationDawnAndDuskTransition)/4);
                break;

            case TickManager.statesDay.Night:
                newIntensity = curveLightIntensity.Evaluate(1);
                newColor = gradientColorLight.Evaluate(1);
                timeCount = 0f;
                break;

            case TickManager.statesDay.Dawn:
                timeCount += Time.deltaTime;
                newIntensity = curveLightIntensity.Evaluate((timeCount / durationDawnAndDuskTransition)/4) * maxIntensity;
                newColor = gradientColorLight.Evaluate((timeCount / durationDawnAndDuskTransition)/4);
                break;
        }
        light.intensity = newIntensity;
        light.color = newColor;
    }
}
