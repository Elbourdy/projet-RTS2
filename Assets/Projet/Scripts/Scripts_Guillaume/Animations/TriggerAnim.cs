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
        myMator = GetComponent<Animator>();
        myAs = GetComponent<AgentStates>();
    }


    private void OnEnable()
    {
        myAs.onAttack += TriggerAttack;
    }

    private void TriggerAttack()
    {
        myMator.SetTrigger("AttackTrigger");
    }

}
