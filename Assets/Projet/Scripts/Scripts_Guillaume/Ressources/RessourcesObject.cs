using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourcesObject : MonoBehaviour
{
    private GameObject nexus;
    private LineRenderer lR;
    public bool canBeHarvestedByNexus = false;
    public enum Ressources_Type { Ressource_1, Ressource_2, Ressource_3 };
    public Ressources_Type ressourceType;

    private int ressourceId;
    public float ResMaxValue; 
    public float  ValeurRestante;
    

    public Material Plein, vide;

    [SerializeField] private float ressourceTimer = 1;
    [SerializeField] private int ressourceQuantityPerTic = 20;
    [SerializeField] private int stockRessources = 400;
    [SerializeField] private int rangeCollection = 20;
    [SerializeField] private float onReadDistance = 0f;

    //temp for testing with nexus
    private float timerCount;


    private void Awake()
    {
        SetIdRessource();
    }


    public void Start()
    {
        ResMaxValue = stockRessources;
        nexus = GameObject.Find("Nexus");
        lR = GetComponent<LineRenderer>();
    }
    // temp just to test with the nexus directly draining ressources instead of workers
    public void Update()
    {
          ValeurRestante = stockRessources / ResMaxValue;
        if (canBeHarvestedByNexus)
        {
            onReadDistance = Vector3.Distance(transform.position, nexus.transform.position);
            if (Vector3.Distance(transform.position, nexus.transform.position) < rangeCollection)
            {
                SetFeedbackNexusCollecting();
                timerCount += Time.deltaTime;

                if (timerCount >= ressourceTimer)
                {
                    AddRessourceToPlayer();
                    timerCount = 0;
                }
            }
            else
            {
                DisableFeedbackCollectionNexus();
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

    public void SetFeedbackNexusCollecting()
    {
        lR.enabled = true;
        lR.SetPosition(0, transform.position);
        lR.SetPosition(1, nexus.transform.position);
    }

    public void DisableFeedbackCollectionNexus()
    {
        lR.enabled = false;
    }






}
