using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRecapProduction : MonoBehaviour
{
    // this script needs to be changed in the future
    // script sur le bouton recap production qui sert � annuler la production d'une unit� 

    public int ID;

    public void ButtonPress()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Action/UI_Act_Click/UI_Act_Click");
        HQBehavior.instance.RemoveFromQueue(ID);
    }
}
