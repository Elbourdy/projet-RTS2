using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowingRecepter : MonoBehaviour
{
    [SerializeField] private Light myLight;
    [SerializeField] private float speedIntensity = 1f;

    private float intensityMax;

    private bool activateBehaviour = false;

    private void Start()
    {
        if (myLight == null) GetComponent<Light>();
        intensityMax = myLight.intensity;
        myLight.intensity = 0f;
    }

    private void Update()
    {
        if (activateBehaviour)
        {
            LerpLight();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger !!!");
        activateBehaviour = true;
    }

    private void LerpLight()
    {
        myLight.intensity += speedIntensity * Time.deltaTime;
        if (myLight.intensity >= intensityMax) myLight.intensity = intensityMax;
    }
}
