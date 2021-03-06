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
    private int ResMaxValue;
    private float ResMaxValuePASTOUCHE;
    private float  remainingEnergyFloat;
    public bool playSound = true;
    private bool isReload = false;
    private float crono = 0;
    public float TempsReload;
    public int ajout;

    [SerializeField] private float tickPerSeconds = 8;
    [SerializeField] private int ressourceQuantityPerTic = 1;
    [SerializeField] private int stockRessources = 400;
    [SerializeField] private int rangeCollection = 20;
    [SerializeField] private float onReadDistance = 0f;
    [SerializeField] private float timeRessourcesToStartReload = 20f;

    private Transform nexusCenter;

    public List<Renderer> crystalRessourcesRenderer = new List<Renderer>();
    public Material chargeCrystal, discargeCrystal;
    private bool playSoundReload = true;

    //temp for testing with nexus
    private float timerCount, timeRessourcesToStartReloadCount;

    [SerializeField] private HealthBar ressourcesBar;

    FMOD.Studio.EventInstance soundRessourceSuckLoop;
    private string soundReloadFinish = "event:/Crystals/Cryst_Recharge";


    /// <NewVFXSystem>
    [SerializeField] private GameObject[] newVFX;

    private void Awake()
    {
        SetIdRessource();
        foreach (var item in newVFX)
        {
            item.SetActive(false);
        }

    }

    [SerializeField] private float onRead;

    public void Start()
    {
        ResMaxValue = stockRessources;
        ResMaxValuePASTOUCHE = stockRessources;
        nexus = GameObject.Find("Nexus");
        lR = GetComponent<LineRenderer>();
        nexusCenter = GameObject.Find("Icosphere_Nexus").transform;


        soundRessourceSuckLoop = FMODUnity.RuntimeManager.CreateInstance("event:/Crystals/Cryst_OnCollect");
        soundRessourceSuckLoop.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
    }

    public void Update()
    {
        timeRessourcesToStartReloadCount += Time.deltaTime;

        remainingEnergyFloat = stockRessources / ResMaxValuePASTOUCHE;
        ressourcesBar.SetHealth(remainingEnergyFloat);

        if (stockRessources > 0)
        {
            //onReadDistance = Vector3.Distance(transform.position, nexus.transform.position);
            if (Vector3.Distance(transform.position, nexus.transform.position) < rangeCollection)
            {
                SetFeedbackNexusCollecting();
                timerCount += Time.deltaTime * NexusLevelManager.instance.GetVitesseCollecte();

                if (timerCount >= 1/tickPerSeconds)
                {
                    AddRessourceToPlayer();
                    timerCount = 0;
                    timeRessourcesToStartReloadCount = 0;
                }
            }
            else
            {
                DisableFeedbackCollectionNexus();
            }
        }
        else
        {
            DisableFeedbackCollectionNexus();
        }

        if (timeRessourcesToStartReloadCount > timeRessourcesToStartReload)
        {
            crono += Time.deltaTime;
            if (crono >= TempsReload)
            {
                stockRessources = stockRessources + ajout;
                crono = 0;

                if (playSoundReload && CalculateRemainingTimeRefill() <= 3)
                {
                    FMODUnity.RuntimeManager.PlayOneShot(soundReloadFinish, transform.position);
                    playSoundReload = false;
                }
            }
            if (stockRessources >= ResMaxValue)
            { 
                stockRessources = ResMaxValue;
                isReload = false;
                crono = 0;
                playSoundReload = true;
            }
        }
        SetFeedbackRessourcesCrystal();
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
        return tickPerSeconds;
    }



    public void AddRessourceToPlayer()
    {
        if (stockRessources - ressourceQuantityPerTic <= 0)
        {
            Global_Ressources.instance.ModifyRessource(ressourceId, stockRessources);
        }
        else Global_Ressources.instance.ModifyRessource(ressourceId, ressourceQuantityPerTic);

        stockRessources -= ressourceQuantityPerTic;
    }
    
       
    public void SetFeedbackNexusCollecting()
    {
        //OldVisualFeedbackOn();
        NewVisualFeedbackOn();
        if (playSound)
        {
            playSound = false;
            soundRessourceSuckLoop.setParameterByName("Suck_Stop", 0);
            soundRessourceSuckLoop.start();
        }

        soundRessourceSuckLoop.setParameterByName("Crystal_Fill_Energy", (1 - remainingEnergyFloat));
    }

    public void DisableFeedbackCollectionNexus()
    {
        //OldVisualFeedbackOff();
        NewVisualFeedbackOff();
        if (!playSound)
        {
            soundRessourceSuckLoop.setParameterByName("Suck_Stop", 1);
        }
        playSound = true;
    }

    public void SetFeedbackRessourcesCrystal()
    {
        foreach (Renderer e in crystalRessourcesRenderer)
        {
            e.material.Lerp(discargeCrystal, chargeCrystal, remainingEnergyFloat);
        }
    }

    public float CalculateRemainingTimeRefill()
    {
        float remainingFilling = ResMaxValue - stockRessources;

        float timeToRefill = (remainingFilling * TempsReload) / ajout;

        return timeToRefill;
    }

    public float GetCurrentRessource()
    {
        return stockRessources;
    }

    public float GetRatioRessources()
    {
        return remainingEnergyFloat;
    }

    private void OldVisualFeedbackOn()
    {
        lR.enabled = true;
        lR.SetPosition(0, transform.position);
        lR.SetPosition(1, nexusCenter.position);
    }

    private void OldVisualFeedbackOff()
    {
        lR.enabled = false;
    }

    private void NewVisualFeedbackOn()
    {
        foreach (var item in newVFX)
        {
            item.SetActive(true);
        }
    }

    private void NewVisualFeedbackOff()
    {
        foreach (var item in newVFX)
        {
            item.SetActive(false);
        }
    }

}
