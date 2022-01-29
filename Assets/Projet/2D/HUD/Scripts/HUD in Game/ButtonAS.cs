using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonAS : MonoBehaviour
{
    public int ID;
    private SelectionPlayer selectionPlayer;

    void Start()
    {
        selectionPlayer = GameObject.Find("GameManager").GetComponent<SelectionPlayer>();
    }

    public void ButtonPress()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Action/UI_Act_Click/UI_Act_Click");

        if (selectionPlayer.selectedUnits[0].GetComponent<ClassBatimentContainer>())
        {
            selectionPlayer.selectedUnits[0].GetComponent<Building>().SetActionShopCases(ID); 
            //lance une fonction dans le batiment avec l'ID du bouton pressé entrainânt l'action correspondante
        }

        if (selectionPlayer.selectedUnits[0].GetComponent<ClassAgentContainer>())
        {
            //Here call the function which need to be wrote by guillaume which will made the selected units acts
        }
    }
}
