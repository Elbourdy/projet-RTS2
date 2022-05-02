using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpeAttackClass))]
[RequireComponent(typeof(SelectableObject))]
public class ToggleSpeAttack : MonoBehaviour
{
    public bool allowToggle = true;

    [SerializeField] private SelectableObject mySelectable;
    [SerializeField] private SpeAttackClass mySpeClass;
    [SerializeField] private AgentStates myAgent;


    private void Update()
    {
        if (mySelectable.IsSelected && allowToggle)
        {
            ToggleCanSpe();
        }
    }


    private void ToggleCanSpe()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            myAgent.allowAutomaticAttack = !myAgent.allowAutomaticAttack;
        }
    }


    public void toggleButtonSpeAttack()
    {
        if (mySelectable.IsSelected && allowToggle)
        {
            myAgent.allowAutomaticAttack = !myAgent.allowAutomaticAttack;
        }
    }

    public bool GetStateAuto()
    {
        return myAgent.allowAutomaticAttack;
    }


    
}
