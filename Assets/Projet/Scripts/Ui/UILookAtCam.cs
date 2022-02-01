using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookAtCam : MonoBehaviour
{
    private Transform camToLook;


    private void OnEnable()
    {
        camToLook = GameObject.FindWithTag("MainCamera").transform;
    }

    private void Update()
    {
        
        transform.LookAt(camToLook);
    }
}
