using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentStates))]
public class TriggerAnim : MonoBehaviour
{
    private Animator myMator;
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


    private void Update()
    {
        Debug.Log(myMator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
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
        myMator.SetTrigger("AttackTrigger");
    }

    private void TriggerRun()
    {
        if (myMator != null)
            myMator.SetTrigger("RunTrigger");
    }

    private void TriggerIdle()
    {
        if (myMator != null)
            myMator.SetTrigger("IdleTrigger");
    }
}
