using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent_Type : MonoBehaviour
{
    public enum TypeAgent {Ally, Enemy };
    public TypeAgent Type;


    private SelectionPlayer sp;

    private void OnEnable()
    {
        sp = GameObject.Find("GameManager").GetComponent<SelectionPlayer>();
        if (Type == TypeAgent.Ally) sp.allFriendlyUnits.Add(gameObject);

    }

    private void OnDisable()
    {
        if (Type == TypeAgent.Ally) sp.allFriendlyUnits.Remove(gameObject);
    }
}
