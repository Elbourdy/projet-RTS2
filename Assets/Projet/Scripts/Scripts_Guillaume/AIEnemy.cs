using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AgentStates))]
public class AIEnemy : MonoBehaviour
{
    private AgentStates aS;
    public GameObject targetPlayer;

    public float radiusVision = 1;


    private void OnEnable()
    {
        aS = GetComponent<AgentStates>();
    }
    private void Update()
    {
        //if (Input.GetKeyUp(KeyCode.N)) AttackOrder();
        SearchForTarget();
    }



    private void AttackOrder()
    {
        aS.SetTarget(targetPlayer);
        aS.SetState(AgentStates.states.Agressif);
        aS.MoveAgent(targetPlayer.transform.position);
    }




    private void SearchForTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radiusVision);
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].GetComponent<Agent_Type>() != null && hits[i].GetComponent<Agent_Type>().Type == Agent_Type.TypeAgent.Ally) 
                {
                    targetPlayer = hits[i].gameObject;
                    AttackOrder();
                    break;
                }
            }
        }
    }

}
