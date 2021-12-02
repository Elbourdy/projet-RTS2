using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Damien Saenz

public class Gestion_HUD : MonoBehaviour
{
    // Variables de Teste
    public string RCS1, RCS2, RCS3;
    public float Cases;
    public float Perso;
    public float Map;
    public float vie;
    public GameObject Test;
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
        if (GameObject.Find("GameManager").GetComponent<SelectionPlayer>().selectedUnits.Count > 0)
        {
            Test = GameObject.Find("GameManager").GetComponent<SelectionPlayer>().selectedUnits[0];


            if (Test.GetComponent<AgentStates>())
            {
                   Perso = Test.GetComponent<AgentStates>().TypeUnit;
                   Cases = Test.GetComponent<AgentStates>().NbrCase;
                   vie = Test.GetComponent<HealthSystem>().GetHealth();
                   GameObject.Find("Vie").GetComponent<Text>().text = "" + vie;
            }
            else if(Test.GetComponent<HQBehavior>())
            {
                Perso = Test.GetComponent<HQBehavior>().BatType;
                Cases = Test.GetComponent<HQBehavior > ().nbrcase;
            }
            
        }
        else
        {
            Perso = 0;
            Cases = 0;
            GameObject.Find("Vie").GetComponent<Text>().text = "";
        }

    }
}
