using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global_Ressources : MonoBehaviour
{
    public static Global_Ressources instance;

    public void Awake()
    {
        instance = this;
    }

    //ne pas rentrer de valeurs dedans, il me sert just a déug le serializedfield
    [SerializeField] int[] ressources = new int[4];
        
    //sert a faire les check pour les couts d'unités et de batiments, renverra la quantité de la ressource X
    public int CheckRessources(int ressourceID)
    {
        return ressources[ressourceID];
    }

    //toute modification de ressource, quelle qu'elle soit, DOIT passer par ici!
    public void ModifyRessource(int ressourceID, int ressourceAmmount)
    {
        ressources[ressourceID] += ressourceAmmount;
        //actualiser l'UI ici!
    }

    public bool CheckIfEnoughRessources(int ressourceID, int ressourceAmount)
    {
        if (ressources[ressourceID] >= ressourceAmount)
            return true;
        else
            return false;
    }
}
