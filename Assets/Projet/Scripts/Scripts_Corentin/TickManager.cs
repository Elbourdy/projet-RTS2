using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TickManager : MonoBehaviour
{
    public static TickManager instance;

    public enum statesDay {Move, Anchor, Attacked};
    public statesDay nexusState = statesDay.Move;

    public void Awake()
    {
        instance = this;
    }

    [SerializeField] private int timerForATick = 30;
    [SerializeField] private int tickToSwitchPhase = 5;
    [SerializeField] private Sprite spriteTickOn, spriteTickOff;
    [SerializeField] private int timerBeforenextAttack = 5;

    private int totalTickCount;
    private float timerCount;
    private List<GameObject> ennemiesAlive = new List<GameObject>();

    public GameObject hBTick;
    public List<GameObject> tickFeedbacks = new List<GameObject>(); 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(nexusState)
        {
            case statesDay.Move:
                SetFeedbackTimer();
                timerCount += Time.deltaTime;

                if (timerCount >= timerForATick)
                {
                    TickEffect();
                }

                break;

            case statesDay.Anchor:

                timerCount += Time.deltaTime;
                
                if (timerCount > timerBeforenextAttack)
                {
                    SpawnEnnemiesAttackingNexus();
                    nexusState = statesDay.Attacked;
                }

                break;

            case statesDay.Attacked:

                if (ennemiesAlive.Count == 0)
                {
                    ResetTickFeedback();
                    nexusState = statesDay.Move;
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

    public void TickEffect()
    {
        timerCount = 0;
        BatteryManager.instance.ChargeUnit();
        totalTickCount++;
        SetTickFeedback();

        if (totalTickCount == tickToSwitchPhase)
        {
            nexusState = statesDay.Anchor;
        }
    }

    public void ResetTickFeedback()
    {
        for (int i = 0; i < tickToSwitchPhase; i++)
        {
            tickFeedbacks[i].GetComponent<Image>().sprite = spriteTickOff;
            totalTickCount = 0;
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

    public void SpawnEnnemiesAttackingNexus()
    {

    }

}
