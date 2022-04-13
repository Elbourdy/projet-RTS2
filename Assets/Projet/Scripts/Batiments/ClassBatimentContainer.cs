using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassBatimentContainer : MonoBehaviour
{
    public BatimentClass myClass;

    private void Start()
    {
        gameObject.name = myClass.nameAgent;
    }
}
