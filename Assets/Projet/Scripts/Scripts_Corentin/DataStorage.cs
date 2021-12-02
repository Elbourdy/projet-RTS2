using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "ScriptableObjects/Unit")]
public class DataStorage : ScriptableObject
{
    public float timerCreation;
    public GameObject unitPrefab;
    public Sprite unitSprite;
    public   int ID;
    // private float ressources cost (A, B, C, X)

    //declaration de classe plus trop necessaire pour le moment

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
