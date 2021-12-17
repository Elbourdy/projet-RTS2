using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourcesObject : MonoBehaviour
{
    public enum Ressources_Type { Ressource_1, Ressource_2, Ressource_3};
    public Ressources_Type ressourceType;

    private int ressourceNmbr;

    [SerializeField] private int ressourceTimer = 1;
    [SerializeField] private int ressourceQuantityPerTic = 20;
    [SerializeField] private int stockRessources = 400;

    private void Awake()
    {
        SetIdRessource();
    }

    private void SetIdRessource()
    {
        switch (ressourceType)
        {
            case Ressources_Type.Ressource_1:
                ressourceNmbr = 0;
                break;
            case Ressources_Type.Ressource_2:
                ressourceNmbr = 1;
                break;
            case Ressources_Type.Ressource_3:
                ressourceNmbr = 2;
                break;
            default:
                break;
        }
    }

    public float GetTimer()
    {
        return ressourceTimer;
    }


    public int GetId()
    {
        return ressourceNmbr;
    }

    public int GetRessource ()
    {
        if (stockRessources <= 0)
        {
            return stockRessources;
        }
        stockRessources -= ressourceQuantityPerTic;
        if (stockRessources <= 0)
        {
            Destroy(gameObject);
        }
        return ressourceQuantityPerTic;
    }
}
