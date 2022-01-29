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

    // Start is called before the first frame update
    void Start()
    {
        
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
        hBTick.transform.GetChild(0).GetComponent<Image>().fillAmount = fillValue;
        hBTick.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = (Mathf.Round(timerForATick - timerCount)).ToString();
    }

    public void TickEffect() //s'applique quand un tick supplementaire appara�t
    {
        dayState = statesDay.Night;
        HQBehavior.instance.currentNexusState = HQBehavior.statesNexus.ForcedImmobilize;
        timerCount = 0;

        FMODUnity.RuntimeManager.PlayOneShot(soundNexusStop, HQBehavior.instance.gameObject.transform.position);
    }

    public void ResetTickCounter()
    {
         timerCount = 0;
    }

    public void LaunchNightAttack()
    {
        timerCount = timerForATick;
    }
}
