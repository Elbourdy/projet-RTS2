using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    //classe de base des batiments, est devenue avec le temps la classe poubelle des fonctions du nexus
    //Filtrage de cette classe prévue avec des enums/sous-classes pour acceder à certaines fonctions en fonction du type de batiment

    [Header("Roster and Unit production")]
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private GameObject rallyPoint;
    [SerializeField] private HealthBar productionBar; 
    [SerializeField] private List<Image> recapProduction;
    [SerializeField] private List<AgentClass> roasterUnits = new List<AgentClass>();
    [SerializeField] public float refundPercentageUnit = 0.5f;

    private List<AgentClass> productionQueue = new List<AgentClass>();
    private List<int> prodQueueStacked = new List<int>();
    private float actualTimer, timerCount;

    [Header("Health and Construction")]
    [SerializeField] private HealthBar constructionBar;
    [SerializeField] private HealthBar HealthBarhealthBar;
    [SerializeField] private float constructionHealthMax;


    [Header("Feedback Audio and Visual")]
    [SerializeField] private GameObject selectedFeedback;

    private bool isSelected, isConstructed, isMovingRallyPoint = false;
    public float constructionHealthActual;

    
    //PRODUCTION D'UNITE
    public void AddToRoaster(int IDNumberRoaster) // ajoute une unité au roster du batiment (unité que le joueur peut créer dans ce batiment)
    {
        List<AgentClass> fullRoaster = GameDataStorage.instance.mainAgentClassStorage;
        roasterUnits.Add(fullRoaster[IDNumberRoaster]);
    } 

    public bool AddToQueue(int IDNumberRoaster) // ajoute une unité à la file d'attente de création
    {
        int i = 0;
        bool check = true;

        foreach (int e in roasterUnits[IDNumberRoaster].ressourcesCost)
        {
            if (!Global_Ressources.instance.CheckIfEnoughRessources(i, roasterUnits[IDNumberRoaster].ressourcesCost[i]))
                check = false;
            i++;
        }

        if (check)
        {
            i = 0;
            foreach (int e in roasterUnits[IDNumberRoaster].ressourcesCost)
            {
                Global_Ressources.instance.ModifyRessource(i, -roasterUnits[IDNumberRoaster].ressourcesCost[i]);
                i++;
            }
            productionQueue.Add(roasterUnits[IDNumberRoaster]);

            return true;
        }
        else
            return false;
    }  

    public void RemoveFromQueue(int buttonPressed) // enlève une unité de la file d'attente 
    {
        Global_Ressources.instance.ModifyRessource(0, productionQueue[buttonPressed].ressourcesCost[0]);
        productionQueue.RemoveAt(prodQueueStacked[buttonPressed] - 1);
    }  

    public void ProcessQueue() // gère la file d'attente de création du batiment 
    {
        if (actualTimer != 0)
            productionBar.SetHealth(timerCount / actualTimer);

        if (productionQueue.Count != 0)
        {
            if (timerCount == 0)
            {
                actualTimer = productionQueue[0].timerCreation;
            }

            timerCount += Time.deltaTime * NexusLevelManager.instance.GetMultiplicatorSpeedProd();

            if (timerCount > actualTimer)
            {
                SpawnUnit();
                productionQueue.RemoveAt(0);
                timerCount = 0;
            }
        }
        else
        {
            timerCount = 0;
        }
    }

    public void SpawnUnit()  // fait apparaître l'unité produite
    {
        GameObject instance = Instantiate(productionQueue[0].unitPrefab, spawnPosition.position, Quaternion.identity);
        instance.GetComponent<AgentStates>().SetState(AgentStates.states.Follow);
        instance.GetComponent<AgentStates>().MoveAgent(rallyPoint.transform.position);
        instance.GetComponent<AgentStates>().SetRestDest(rallyPoint.transform.position);
        if (instance.GetComponent<OnCreateSound>()) instance.GetComponent<OnCreateSound>().LaunchSoundCreation();
    }


    //GESTION INPUT SHOPCASES
    public bool SetActionShopCases (int ButtonID) // fait une action en fonction des boutons appuyés dans la case shop du HUD (spawn unit, setrallypoint, etc)
    {
        if (ButtonID < roasterUnits.Count)
        {
            return AddToQueue(ButtonID);
        }

        if (ButtonID == 9)
        {
            if (HQBehavior.instance.currentNexusState == HQBehavior.statesNexus.Move)
            {
                HQBehavior.instance.currentNexusState = HQBehavior.statesNexus.Immobilize;
            }
            else if (HQBehavior.instance.currentNexusState == HQBehavior.statesNexus.Immobilize)
            {
                HQBehavior.instance.currentNexusState = HQBehavior.statesNexus.Move;
            }
            return true;
        }

        if (ButtonID == 11)
        {
            SelectionPlayer.instance.canSelect = false;
            isMovingRallyPoint = true;
            return true;
        }

        return false;
    }


    //FEEDBACKS VISUELS ET SONORE
    public void SetFeedbackUI () // gère l'apparition des differents élements du HUD du batiment sur la scène
    {
        if (isSelected)
        {
            selectedFeedback.SetActive(true);

            //healthBar.gameObject.SetActive(true);
            recapProduction[0].transform.parent.gameObject.SetActive(true);
            rallyPoint.SetActive(true);
        }
        else
        {
            selectedFeedback.SetActive(false);
            //healthBar.gameObject.SetActive(false);
            //rallyPoint.SetActive(false);

            //recapProduction[0].transform.parent.gameObject.SetActive(false);
        }

        if (productionQueue.Count > 0)
        {
            productionBar.gameObject.SetActive(true);
        }
        else
        {
            productionBar.gameObject.SetActive(false);
        }

        SetRecapProductionTotal();
    }

    public void SetSelectedFeedback(GameObject selectedFeedback)
    {
        this.selectedFeedback = selectedFeedback;
    }

    public void SetOneRecapProduction(int IDInList, int numberUnitCurrentlyProduce, Sprite unitCurrentlyProduce, bool active)
    {
        recapProduction[IDInList].sprite = unitCurrentlyProduce;
        recapProduction[IDInList].transform.GetChild(0).GetComponent<Text>().text = numberUnitCurrentlyProduce.ToString();
        recapProduction[IDInList].transform.gameObject.SetActive(active);
    }

    public void SetRecapProductionTotal()
    {
        prodQueueStacked.Clear();

        int i = 0;
        if (productionQueue.Count > 0)
        {
            AgentClass precedentElementChecked = productionQueue[0];
            int countUnit = 0, unitChecked = 0;

            foreach (AgentClass e in productionQueue)
            {
                unitChecked++;
                if (i < recapProduction.Count) // number of possible recap for now, you can add more but it's for test purposes
                {
                    if (precedentElementChecked == e)
                    {
                        countUnit++;
                    }
                    else
                    {
                        if (prodQueueStacked.Count > 0)
                            prodQueueStacked.Add(countUnit + prodQueueStacked[prodQueueStacked.Count - 1]);
                        else
                            prodQueueStacked.Add(countUnit);

                        SetOneRecapProduction(i, countUnit, precedentElementChecked.unitSprite, true);
                        precedentElementChecked = e;
                        countUnit = 1;
                        i++;
                    }
                }

                if (unitChecked == productionQueue.Count && i < recapProduction.Count)
                {
                    if (prodQueueStacked.Count > 0)
                        prodQueueStacked.Add(countUnit + prodQueueStacked[prodQueueStacked.Count - 1]);
                    else
                        prodQueueStacked.Add(countUnit);

                    SetOneRecapProduction(i, countUnit, precedentElementChecked.unitSprite, true);
                    i++;
                }
            }
        }

        for (int j = i; j < recapProduction.Count; j++)
        {
            SetOneRecapProduction(j, 0, null, false);
        }
    }


    //CONSTRUCTION DES BATIMENTS *inactif pour le moment*
    public void CheckIfBuildingConstructed()
    {
        if (constructionHealthActual >= constructionHealthMax)
        {
            isConstructed = true;
            constructionBar.gameObject.SetActive(false);
        }
        UpdateConstructionBar();
    }

    public void UpdateConstructionBar()
    {
        constructionBar.SetHealth(constructionHealthActual / constructionHealthMax);
    }

    
    //SELECTION
    public void CheckIfSelected() // vérifie si le nexus est dans la selction 
    {
        int i = 0;
        List<GameObject> tmp = SelectionPlayer.instance.selectedUnits;

        foreach (GameObject e in tmp)
        {
            if (e.gameObject == gameObject)
            {
                i++;
            }
        }

        if (i > 0)
        {
            isSelected = true;
        }
        else
        {
            isSelected = false;
        }
    }


    //GET/SET Divers
    public void SetIsSelected(bool choice)
    {
        isSelected = choice;
    }

    public bool GetIsSelected()
    {
        return isSelected;
    }

    public void SetIsConstructed(bool choice)
    {
        isConstructed = choice;
    }

    public bool GetIsConstructed()
    {
        return isConstructed;
    }

    public void SetProductionBar(HealthBar productionBar)
    {
        this.productionBar = productionBar;
    }

    public void SetProductionRecap(List<Image> productionRecap)
    {
        this.recapProduction = productionRecap;
    }

    public void SetRallyPoint (Vector3 rallyPoint)
    {
        this.rallyPoint.transform.position = rallyPoint;
    }

    public GameObject GetRallyPoint()
    {
        return rallyPoint;
    }

    public float GetConstructionHealth()
    {
        return constructionHealthActual;
    }

    public void SetConstructionHealth(float constructionHealth)
    {
        constructionHealthActual = constructionHealth;
    }

    public List<AgentClass> GetRoasterUnits()
    {
        return roasterUnits;
    }

    public void SetIsMovingRallyPoint(bool choice)
    {
        isMovingRallyPoint = choice;
    }

    public bool GetIsMovingRallyPoint()
    {
        return isMovingRallyPoint;
    }

    public void SetSpawnPosition(Transform spawnPosition)
    {
        this.spawnPosition = spawnPosition;
    }



    /*public void SetNexusLevel(int level)
    {
        nexusLevel = level;
    }

    public int GetNexusLevel()
    {
        return nexusLevel;
    }

    public void EvolveNexus()
    {
        if (nexusLevel < ressourcesMax.Length)
        {
            if (Global_Ressources.instance.CheckIfEnoughRessources(0, ressourcesToEvolve[nexusLevel]))
            {
                nexusLevel++;

                Global_Ressources.instance.ModifyRessource(0, -ressourcesToEvolve[nexusLevel]);

                Global_Ressources.instance.ModifyRessourceMax(0, ressourcesMax[nexusLevel]); 
            }
        }
    }*/
}