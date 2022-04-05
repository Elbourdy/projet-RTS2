using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagesBraodcaster : MonoBehaviour
{
    [HideInInspector] public Agent_Type.TypeAgent myType;

    private AgentStates myAgentState;
    private AIAgents myAi;

    [SerializeField] private float radiusHelp = 3f;


    private void Awake()
    {
        myType = GetComponent<Agent_Type>().Type;
        myAgentState = GetComponent<AgentStates>();
    }

    private void OnEnable()
    {
        myAi = GetComponent<AIAgents>();
        myAi.onFindingEnemy += GetHelp;
    }

    private void OnDisable()
    {
        myAi.onFindingEnemy -= GetHelp;
    }


    private void GetHelp()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radiusHelp, LayerMask.GetMask("GameplayUnits"));
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].GetComponent<MessagesBraodcaster>())
                {
                    var unitBroadcaster = hits[i].GetComponent<MessagesBraodcaster>();
                    if (unitBroadcaster.myType == myType)
                    {
                        AskForHelp(unitBroadcaster);
                    }
                }
            }
        }
    }

    public void AskForHelp(MessagesBraodcaster allyBroadcaster)
    {
        if (!allyBroadcaster.myAgentState.HasTarget())
        {
            allyBroadcaster.myAgentState.SetTarget(myAgentState.ReturnTarget());
            allyBroadcaster.myAgentState.SetState(AgentStates.states.Aggressive);
        }
    }
}
