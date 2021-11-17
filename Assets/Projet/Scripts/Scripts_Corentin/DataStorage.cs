using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataStorage
{
    private float timerCreation;
    private GameObject unitPrefab;
    private Sprite unitSprite;
    private int ID;
    // private float ressources cost (A, B, C, X)

    public DataStorage(float timerCreation, GameObject unitPrefab, Sprite unitSprite, int ID)
    {
        this.timerCreation = timerCreation;
        this.unitPrefab = unitPrefab;
        this.unitSprite = unitSprite;
        this.ID = ID;
    }

    public int GetID()
    {
        return ID;
    }

    public float GetTimerCreation()
    {
        return timerCreation;
    }

    public GameObject GetPrefab()
    {
        return unitPrefab;
    }

    public Sprite GetSprite()
    {
        return unitSprite;
    }
}

//plus tard séparer la classe DATA storage en deux clases pour les technologies et les unités
