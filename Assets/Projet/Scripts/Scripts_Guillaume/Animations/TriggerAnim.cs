using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnim : MonoBehaviour
{
    private Animator myMator;
    private AgentStates myAs;

    private void Awake()
    {
        myMator = GetComponent<Animator>();
    }



}
