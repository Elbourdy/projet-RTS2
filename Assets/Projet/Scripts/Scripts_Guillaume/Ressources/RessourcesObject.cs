using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourcesObject : MonoBehaviour
{
    public enum Ressources_Type { Ressource_1, Ressource_2, Ressource_3};
    public Ressources_Type ressourceType;

    [SerializeField] private float ressourceTimer = 1;
    [SerializeField] private float stockRessources = 400;


    public float GetTimer()
    {
        return ressourceTimer;
    }

    public float GetRessource (float ressourceToGet)
    {
        if (stockRessources <= 0)
        {
            return stockRessources;
        }
        stockRessources -= ressourceToGet;
        if (stockRessources <= 0)
        {
            Destroy(gameObject);
        }
        return ressourceToGet;
    }
}
