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
    [SerializeField] private int tickToSwitchPhase = 5;
    [SerializeField] private Sprite spriteTickOn, spriteTickOff;
    [SerializeField] private int timeBeforeAttack = 2;

    private int totalTickCount;
    private float timerCount;

    public GameObject hBTick;
    public List<GameObject> tickFeedbacks = new List<GameObject>(); 

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

    public void TickEffect() //s'applique quand un tick supplementaire apparaît
    {
        timerCount = 0;
        //BatteryManager.instance.ChargeUnit();
        totalTickCount++;
        SetTickFeedback();

        if (totalTickCount == tickToSwitchPhase)
        {
            dayState = statesDay.Night;
            HQBehavior.instance.currentNexusState = HQBehavior.statesNexus.ForcedImmobilize;
        }
    }

    public void ResetTickFeedback()
    {
        for (int i = 0; i < tickToSwitchPhase; i++)
        {
            tickFeedbacks[i].GetComponent<Image>().sprite = spriteTickOff;
            totalTickCount = 0;
            timerCount = 0;
        }
    }

    public void SetTickFeedback()
    {
        for(int i = 0; i < tickToSwitchPhase; i++)
        {
            if (i < totalTickCount)
            {
                tickFeedbacks[i].GetComponent<Image>().sprite = spriteTickOn;
            }
            else
            {
                tickFeedbacks[i].GetComponent<Image>().sprite = spriteTickOff;
            }
        }
    }
}
