using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Global_Ressources : MonoBehaviour
{
    public static Global_Ressources instance;

    public void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        CheckIfOverflowRessources();
    }

    //ne pas rentrer de valeurs dedans, il me sert just a déug le serializedfield
    [SerializeField] int[] ressources = new int[4];
    [SerializeField] int[] maxRessources = new int[4];
        
    //sert a faire les check pour les couts d'unités et de batiments, renverra la quantité de la ressource X
    public int CheckRessources(int ressourceID)
    {
        return ressources[ressourceID];
    }

    public int CheckRessourcesMax(int ressourceID)
    {
        return maxRessources[ressourceID];
    }

    //toute modification de ressource, quelle qu'elle soit, DOIT passer par ici!
    public void ModifyRessource(int ressourceID, int ressourceAmmount)
    {
        ressources[ressourceID] += ressourceAmmount;
        if (ressources[0] <= 0) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //actualiser l'UI ici!
    }

    public void ModifyRessourceMax(int ressourceID, int ressourceAmmount)
    {
        maxRessources[ressourceID] = ressourceAmmount;
        //actualiser l'UI ici!
    }

    public bool CheckIfEnoughRessources(int ressourceID, int ressourceAmount)
    {
        if (ressources[ressourceID] >= ressourceAmount)
            return true;
        else
            return false;
    }

    public void CheckIfOverflowRessources()
    {
        int i = 0;
        foreach (int e in ressources)
        {
            if (e > maxRessources[i])
                ressources[i] = maxRessources[i];
        }
    }
}
