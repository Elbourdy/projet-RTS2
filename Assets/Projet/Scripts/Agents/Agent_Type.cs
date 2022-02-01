using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*D�fini le type d'un agent aka Alli� ou Ennemi
 * Permet aux agents de connaitre leur camps pour ensuite d�finir les param�tres de chacun
 */
public class Agent_Type : MonoBehaviour
{
    // Enum de nos camps et variable public
    public enum TypeAgent {Ally, Enemy };
    public TypeAgent Type;

    // Pour d�finir la couleur de la barre de vie d'un camp
    public GameObject healthBar;

    // Pour savoir s'il s'agit d'une construction ou non. On ne veut pas d'un batiment dans notre multi-selection
    public bool isConstruction = false;
    

    private SelectionPlayer sp;
    private BatteryManager myBatteryManager;

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
        sp = GameObject.Find("GameManager").GetComponent<SelectionPlayer>();
        myBatteryManager = GameObject.Find("GameManager").GetComponent<BatteryManager>();

        // Ajout de l'unit� dans nos managers, permet d'utiliser la multi-s�lection et le syst�me de batterie
        AddToManager();
    }

    private void OnDisable()
    {
        RemoveFromManager();
    }

    private void AddToManager()
    {
        if (Type == TypeAgent.Ally && !isConstruction) sp.allFriendlyUnits.Add(gameObject);
        if (Type == TypeAgent.Ally && gameObject.name != "Nexus") myBatteryManager.batteries.Add(gameObject);
    }

    private void RemoveFromManager()
    {
        if (Type == TypeAgent.Ally && !isConstruction) sp.allFriendlyUnits.Remove(gameObject);
        sp.selectedUnits.Remove(gameObject);

        if (Type == TypeAgent.Ally && gameObject.name != "Nexus") myBatteryManager.batteries.Remove(gameObject);
    }
}
