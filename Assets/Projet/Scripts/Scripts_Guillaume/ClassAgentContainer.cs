using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ClassAgentContainer : MonoBehaviour
{
    public AgentClass myClass;

    private void Start()
    {
        gameObject.name = myClass.name;
    }
}
