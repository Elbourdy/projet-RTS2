using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AfficheCoutEnergy : MonoBehaviour
{
    public int Valeur;
    public GameObject Manager;

    // Update is called once per frame
    void Update()
    {
        Valeur = Manager.GetComponent<BatteryManager>().energyConsumeByTick;
        gameObject.GetComponent<Text>().text = "Prochain Côut :" + Valeur;
    }
}
