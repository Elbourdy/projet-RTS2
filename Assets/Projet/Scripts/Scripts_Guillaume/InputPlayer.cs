using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlayer : MonoBehaviour
{
    public GameObject agent;


    private SelectionPlayer sp;


    private void Start()
    {
        sp = GetComponent<SelectionPlayer>();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (sp.selectedUnits.Count > 0)
            {
                CheckHitType();
            }
        }

        //if (Input.GetKey(KeyCode.Space) && Input.GetMouseButtonDown(1))
        //{
        //    ConstructOrder();
        //  Building.SetConstructionHealth(GetConstructionHealth() += construction * Time.deltaTime);
        //}
    }

    private void ConstructOrder()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.name == "Sol")
            {
                agent.GetComponent<AgentStates>().SetState(AgentStates.states.Construction);
                agent.GetComponent<AgentStates>().MoveAgent(hit.point);
            }
        }
    }

    private void CheckHitType ()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.name == "Sol")
            {
                GoToTarget(hit);
            }

            if (hit.collider.CompareTag("Ressource"))
            {
                Recolte(hit);
            }

            if (hit.collider.GetComponent<Agent_Type>() != null)
            {
                if (hit.collider.GetComponent<Agent_Type>().Type == Agent_Type.TypeAgent.Enemy)
                {
                    AttaqueWithAgent(hit);
                }
            }
        }
    }


    private void GoToTarget (RaycastHit hit)
    {
        foreach (var agent in sp.selectedUnits)
        {
            if (agent.GetComponent<AgentStates>() != null)
            {
                agent.GetComponent<AgentStates>().SetState(AgentStates.states.Follow);
                agent.GetComponent<AgentStates>().MoveAgent(hit.point);
            }
        }
    }

    private void MoveAgents (RaycastHit hit)
    {
        foreach (var agent in sp.selectedUnits)
        {
            if (agent.GetComponent<AgentStates>() != null)
            {
                agent.GetComponent<AgentStates>().SetState(AgentStates.states.Idle);
                agent.GetComponent<AgentStates>().MoveAgent(hit.point);
            }
        }
    }

    private void AttaqueWithAgent (RaycastHit hit)
    {

        foreach (var agent in sp.selectedUnits)
        {
            if (agent.GetComponent<AgentStates>() != null)
            {
                agent.GetComponent<AgentStates>().SetTarget(hit.collider.gameObject);
                agent.GetComponent<AgentStates>().SetState(AgentStates.states.Agressif);
                agent.GetComponent<AgentStates>().MoveAgent(hit.point);
            }
        }
    }

    private void Recolte (RaycastHit hit)
    {
        foreach (var agent in sp.selectedUnits)
        {
            if (agent.GetComponent<AgentStates>() != null)
            {
                agent.GetComponent<AgentStates>().SetState(AgentStates.states.Recolte);
                agent.GetComponent<AgentStates>().SetRessourceTarget(hit.collider.gameObject);
                agent.GetComponent<AgentStates>().MoveAgent(hit.point);
            }
        }
    }



}
