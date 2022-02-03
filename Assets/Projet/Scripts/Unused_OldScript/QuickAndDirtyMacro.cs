using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickAndDirtyMacro : MonoBehaviour
{
    [SerializeField] GameObject[] batiments;
    [SerializeField] Global_Ressources ressources;
    [SerializeField] Building_PlacementAndValidation placer;
    int[,] buildingCosts = new int[4,4] ;
    int buildingSelectedID;
    private void Start()
    {
        //c'est d�geu, et c'est pour ca que l'h�ritage existe, mais comme rien n'est pour l'instant setup, ou alors j'ai pas trouv� :pepelaugth:
        buildingCosts[0, 0] = 100;
        buildingCosts[0, 1] = 0;
        buildingCosts[0, 2] = 0;
        buildingCosts[0, 3] = 0;
        buildingCosts[1, 0] = 125;
        buildingCosts[1, 1] = 25;
        buildingCosts[1, 2] = 0;
        buildingCosts[1, 3] = 0;
        buildingCosts[2, 0] = 100;
        buildingCosts[2, 1] = 0;
        buildingCosts[2, 2] = 100;
        buildingCosts[2, 3] = 100;
        buildingCosts[3, 0] = 75;
        buildingCosts[3, 1] = 25;
        buildingCosts[3, 2] = 25;
        buildingCosts[3, 3] = 25;
    }
    //la lateupdate est n�cessaire avec comment j'ai cod� le behavior de la construction
    private void LateUpdate()
    {
        //juste une s�lection rapide pour la d�monstration
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            buildingSelectedID = (buildingSelectedID + 1) % buildingCosts.GetLength(0);
            Debug.Log("le batiment s�lectionn� a chang�");
        }
            if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if (buildingSelectedID == 0) buildingSelectedID = buildingCosts.GetLength(0) - 1;
            else buildingSelectedID--;
            Debug.Log("le batiment s�lectionn� a chang�");
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            //le test de ressources
            bool ressourcesValidated = true;
            for (int i = 0; i < buildingCosts.GetLength(1); i++)
            {
                if (ressources.CheckRessources(i) < buildingCosts[buildingSelectedID, i]) { ressourcesValidated = false; Debug.LogError("Il manque " + (buildingCosts[buildingSelectedID, i] - ressources.CheckRessources(i)) + " de la ressource ID " + i); }
            }
            //si le test est valide on entre en mode cosntruction
            if (ressourcesValidated) placer.EnterBuildingPlacement(batiments[buildingSelectedID]);
        }
    }

    public void BuildingValidated()
    {
        for (int i = 0; i < buildingCosts.GetLength(1); i++)
        {
            ressources.ModifyRessource(i, -buildingCosts[buildingSelectedID, i]);
        }
    }

    //le getlegth(x) n'est utile que pour des tableau a plusieurs dimentions
}
