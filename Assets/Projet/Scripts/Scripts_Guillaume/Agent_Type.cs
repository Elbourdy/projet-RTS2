using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent_Type : MonoBehaviour
{
    public enum TypeAgent {Ally, Enemy };
    public TypeAgent AgentType;


    public SelectionPlayer sp;

    private void OnEnable()
    {
        sp = GameObject.Find("GameManager").GetComponent<SelectionPlayer>();
        if (AgentType == TypeAgent.Ally) sp.allFriendlyUnits.Add(gameObject);

    }

    private void OnDisable()
    {
        if (AgentType == TypeAgent.Ally) sp.allFriendlyUnits.Remove(gameObject);
    }
}
