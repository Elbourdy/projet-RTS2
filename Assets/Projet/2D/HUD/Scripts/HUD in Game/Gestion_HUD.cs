using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Damien Saenz

public class Gestion_HUD : MonoBehaviour
{
    public float Map;

    public GameObject oneUnitDisplay;   //grosse image zoom sur une unité
    private SelectionPlayer selectionPlayer;      ///script de selection de perso c'est plus rapide vu que je l'appelle pelin de fois
    public List<GameObject> shopCases = new List<GameObject>();   ///liste des petites cases d'actions
    public List<GameObject> unitSelectionCases = new List<GameObject>();     //liste des cases qui montrent les personnages selectionnés
    public List<GameObject> ressourcesCases = new List<GameObject>();

    void Start()
    {
        selectionPlayer = GameObject.Find("GameManager").GetComponent<SelectionPlayer>();

        for (int i = 0; i < shopCases.Count; i++)
        {
            shopCases[i].GetComponent<ButtonAS>().ID = i;
        }
    }

    
    void Update()
    {
        RessourcesDisplay();
        if (selectionPlayer.selectedUnits.Count > 0)
        {
            OneUnitDisplayUpdate();
            DisplayUnitSelectionCases();

            if (selectionPlayer.selectedUnits[0].GetComponent<ClassBatimentContainer>())
            {
                if (selectionPlayer.selectedUnits[0].GetComponent<Building>().GetIsConstructed()) ///temporary fix for when the building is not constructed, it does not display roaster units nor prevent updates of display
                    DisplayShopCasesForBuilding();
                else
                    ResetShopCases();
            }

            if (selectionPlayer.selectedUnits[0].GetComponent<ClassAgentContainer>())
            {
                ResetShopCases(); 
                //Here add the functions for displaying selection for unit and suppress the line above
            }
        }
        else
        {
            ResetOneUnitDisplay();
            ResetShopCases();
            ResetUnitSelectionCases();
        }
    }

    public void OneUnitDisplayUpdate()   //fonction qui affiche la grosse image pour une unité
    {
        GameObject selectedUnit = selectionPlayer.selectedUnits[0];

        if (selectedUnit.GetComponent<ClassAgentContainer>() != null)
        {
            oneUnitDisplay.GetComponent<Image>().sprite = selectedUnit.GetComponent<ClassAgentContainer>().myClass.unitSprite;
            oneUnitDisplay.transform.GetChild(0).GetComponent<Text>().text = selectedUnit.GetComponent<HealthSystem>().GetHealth().ToString();
        }

        if (selectedUnit.GetComponent<ClassBatimentContainer>() != null)
        {
            oneUnitDisplay.GetComponent<Image>().sprite = selectedUnit.GetComponent<ClassBatimentContainer>().myClass.unitSprite;
            oneUnitDisplay.transform.GetChild(0).GetComponent<Text>().text = selectedUnit.GetComponent<HealthSystem>().GetHealth().ToString();
        }
    }

    public void ResetOneUnitDisplay()    /// fonction qui reset le grosse image pour une unité
    {
        oneUnitDisplay.GetComponent<Image>().sprite = null;
        oneUnitDisplay.transform.GetChild(0).GetComponent<Text>().text = "";
    }

    void DisplayShopCasesForBuilding()   //fonction qui remplit le contenu des shop cases pour les batiments (sera changé quand on ajoutera des fonctions speciales
    {
        List<AgentClass> roasterUnits = new List<AgentClass>();
        roasterUnits = selectionPlayer.selectedUnits[0].GetComponent<Building>().GetRoasterUnits(); //récuperation des unités qui peuvent être créee par le batiment selectionné

        for (int i = 0; i < 12; i++)
        {
            if (i < roasterUnits.Count)
            {
                shopCases[i].SetActive(true);
                shopCases[i].GetComponent<Image>().sprite = roasterUnits[i].unitSprite;
                shopCases[i].transform.GetChild(0).GetComponent<Text>().text = roasterUnits[i].name;
            }
            else if (i == 11)
            {
                shopCases[i].SetActive(true);
                shopCases[i].GetComponent<Image>().sprite = selectionPlayer.selectedUnits[0].GetComponent<ClassBatimentContainer>().myClass.rallyPointSprite;
                shopCases[i].transform.GetChild(0).GetComponent<Text>().text = "Rally Point";
            }
            else
            {
                shopCases[i].SetActive(false);
            }
        }
    }

    void ResetShopCases()   //fonction qui reset le contenu des shop cases
    {
        foreach (GameObject e in shopCases)
        {
            e.SetActive(false);
            e.GetComponent<Image>().sprite = null;
            e.transform.GetChild(0).GetComponent<Text>().text = "";
        }
    }

    void DisplayUnitSelectionCases()     //Fonction qui affiche le contenu des petites cases de selection d'unités, à changer si on veut afficher plusieurs batiments à lka fois
    {
        int i = 0;
        foreach(GameObject e in selectionPlayer.selectedUnits)
        {
            if (i <= unitSelectionCases.Count && i > 0) //on remplit les cases tant qu'il y a des unités sauf pour la première (elle sera dans le zoom)
            {
                unitSelectionCases[i-1].SetActive(true);
                unitSelectionCases[i-1].GetComponent<Image>().sprite = e.GetComponent<ClassAgentContainer>().myClass.unitSprite;
                unitSelectionCases[i-1].transform.GetChild(0).GetComponent<Text>().text = e.GetComponent<HealthSystem>().GetHealth().ToString();
            }
            i++;
        }

        for (int j = i; j <= unitSelectionCases.Count; j++)  //on vide le reste des cases
        {
            if (j>0)
                unitSelectionCases[j - 1].SetActive(false);
        }
    }

    void ResetUnitSelectionCases()   //Fonction qui reset le contenu des petites cases de selection d'unités
    {
        foreach(GameObject e in unitSelectionCases)
        {
            e.SetActive(false);
            e.GetComponent<Image>().sprite = null;
            e.transform.GetChild(0).GetComponent<Text>().text = "";
        }
    }

    void RessourcesDisplay()
    {
        int i = 0;
        foreach(GameObject e in ressourcesCases)
        {
            e.GetComponent<Text>().text = Global_Ressources.instance.CheckRessources(i).ToString();
            i++;
        }
    }
}
