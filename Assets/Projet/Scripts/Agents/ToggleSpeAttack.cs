using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpeAttackClass))]
[RequireComponent(typeof(SelectableObject))]
public class ToggleSpeAttack : MonoBehaviour
{
    public bool allowAutomaticSpeAttack = true;

    [SerializeField] private SelectableObject mySelectable;
    [SerializeField] private SpeAttackClass mySpeClass;
    [SerializeField] private AgentStates myAgent;

    

    private void Update()
    {
        if (mySelectable.IsSelected)
        {
            ToggleCanSpe();
        }
    }


    private void ToggleCanSpe()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            myAgent.canSpeAttack = !myAgent.canSpeAttack;
        }
    }

}
