using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TickManager : MonoBehaviour
{
    public static TickManager instance;

    public enum statesDay {Day, Night};
    public statesDay dayState = statesDay.Day;

    public void Awake()
    {
        instance = this;
    }

    [SerializeField] private int timerForATick = 30;
    [SerializeField] private int timeBeforeAttack = 2;

    private float timerCount;
    public GameObject hBTick;

    private string soundNexusStop = "event:/Building/Build_Nexus/Build_Nex_OnStop/Build_Nex_OnStop";
    FMOD.Studio.EventInstance soundEnvironnementManager;

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
                SetFeedbackTimer();
                timerCount += Time.deltaTime;

                if (timerCount >= timerForATick)
                {
                    TickEffect();
                }

                break;

            case statesDay.Night:
                timerCount += Time.deltaTime;
                NightAttack.instance.PreparationStartAttack();

                if (timerCount >= timeBeforeAttack)
                {
                    NightAttack.instance.SpawnEnnemies();
                }
                    
                break;
        } 
    }

    public void SetFeedbackTimer()
    {
        float fillValue = timerCount / timerForATick;
        hBTick.transform.GetChild(1).GetComponent<Image>().fillAmount = fillValue;
        hBTick.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = (Mathf.Round(timerForATick - timerCount)).ToString();
    }

    public void TickEffect() //s'applique quand un tick supplementaire apparaît
    {
        dayState = statesDay.Night;
        HQBehavior.instance.currentNexusState = HQBehavior.statesNexus.ForcedImmobilize;
        timerCount = 0;

        FMODUnity.RuntimeManager.PlayOneShot(soundNexusStop, HQBehavior.instance.gameObject.transform.position);

        soundEnvironnementManager.setParameterByName("Day_Or_Night", 1);
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
}
