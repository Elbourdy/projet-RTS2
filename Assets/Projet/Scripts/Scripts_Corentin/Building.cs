using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    public Transform spawnPosition, rallyPoint;
    public GameObject selectedFeedback;
    public HealthBar productionBar, constructionBar, healthBar;
    public List<Image> recapProduction, UIClickable;
    public GameObject gameManager;
    public float constructionHealthMax;

    private List<DataStorage> roasterUnits = new List<DataStorage>();
    private List<DataStorage> productionQueue = new List<DataStorage>(); 
    private float actualTimer, timerCount;
    private bool isSelected, isConstructed;
    private float constructionHealthActual;

    public void AddToRoaster(int IDNumberRoaster)
    {
        List<DataStorage> fullRoaster = GameObject.Find("GameManager").GetComponent<GameDataStorage>().mainDataStorage;
        roasterUnits.Add(fullRoaster[IDNumberRoaster]);
    }

    public void RemoveFromRoaster(int IDNumberRoaster)  
    {
        foreach (DataStorage e in roasterUnits)
        {
            if (e.GetID() == IDNumberRoaster)
            {
                roasterUnits.Remove(e);
            }
        }
    }

    public void DisplayRoaster()
    {
        int i = 0;
        foreach (DataStorage e in roasterUnits)
        {
            UIClickable[i].sprite = e.GetSprite();
            UIClickable[i].GetComponent<UiSquareStorage>().SetIDNumber(e.GetID());
            i++;
        }

        for (int j = i; j < UIClickable.Count; j++)
        {
            UIClickable[j].gameObject.SetActive(false);
        }
    }

    public void AddToQueue(int IDNumberRoaster)
    {
        productionQueue.Add(roasterUnits[IDNumberRoaster]);
    }

    public void ProcessQueue()
    {
        if (actualTimer != 0)
            productionBar.SetHealth(timerCount / actualTimer);

        if (productionQueue.Count != 0)
        {
            if (timerCount == 0)
            {
                actualTimer = productionQueue[0].GetTimerCreation();
            }

            timerCount += Time.deltaTime;

            if (timerCount > actualTimer)
            {
                SpawnUnit();
                productionQueue.RemoveAt(0);
                timerCount = 0;
            }
        }
    }

    public void SetFeedbackUI ()
    {
        if (isSelected)
        {
            selectedFeedback.SetActive(true);

            if (isConstructed)
            {
                healthBar.gameObject.SetActive(true);
                recapProduction[0].transform.parent.gameObject.SetActive(true);
            }
            else
            {
                healthBar.gameObject.SetActive(false);
                productionBar.gameObject.SetActive(false);
            }
        }
        else
        {
            selectedFeedback.SetActive(false);
            healthBar.gameObject.SetActive(false);

            recapProduction[0].transform.parent.gameObject.SetActive(false);
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

    public void SpawnUnit()
    {
        GameObject instance = Instantiate(productionQueue[0].GetPrefab(), spawnPosition.position, Quaternion.identity);
    }

    public void SetOneRecapProduction(int IDInList, int numberUnitCurrentlyProduce, Sprite unitCurrentlyProduce, bool active)
    {
        recapProduction[IDInList].sprite = unitCurrentlyProduce;
        recapProduction[IDInList].transform.GetChild(0).GetComponent<Text>().text = numberUnitCurrentlyProduce.ToString();
        recapProduction[IDInList].transform.gameObject.SetActive(active);
    }

    public void SetRecapProductionTotal()
    {
        int i = 0;
        if (productionQueue.Count > 0)
        {
            DataStorage precedentElementChecked = productionQueue[0];
            int countUnit = 0, unitChecked = 0;

            foreach (DataStorage e in productionQueue)
            {
                unitChecked++;
                if (i < 5) // number of possible recap for now, you can add more but it's for test purposes
                {
                    if (precedentElementChecked == e)
                    {
                        countUnit++;
                    }
                    else
                    {
                        SetOneRecapProduction(i, countUnit, precedentElementChecked.GetSprite(), true);
                        precedentElementChecked = e;
                        countUnit = 1;
                        i++;
                    }
                }

                if (unitChecked == productionQueue.Count && i < 5)
                {
                    SetOneRecapProduction(i, countUnit, precedentElementChecked.GetSprite(), true);
                    i++;
                }
            }
        }

        for (int j = i; j < 5; j++)
        {
            SetOneRecapProduction(j, 0, null, false);
        }
    }

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

    public void SetSpawnPosition(Transform spawnPosition)
    {
        this.spawnPosition = spawnPosition;
    }

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

    public void SetRallyPoint (Transform rallyPoint)
    {
        this.rallyPoint = rallyPoint;
    }

    public Transform GetRallyPoint()
    {
        return rallyPoint;
    }

    public void SetUiClickable(List<Image> UIClickable)
    {
        this.UIClickable = UIClickable;
    }

    public float GetConstructionHealth()
    {
        return constructionHealthActual;
    }

    public void SetConstructionHealth(float constructionHealth)
    {
        constructionHealthActual = constructionHealth;
    }

    public void CheckIfSelected()
    {
        int i = 0;
        List<GameObject> tmp = gameManager.GetComponent<SelectionPlayer>().selectedUnits;

        foreach (GameObject e in tmp)
        {
            if (e.gameObject == gameObject)
            {
                i++;
            }
        }

        if (i > 1)
            isSelected = true;
        else
            isSelected = false;
    }
}