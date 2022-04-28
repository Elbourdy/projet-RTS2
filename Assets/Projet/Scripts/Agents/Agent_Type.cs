using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*Défini le type d'un agent aka Allié ou Ennemi
 * Permet aux agents de connaitre leur camps pour ensuite définir les paramètres de chacun
 */
public class Agent_Type : MonoBehaviour
{
    // Enum de nos camps et variable public
    public enum TypeAgent {Ally, Enemy };
    public TypeAgent Type;

    // Pour définir la couleur de la barre de vie d'un camp
    public GameObject healthBar;

    // Pour savoir s'il s'agit d'une construction ou non. On ne veut pas d'un batiment dans notre multi-selection
    public bool isConstruction = false;
    

    private BatteryManager myBatteryManager;

    private bool isTargetable = true;

    public void Start()
    {
        // Mise en place de la couleur des barres de vie
        if(Type == TypeAgent.Ally)
            {
            healthBar.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
        }
        if (Type == TypeAgent.Enemy)
        {
            healthBar.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
        }
    }

    private void OnEnable()
    {
        myBatteryManager = GameObject.Find("GameManager").GetComponent<BatteryManager>();

        // Ajout de l'unité dans nos managers, permet d'utiliser la multi-sélection et le système de batterie
        AddToManager();
    }

    private void OnDisable()
    {
        RemoveFromManager();
    }

    private void AddToManager()
    {
        if (Type == TypeAgent.Ally && gameObject.name != "Nexus") myBatteryManager.batteries.Add(gameObject);
    }

    private void RemoveFromManager()
    {

        if (Type == TypeAgent.Ally && gameObject.name != "Nexus") myBatteryManager.batteries.Remove(gameObject);
    }


    public  bool GetIsTargetable()
    {
        return isTargetable;
    }

    public void SetIsTargetable(bool _newBool)
    {
        isTargetable = _newBool;
    }
}
