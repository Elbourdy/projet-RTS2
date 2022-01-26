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
    //public List<GameObject> ressourcesCases = new List<GameObject>();

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
        if (selectionPlayer.selectedUnits.Count > 0)
        {
            OneUnitDisplayUpdate();
            DisplayUnitSelectionCases();

            if (selectionPlayer.selectedUnits[0].GetComponent<ClassBatimentContainer>())
            {
                DisplayShopCasesForBuilding();
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
        oneUnitDisplay.SetActive(true);
        oneUnitDisplay.transform.GetChild(4).gameObject.SetActive(true);
        oneUnitDisplay.transform.GetChild(5).gameObject.SetActive(true);

        GameObject selectedUnit = selectionPlayer.selectedUnits[0];

        oneUnitDisplay.transform.GetChild(2).GetComponent<Text>().text = selectedUnit.GetComponent<HealthSystem>().GetHealth().ToString();
        oneUnitDisplay.transform.GetChild(3).GetComponent<Text>().text = selectedUnit.GetComponent<HealthSystem>().GetMaxHealth().ToString();
        oneUnitDisplay.transform.GetChild(4).GetComponent<HealthBar>().SetHealth(selectedUnit.GetComponent<HealthSystem>().GetHealth() / selectedUnit.GetComponent<HealthSystem>().GetMaxHealth());

        if (selectedUnit.GetComponent<ClassAgentContainer>() != null)
        {
            oneUnitDisplay.transform.GetChild(0).GetComponent<Text>().text = selectedUnit.GetComponent<ClassAgentContainer>().myClass.name;
            oneUnitDisplay.transform.GetChild(1).GetComponent<Image>().sprite = selectedUnit.GetComponent<ClassAgentContainer>().myClass.unitSprite;
            oneUnitDisplay.transform.GetChild(6).gameObject.SetActive(true);
            oneUnitDisplay.transform.GetChild(6).GetComponent<HealthBar>().SetHealth(selectedUnit.GetComponent<HealthSystem>().GetBatteryHealth()/selectedUnit.GetComponent<HealthSystem>().GetMaxBatteryHealth());
        }

        if (selectedUnit.GetComponent<ClassBatimentContainer>() != null)
        {
            oneUnitDisplay.transform.GetChild(0).GetComponent<Text>().text = selectedUnit.GetComponent<ClassBatimentContainer>().myClass.name;
            oneUnitDisplay.transform.GetChild(1).GetComponent<Image>().sprite = selectedUnit.GetComponent<ClassBatimentContainer>().myClass.unitSprite;
            oneUnitDisplay.transform.GetChild(6).GetComponent<HealthBar>().SetHealth(1);
        }
    }

    public void ResetOneUnitDisplay()    // fonction qui reset le grosse image pour une unité 
    {
        oneUnitDisplay.SetActive(false);
        oneUnitDisplay.transform.GetChild(0).GetComponent<Text>().text = "";
        oneUnitDisplay.transform.GetChild(1).GetComponent<Image>().sprite = null;
        oneUnitDisplay.transform.GetChild(2).GetComponent<Text>().text = "";
        oneUnitDisplay.transform.GetChild(3).GetComponent<Text>().text = "";
        oneUnitDisplay.transform.GetChild(4).gameObject.SetActive(false);
        oneUnitDisplay.transform.GetChild(5).gameObject.SetActive(false);
        oneUnitDisplay.transform.GetChild(6).gameObject.SetActive(false);
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
            /*else if (i == 9) //up and down nexus
            {
                shopCases[i].SetActive(true);
                if (HQBehavior.instance.currentNexusState == HQBehavior.statesNexus.Immobilize)
                {
                    shopCases[i].GetComponent<Image>().sprite = selectionPlayer.selectedUnits[0].GetComponent<ClassBatimentContainer>().myClass.upFreeSprite;
                    shopCases[i].transform.GetChild(0).GetComponent<Text>().text = "Ascend Nexus";
                }
                else if (HQBehavior.instance.currentNexusState == HQBehavior.statesNexus.Move)
                {
                    shopCases[i].GetComponent<Image>().sprite = selectionPlayer.selectedUnits[0].GetComponent<ClassBatimentContainer>().myClass.downSprite;
                    shopCases[i].transform.GetChild(0).GetComponent<Text>().text = "Descend Nexus";
                }
                else if (HQBehavior.instance.currentNexusState == HQBehavior.statesNexus.ForcedImmobilize)
                {
                    shopCases[i].GetComponent<Image>().sprite = selectionPlayer.selectedUnits[0].GetComponent<ClassBatimentContainer>().myClass.upLockedSprite;
                    shopCases[i].transform.GetChild(0).GetComponent<Text>().text = "Locked";
                }
            }*/
            else if (i == 10) // evolve nexus case 
            {
                shopCases[i].SetActive(true);
                shopCases[i].GetComponent<Image>().sprite = selectionPlayer.selectedUnits[0].GetComponent<ClassBatimentContainer>().myClass.upgradeSprite;
                shopCases[i].transform.GetChild(0).GetComponent<Text>().text = "Upgrade";
            }
            else if (i == 11)  // set rally point case
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
            if (i <= unitSelectionCases.Count) //on remplit les cases tant qu'il y a des unités sauf pour la première (elle sera dans le zoom)
            {
                if (e.GetComponent<ClassAgentContainer>())
                {
                    unitSelectionCases[i].GetComponent<Image>().sprite = e.GetComponent<ClassAgentContainer>().myClass.unitSprite;
                }

                if (e.GetComponent<ClassBatimentContainer>())
                {
                    unitSelectionCases[i].GetComponent<Image>().sprite = e.GetComponent<ClassBatimentContainer>().myClass.unitSprite;
                }

                unitSelectionCases[i].SetActive(true);
                unitSelectionCases[i].transform.GetChild(0).GetComponent<Text>().text = e.GetComponent<HealthSystem>().GetHealth().ToString();
            }
            i++;
        }

        for (int j = i; j < unitSelectionCases.Count; j++)  //on vide le reste des cases
        {
            if (j>0)
                unitSelectionCases[j].SetActive(false);
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
}
