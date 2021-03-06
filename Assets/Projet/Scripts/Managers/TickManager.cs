using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TickManager : MonoBehaviour
{
    //g?re le syst?me jour/nuit
    public static TickManager instance;

    public enum statesDay {Day, Night, Dusk, Dawn};
    public statesDay dayState = statesDay.Day;

    public void Awake()
    {
        instance = this;
    }

    [SerializeField] private int timerForATick = 30;
    [SerializeField] public int timeTransitionDuskAndDawn = 5;

    private float timerCount;
    public GameObject hBTick;

    private string soundNexusStop = "event:/Building/Build_Nexus/Build_Nex_OnStop/Build_Nex_OnStop";
    FMOD.Studio.EventInstance soundEnvironnementManager;

    public int numberOfDaysPassed = 0;

    private bool startNight = false;

    // Start is called before the first frame update
    void Start()
    {
        soundEnvironnementManager = FMODUnity.RuntimeManager.CreateInstance("event:/Environnement/Env_Phase/Env_Phs_Manager/Env_Phs_Manager");
        soundEnvironnementManager.start();
    }

    // Update is called once per frame
    void Update()
    {
        switch(dayState)
        {
            case statesDay.Day:
                NightDayModule.instance.UpdateHUD(timerCount/timerForATick * 1.0f);
                timerCount += Time.deltaTime;

                if (timerCount >= timerForATick)
                {
                    TickEffect();
                }

                break;

            case statesDay.Dusk:
                if (!startNight)
                {
                    NightAttack.instance.PreparationStartAttack();
                    startNight = true;
                }
                    

                timerCount += Time.deltaTime;
                if (timerCount >= timeTransitionDuskAndDawn)
                {
                    dayState = statesDay.Night;
                    timerCount = 0;
                    startNight = false;
                }
                break; 

            case statesDay.Night:
                NightAttack.instance.SpawnEnnemies(numberOfDaysPassed);
                break;

            case statesDay.Dawn:
                if (timerCount == 0)
                    NightDayModule.instance.SwitchDay();

                timerCount += Time.deltaTime;
                if (timerCount >= timeTransitionDuskAndDawn)
                {
                    dayState = statesDay.Day;
                    timerCount = 0;
                    numberOfDaysPassed++;
                }
                break;
        } 
    }

    public void TickEffect() //s'applique quand un tick supplementaire appara?t
    {
        dayState = statesDay.Dusk;
        HQBehavior.instance.currentNexusState = HQBehavior.statesNexus.ForcedImmobilize;
        timerCount = 0;

        FMODUnity.RuntimeManager.PlayOneShot(soundNexusStop, HQBehavior.instance.gameObject.transform.position);

        soundEnvironnementManager.setParameterByName("Day_Or_Night", 1);

        NightDayModule.instance.SwitchNight();
    }

    public void ResetTickCounter()
    {
        timerCount = 0;
        soundEnvironnementManager.setParameterByName("Day_Or_Night", 0);
    }

    public void LaunchNightAttack()
    {
        timerCount = timerForATick;
    }

    public float GetDayAvancement()
    {
        return timerCount / timerForATick;
    }
}
