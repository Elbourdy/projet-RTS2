using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AgentStates))]
public class AIEnemy : MonoBehaviour
{
    private AgentStates aS;
    private ClassAgentContainer cac;
    public GameObject targetPlayer;

    public float radiusVision = 5;

    public Agent_Type.TypeAgent typeToTarget;

    private void Awake()
    {
        aS = GetComponent<AgentStates>();
        cac = GetComponent<ClassAgentContainer>();
        radiusVision = cac.myClass.radiusVision;
    }
    private void Update()
    {
        if (aS.myState != AgentStates.states.Follow)
        SearchForTarget();
    }



    private void AttackOrder()
    {
        if (aS.myState != AgentStates.states.Agressif)
        {
            aS.SetTarget(targetPlayer);
            aS.SetState(AgentStates.states.Agressif);
        }
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
                        targetPlayer = hits[i].gameObject;
                        AttackOrder();
                        break;
                    }
                }
            }
        }
    }

}
