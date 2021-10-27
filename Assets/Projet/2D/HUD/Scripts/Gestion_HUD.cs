using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Damien Saenz

public class Gestion_HUD : MonoBehaviour
{
    // Variables de Teste
    public string RCS1, RCS2, RCS3;
    public int Cases;
    public int Perso;
    public float Map;
    private GameObject RC1, RC2, RC3;


    void Start()
    {   // Aquisition des object texte
        RC1 = GameObject.Find("Ressource1");
        RC2 = GameObject.Find("Ressource2");
        RC3 = GameObject.Find("Ressource3");
    }

    
    void Update()
    {   //Incrémentation des ObjetsText
        RC1.GetComponent<Text>().text = RCS1;
        RC2.GetComponent<Text>().text = RCS2;
        RC3.GetComponent<Text>().text = RCS3;


    }
}
