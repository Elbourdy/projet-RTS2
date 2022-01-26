using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRecapProduction : MonoBehaviour
{
    // this script needs to be changed in the future
    public int ID;

    void Start()
    {
        
    }

    public void ButtonPress()
    {
        HQBehavior.instance.RemoveFromQueue(ID);
    }
}
