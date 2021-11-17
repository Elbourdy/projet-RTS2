using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    private List<DataStorage> roasterUnits = new List<DataStorage>();
    private List<DataStorage> productionQueue = new List<DataStorage>();
    private List<Image> recapProduction;
    private float actualTimer, timerCount;
    private Vector3 spawnPosition, rallyPoint;
    private GameObject selectedFeedback;
    private HealthBar productionBar;
    private bool isSelected;

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

    public void SetSelectedFeedbackActive(bool choice)
    {
        selectedFeedback.SetActive(choice);
        productionBar.transform.parent.gameObject.SetActive(choice);
    }

    public void SetSelectedFeedback(GameObject selectedFeedback)
    {
        this.selectedFeedback = selectedFeedback;
    }

    public void SpawnUnit()
    {
        GameObject instance = Instantiate(productionQueue[0].GetPrefab(), spawnPosition, Quaternion.identity);
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

    public void SetSpawnPosition(Vector3 spawnPosition)
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
        this.rallyPoint = rallyPoint;
    }

    public Vector3 GetRallyPoint()
    {
        return rallyPoint;
    }
}