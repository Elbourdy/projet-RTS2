using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Agent_Type : MonoBehaviour
{
    public enum TypeAgent {Ally, Enemy };
    public TypeAgent Type;
    public GameObject healthBar;
    public bool isConstruction = false;
    

    private SelectionPlayer sp;
    private BatteryManager myBatteryManager;

    public void Start()
    {
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
        if (Type == TypeAgent.Ally && !isConstruction) sp.allFriendlyUnits.Add(gameObject);
        if (Type == TypeAgent.Ally && gameObject.name != "Nexus") myBatteryManager.batteries.Add(gameObject);
    }

    private void OnDisable()
    {
        if (Type == TypeAgent.Ally && !isConstruction) sp.allFriendlyUnits.Remove(gameObject);
        sp.selectedUnits.Remove(gameObject);

        if (Type == TypeAgent.Ally && gameObject.name != "Nexus") myBatteryManager.batteries.Remove(gameObject);
    }
}
