using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentStates))]
public class TriggerAnim : MonoBehaviour
{
    public Animator myMator;
    private AgentStates myAs;

    private void Awake()
    {
        if (GetComponent<Animator>() != null)
        {
            myMator = GetComponent<Animator>();
        }
        else if (GetComponentInChildren<Animator>() != null)
        {
            myMator = GetComponentInChildren<Animator>();
        }
        myAs = GetComponent<AgentStates>();
    }


    

    private void OnEnable()
    {
        myAs.onAttack += TriggerAttack;
        myAs.onFollowEnter += TriggerRun;
        myAs.onIdleEnter += TriggerIdle;
    }

    private void TriggerAttack()
    {
        if (myMator != null)
        {
            //myMator.ResetTrigger("IdleTrigger");
            myMator.SetTrigger("AttackTrigger");
        }
    }

    private void TriggerRun()
    {
        if (myMator != null)
        {
            //myMator.ResetTrigger("IdleTrigger");
            myMator.SetTrigger("RunTrigger");
        }
    }

    private void TriggerIdle()
    {
        if (myMator != null && !myMator.IsInTransition(0))
        {
            ResetTrigger();
            myMator.SetTrigger("IdleTrigger");
        }
    }



    private void ResetTrigger()
    {
        myMator.ResetTrigger("IdleTrigger");
        myMator.ResetTrigger("RunTrigger");
        myMator.ResetTrigger("AttackTrigger");
    }
}
