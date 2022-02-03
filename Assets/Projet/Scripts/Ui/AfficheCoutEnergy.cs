using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AfficheCoutEnergy : MonoBehaviour
{
    private int valeur;

    // Update is called once per frame
    void Update()
    {
        valeur = BatteryManager.instance.energyConsumeByTick;
        gameObject.GetComponent<Text>().text = "-"+valeur.ToString();
    }
}
