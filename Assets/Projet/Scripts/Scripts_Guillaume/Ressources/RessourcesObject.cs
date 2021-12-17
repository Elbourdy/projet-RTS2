using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourcesObject : MonoBehaviour
{
    public bool canBeHarvestedByNexus = false;
    public enum Ressources_Type { Ressource_1, Ressource_2, Ressource_3};
    public Ressources_Type ressourceType;

    private int ressourceId;

    [SerializeField] private int ressourceTimer = 1;
    [SerializeField] private int ressourceQuantityPerTic = 20;
    [SerializeField] private int stockRessources = 400;

    //temp for testing with nexus
    private float timerCount;
    

    private void Awake()
    {
        SetIdRessource();
    }

    // temp just to test with the nexus directly draining ressources instead of workers
    public void Update()
    {
        if (canBeHarvestedByNexus)
        {
            RaycastHit[] hit = Physics.SphereCastAll(transform.position, 3f, transform.forward);

            foreach (RaycastHit e in hit)
            {
                if (e.transform.GetComponent<ClassBatimentContainer>())
                {
                    if (e.transform.GetComponent<ClassBatimentContainer>().name == "Nexus") ;
                    {
                        timerCount += Time.deltaTime;

                        if (timerCount >= ressourceTimer)
                        {
                            AddRessourceToPlayer();
                            timerCount = 0;
                        }
                    }
                }
            }
        }
    }



    private void SetIdRessource()
    {
        switch (ressourceType)
        {
            case Ressources_Type.Ressource_1:
                ressourceId = 0;
                break;
            case Ressources_Type.Ressource_2:
                ressourceId = 1;
                break;
            case Ressources_Type.Ressource_3:
                ressourceId = 2;
                break;
            default:
                break;
        }
    }

    public float GetTimer()
    {
        return ressourceTimer;
    }



    public void AddRessourceToPlayer()
    {
        if (stockRessources - ressourceQuantityPerTic <= 0)
        {
            Global_Ressources.instance.ModifyRessource(ressourceId, stockRessources);
        }

        else Global_Ressources.instance.ModifyRessource(ressourceId, ressourceQuantityPerTic);
        stockRessources -= ressourceQuantityPerTic;
        if (stockRessources <= 0)
        {
            Destroy(gameObject);
        }
        
    }
}
