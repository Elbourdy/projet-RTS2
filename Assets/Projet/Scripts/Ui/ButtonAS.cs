using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonAS : MonoBehaviour
{
    public int ID;
    //private SelectionPlayer selectionPlayer;
    private NewSelectionManager selectionManager;

    void Start()
    {
        //selectionPlayer = GameObject.Find("GameManager").GetComponent<SelectionPlayer>();
        selectionManager = NewSelectionManager.instance;
    }

    public void ButtonPress()
    {
        if (selectionManager.SelectedObjects[0].GetComponent<ClassBatimentContainer>())
        {
            if (selectionManager.SelectedObjects[0].GetComponent<Building>().SetActionShopCases(ID))
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Action/UI_Act_Click/UI_Act_Click");
            }
            else
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Action/UI_Act_NotPossible/UI_Act_Not");
            }
            //lance une fonction dans le batiment avec l'ID du bouton pressé entrainânt l'action correspondante
        }

        if (selectionManager.SelectedObjects[0].GetComponent<ClassAgentContainer>())
        {
            //Here call the function which need to be wrote by guillaume which will made the selected units acts
        }
    }
}
