using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TickManager : MonoBehaviour
{
    public static TickManager instance;

    public void Awake()
    {
        instance = this;
    }

    public int timerForATick = 30;
    private float timerCount;
    private int totalTickCount;

    public GameObject hBTick;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetFeedbackTimer();
        timerCount += Time.deltaTime;

        if (timerCount >= timerForATick)
        {
            timerCount = 0;
            BatteryManager.instance.ChargeUnit();
        }
    }

    public void SetFeedbackTimer()
    {
        float fillValue = timerCount / timerForATick;
        hBTick.transform.GetChild(0).GetComponent<Image>().fillAmount = fillValue;
        hBTick.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = (Mathf.Round(timerForATick - timerCount)).ToString();
    }
}
