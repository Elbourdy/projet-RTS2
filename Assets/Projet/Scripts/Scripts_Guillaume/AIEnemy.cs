using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AgentStates))]
public class AIEnemy : MonoBehaviour
{
    private AgentStates aS;
    private NavMeshAgent navM;
    
    private ClassAgentContainer cac;
    public GameObject targetPlayer;

    public float radiusVision = 5;

    public Agent_Type.TypeAgent typeToTarget;
    public bool hasTargetInSight = false;


    private void Awake()
    {
        aS = GetComponent<AgentStates>();
        cac = GetComponent<ClassAgentContainer>();
        radiusVision = cac.myClass.radiusVision;
        navM = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        //if (aS.isSuperAggressif || aS.myState == AgentStates.states.Idle)
        SearchForTarget();
    }



    private void AttackOrder()
    {

            aS.SetTarget(targetPlayer);
        if (aS.myState != AgentStates.states.Agressif && aS.myState != AgentStates.states.Follow) aS.SetState(AgentStates.states.Agressif);
    }

    private void DrawRadiusVision()
    {
        if (cac == null) 
        { 
            cac = GetComponent<ClassAgentContainer>();
            radiusVision = cac.myClass.radiusVision;
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radiusVision);
    }


    private void OnDrawGizmos()
    {
        DrawRadiusVision();
    }

    private void SearchForTarget()
    {
        if (aS.myState != AgentStates.states.Agressif)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, radiusVision);
            if (hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].GetComponent<Agent_Type>() != null && hits[i].GetComponent<Agent_Type>().Type == typeToTarget)
                    {
                        hasTargetInSight = true;
                        targetPlayer = hits[i].gameObject;
                        AttackOrder();
                        break;
                    }
                    hasTargetInSight =false;
                }
            }
        }
    }

}
