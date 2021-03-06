using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBehavior : MonoBehaviour
{
    private BaliseBehavior.statesBalise switchBehavior = BaliseBehavior.statesBalise.Classic;
    private Animator animator;

    public BaliseBehavior bB;

    public bool activated = false;

    public float countdownTime = 15f;
    private float simultaneousTime = 2f, count = 0f;

    private LineRenderer lR;
    public Material matOn, matOff;

    private HealthBar bar;

    FMOD.Studio.EventInstance switchOn;

    private void OnDestroy()
    {
        if (switchBehavior == BaliseBehavior.statesBalise.CountDown)
            bar.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (switchBehavior == BaliseBehavior.statesBalise.CountDown)
        {
            bar = transform.GetChild(1).GetChild(0).GetComponent<HealthBar>();
            bar.gameObject.SetActive(false);
        }

        lR = GetComponent<LineRenderer>();
        InitialiseLineRenderer();

        animator = transform.GetChild(0).GetComponent<Animator>();

        switchOn = FMODUnity.RuntimeManager.CreateInstance("event:/Building/Build_UnlimitedBattery/Build_UnlitBat_Rise/Build_UnlitBat_Rise");
        switchOn.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            if (switchBehavior == BaliseBehavior.statesBalise.CountDown)
            {
                bar.SetHealth(1 - (count / countdownTime));

                if (count > countdownTime)
                {
                    activated = false;
                    lR.material = matOff;
                    bar.gameObject.SetActive(false);
                    animator.SetBool("Activated", false);
                }
            }

            if (switchBehavior == BaliseBehavior.statesBalise.Simultaneous && count > simultaneousTime)
            {
                activated = false;
                lR.material = matOff;
                animator.SetBool("Activated", false);
            }
            count += Time.deltaTime;
        }  
    }

    public void SetState(bool state)
    {
        switch(switchBehavior)
        {
            case BaliseBehavior.statesBalise.Classic:
                if (activated != state)
                {
                    switchOn.start();
                    activated = state;
                    lR.material = matOn;
                    animator.SetBool("Activated", true);
                    bB.Switch();
                }
                break;

            case BaliseBehavior.statesBalise.CountDown:
                if (activated != state)
                {
                    activated = state;
                    count = 0;
                    lR.material = matOn;
                    bar.gameObject.SetActive(true);
                    animator.SetBool("Activated", true);
                    bB.Switch();
                }
                break;

            case BaliseBehavior.statesBalise.Simultaneous:
                if (activated != state)
                {
                    activated = state;
                    count = 0;
                    lR.material = matOn;
                    animator.SetBool("Activated", true);
                    bB.Switch();
                }
                break;
        }
        activated = state;
    }

    public bool GetState()
    {
        return activated;
    }

    public void SetSwitchType(BaliseBehavior.statesBalise sB)
    {
        switchBehavior = sB;
    }

    private void InitialiseLineRenderer()
    {
        lR.positionCount = 2;
        lR.SetPosition(0, transform.position);
        lR.SetPosition(1, bB.spawnPoint.position);
        lR.material = matOff;
    }
}
