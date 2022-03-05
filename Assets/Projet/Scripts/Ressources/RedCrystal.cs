using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCrystal : MonoBehaviour
{
    private LineRenderer lR;
    private Transform nexusCenter;

    private float drainRange, timerCount;
    [SerializeField] private float tickPerSeconds;
    [SerializeField] private int removePerTick;


    // Start is called before the first frame update
    void Start()
    {
        lR = GetComponent<LineRenderer>();
        nexusCenter = HQBehavior.instance.transform.GetChild(3).GetChild(6);
    }


    void Update()
    {
        drainRange = BatteryManager.instance.radiusBattery * NexusLevelManager.instance.GetMultiplicatorRangeNexus();
        if (Vector3.Distance(transform.position, nexusCenter.position) < drainRange)
        {
            SetFeedbackNexusCollecting();
            

            if (timerCount >= 1/tickPerSeconds)
            {
                Global_Ressources.instance.ModifyRessource(0, -removePerTick);
                timerCount = 0;
            }
            timerCount += Time.deltaTime;
        }
        else
        {
            DisableFeedbackCollectionNexus();
        }
    }

    private void SetFeedbackNexusCollecting()
    {
        lR.enabled = true;
        lR.SetPosition(0, nexusCenter.position);
        lR.SetPosition(1, transform.position);
    }

    private void DisableFeedbackCollectionNexus()
    {
        lR.enabled = false;
    }
}
