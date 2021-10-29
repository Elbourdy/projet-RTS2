using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlayer : MonoBehaviour
{
    public GameObject agent;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CheckHitType();
        }

        if (Input.GetKey(KeyCode.Space) && Input.GetMouseButtonDown(1))
        {
            ConstructOrder();
        }
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
                agent.GetComponent<AgentStates>().SetState(AgentStates.states.Idle);
                agent.GetComponent<AgentStates>().MoveAgent(hit.point);
            }

            if (hit.collider.name == "Ressource")
            {
                agent.GetComponent<AgentStates>().SetState(AgentStates.states.Recolte);
                agent.GetComponent<AgentStates>().MoveAgent(hit.point);
            }


            if (hit.collider.name == "Ennemi")
            {
                agent.GetComponent<AgentStates>().SetObjectDestination(hit.collider.gameObject);
                agent.GetComponent<AgentStates>().SetState(AgentStates.states.Agressif);
                agent.GetComponent<AgentStates>().MoveAgent(hit.point);
            }
        }
    }


    
}
