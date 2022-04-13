using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



// IA des alliés et ennemis
// On fait le max pour que les actions du joueur soit prioritaire par rapport à ce script


// LOGIQUE A REVOIR APRES SOUTENANCE, FUTUR PROBLEME INCOMING
// GROS PROBLEME DE CONDITIONS TROP FRAGILES. FAIS LE TAF MAINTENANT, MAIS NE PERMET AUCUNE VERSATILITE DES COMPORTEMENTS FUTURS 


[RequireComponent(typeof(AgentStates))]
public class AIAgents : MonoBehaviour
{
    public delegate void EventAI();
    public EventAI onFindingEnemy;


    private AgentStates aS;
    
    private ClassAgentContainer cac;
    public GameObject newTarget;

    public float radiusVision = 5;

    public Agent_Type.TypeAgent typeToTarget;
    public bool hasTargetInSight = false;

    public bool canSearch = true;
    private void Awake()
    {
        aS = GetComponent<AgentStates>();
        cac = GetComponent<ClassAgentContainer>();
        radiusVision = cac.myClass.radiusVision;
    }
    private void Update()
    {
        if (canSearch)
        {
            SearchForTarget();
            CheckVisbilityTarget();
        }
    }

    private void CheckVisbilityTarget()
    {
        if (aS.HasTarget())
        {
            if (!aS.ReturnTarget().GetComponent<Agent_Type>().GetIsTargetable())
            {
                aS.SetTarget(null);
            }
        }
    }


    private void AttackOrder()
    {
        aS.SetTarget(newTarget);

        // On ne remet un agent déjà aggresif dans cet état
        // Un agent se dirigeant vers un endroit précis n'attaque pas tant qu'il n'est pas arrivé (permet fuite entre autre

        if (aS.myState != AgentStates.states.Aggressive && aS.myState != AgentStates.states.Follow)
        { 
            aS.SetState(AgentStates.states.Aggressive);
            onFindingEnemy?.Invoke();
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
        if (aS.myState != AgentStates.states.Aggressive || IsCapableOfSuperAgression())
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, radiusVision, LayerMask.GetMask("GameplayUnits"));
            if (hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].GetComponent<Agent_Type>() != null && hits[i].GetComponent<Agent_Type>().Type == typeToTarget)
                    {
                        if (hits[i].GetComponent<Agent_Type>().GetIsTargetable())
                        hasTargetInSight = true;
                        newTarget = hits[i].gameObject;
                        AttackOrder();
                        break;
                    }
                    hasTargetInSight = false;
                }
            }
        }
    }

    private bool IsCapableOfSuperAgression()
    {
        if (aS.myState == AgentStates.states.Aggressive && aS.isSuperAggressive && aS.ReturnTarget().name == "Nexus")
        {
            return true;
        }
        return false;
    }

}
