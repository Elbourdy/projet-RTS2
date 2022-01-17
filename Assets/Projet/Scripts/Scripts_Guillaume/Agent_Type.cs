using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Agent_Type : MonoBehaviour
{
    public enum TypeAgent {Ally, Enemy };
    public TypeAgent Type;
    public GameObject Bare;
    public Material Vert, Rouge;
    public bool isConstruction = false;
    

    private SelectionPlayer sp;

    public void Start()
    {
        if(Type == TypeAgent.Ally)
            {
            Bare.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
        }
        if (Type == TypeAgent.Enemy)
        {
            Bare.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
        }
    }

    private void OnEnable()
    {
        sp = GameObject.Find("GameManager").GetComponent<SelectionPlayer>();
        if (Type == TypeAgent.Ally && !isConstruction) sp.allFriendlyUnits.Add(gameObject);

    }

    private void OnDisable()
    {
        if (Type == TypeAgent.Ally && !isConstruction) sp.allFriendlyUnits.Remove(gameObject);
        sp.selectedUnits.Remove(gameObject);
    }
}
