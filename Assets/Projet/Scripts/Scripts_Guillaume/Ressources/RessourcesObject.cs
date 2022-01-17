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
    public int ResMaxValue;
    public float ResMaxValuePASTOUCHE;
    public float  ValeurRestante;
    public bool playSound = true;
    public bool IsReload = false;
    public float crono = 0;
    public float TempsReload;
    public int ajout;


    public Material Plein, vide;

    [SerializeField] private float ressourceTimer = 1;
    [SerializeField] private int ressourceQuantityPerTic = 20;
    [SerializeField] private int stockRessources = 400;
    [SerializeField] private int rangeCollection = 20;
    [SerializeField] private float onReadDistance = 0f;

    //temp for testing with nexus
    private float timerCount;

    FMOD.Studio.EventInstance soundRessourceSuckLoop;
    private string soundRessourceSuckOneShotStart, soundRessourceSuckOneShotStop;

    private void Awake()
    {
        SetIdRessource();
    }


    public void Start()
    {
        ResMaxValue = stockRessources;
        ResMaxValuePASTOUCHE = stockRessources;
        nexus = GameObject.Find("Nexus");
        lR = GetComponent<LineRenderer>();

        soundRessourceSuckLoop = FMODUnity.RuntimeManager.CreateInstance("event:/Building/Build_Nexus/Build_Nex_Collect/Build_Nex_Collect");
        soundRessourceSuckLoop.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));

        //soundRessourceSuckOneShotStart = "event:/Fire/Fire_Spawn/Fire_Spawn";

        //soundRessourceSuckOneShotStop = "event:/Fire/Fire_Spawn/Fire_Spawn";
    }
    // temp just to test with the nexus directly draining ressources instead of workers
    public void Update()
    {
          ValeurRestante = stockRessources / ResMaxValuePASTOUCHE; ;
        if (canBeHarvestedByNexus)
        {
            onReadDistance = Vector3.Distance(transform.position, nexus.transform.position);
            if (Vector3.Distance(transform.position, nexus.transform.position) < rangeCollection)
            {
                if (playSound)
                {
                    //FMODUnity.RuntimeManager.PlayOneShot(soundRessourceSuckOneShotStart, transform.position);
                    soundRessourceSuckLoop.start();
                    Debug.Log("StartPlaying");
                }
                playSound = false;
                soundRessourceSuckLoop.setParameterByName("Crystal_Fill_Energy", (1 - ValeurRestante));

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
        


        if (IsReload == true)
        {
            crono += Time.deltaTime;
            if (crono >= TempsReload)
            {
                stockRessources = stockRessources + ajout;
                crono = 0;
            }
            if (stockRessources >= ResMaxValue)
            {
                stockRessources = ResMaxValue;
                IsReload = false;
                crono = 0;
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

        if (IsReload == false)
        {

       
        if (stockRessources - ressourceQuantityPerTic <= 0)
        {
            Global_Ressources.instance.ModifyRessource(ressourceId, stockRessources);
        }

        else Global_Ressources.instance.ModifyRessource(ressourceId, ressourceQuantityPerTic);
        stockRessources -= ressourceQuantityPerTic;
        if (stockRessources <= 0)
        {
            soundRessourceSuckLoop.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            Debug.Log("StopPlaying");
            IsReload = true;
                crono = 0;
            
           

            
        }

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

        if (!playSound)
        {
            //FMODUnity.RuntimeManager.PlayOneShot(soundRessourceSuckOneShotStop, transform.position);
            soundRessourceSuckLoop.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            Debug.Log("StopPlaying");
        }
        playSound = true;
    }






}
