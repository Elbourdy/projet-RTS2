using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NexusLevelManager : MonoBehaviour
{
    public static NexusLevelManager instance;

    public void Awake()
    {
        instance = this;
    }

    [SerializeField] private float pityTimerLevel = 2f;

    [SerializeField] private List<int> levelThresholdRessources = new List<int>();
    [SerializeField] private List<float> vitesseNexus = new List<float>();
    [SerializeField] private List<float> vitesseCollecte = new List<float>();
    [SerializeField] private List<float> multiplicatorConsumption = new List<float>();
    [SerializeField] private List<float> multiplicatorSpeedProd = new List<float>();
    [SerializeField] private List<float> rangeNexusMultiplier = new List<float>();
    [SerializeField] private List<Material> materialNexusLevel = new List<Material>();
    [SerializeField] private List<float> animationSpeedNexus = new List<float>();

    [SerializeField] private List<Image> feedbackLevel = new List<Image>();
    [SerializeField] private Sprite levelOn, levelOff, levelTemp;
    [SerializeField] private HealthBar ressourceBar;
    [SerializeField] private GameObject ressourceText;

    [SerializeField] public int currentNexusLevel = 0;

    private int maxNexusLevel, newNexusLevel;
    [SerializeField] private float pityTimerCount;

    private bool stopSound = false;
    private float timerStopSound = 6, timerStopSoundCount = 0;
    FMOD.Studio.EventInstance soundNexusLevelChange;

    // Start is called before the first frame update
    void Start()
    {
        maxNexusLevel = levelThresholdRessources.Count - 1;

        currentNexusLevel = CheckNexusLevel();

        SetFeedbackNexusLevel(materialNexusLevel[currentNexusLevel], animationSpeedNexus[currentNexusLevel]);

        soundNexusLevelChange = FMODUnity.RuntimeManager.CreateInstance("event:/Building/Build_Nexus/Build_Nex_Level/Build_Nex_LvL_Up/Build_Nex_LvL_Up");
    }

    // Update is called once per frame
    void Update()
    {
        /*if (stopSound)
        {
            timerStopSoundCount += Time.deltaTime;

            if (timerStopSoundCount > timerStopSound)
            {
                soundNexusLevelChange.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                stopSound = false;
                timerStopSoundCount = 0;
                Debug.Log("StopSound");
            }
        }*/

        newNexusLevel = CheckNexusLevel();

        if (newNexusLevel < currentNexusLevel)
        {
            pityTimerCount += Time.deltaTime;

            if (pityTimerCount > pityTimerLevel)
            {
                currentNexusLevel = newNexusLevel;
                SetFeedbackNexusLevel(materialNexusLevel[currentNexusLevel], animationSpeedNexus[currentNexusLevel]);

                soundNexusLevelChange.setParameterByName("Nex_Level_Up", (currentNexusLevel + 1) * 2);
                soundNexusLevelChange.start();
                stopSound = true;
                pityTimerCount = 0;
            }
        }
        else if (newNexusLevel > currentNexusLevel)
        {
            currentNexusLevel = newNexusLevel;
            SetFeedbackNexusLevel(materialNexusLevel[currentNexusLevel], animationSpeedNexus[currentNexusLevel]);
            
            soundNexusLevelChange.setParameterByName("Nex_Level_Up", currentNexusLevel * 2 - 1);
            soundNexusLevelChange.start();

            stopSound = true;
            pityTimerCount = 0;
        }

        SetFeedbackLevelNexusPoint();
        RessourcesDisplay();
    }

    public int CheckNexusLevel()
    {
        int ressourcesAmount = Global_Ressources.instance.CheckRessources(0);

        for (int i = maxNexusLevel; i >= 0; i--)
        {
            if (ressourcesAmount > levelThresholdRessources[i])
            {
                return i;
            }
        }
        return 0;
    }

    public float GetVitesseNexus()
    {
        return vitesseNexus[currentNexusLevel];
    }

    public float GetVitesseCollecte()
    {
        return vitesseCollecte[currentNexusLevel];
    }

    public float GetMultiplicatorConsomption()
    {
        return multiplicatorConsumption[currentNexusLevel];
    }

    public float GetMultiplicatorSpeedProd()
    {
        return multiplicatorSpeedProd[currentNexusLevel];
    }

    public float GetMultiplicatorRangeNexus()
    {
        return rangeNexusMultiplier[currentNexusLevel];
    }

    private void SetFeedbackLevelNexusPoint()
    {
        for (int i = 0; i < maxNexusLevel; i++)
        {
            if (i < currentNexusLevel && i < newNexusLevel)
                feedbackLevel[i].sprite = levelOn;
            else if (i < currentNexusLevel && i >= newNexusLevel && newNexusLevel != currentNexusLevel)
                feedbackLevel[i].sprite = levelTemp;
            else
                feedbackLevel[i].sprite = levelOff;
        }
    }


    private void RessourcesDisplay()
    {
        int ressourcesToLerp = 0, highBar = 0;

        if (newNexusLevel < 5)
        {
            ressourcesToLerp = Global_Ressources.instance.CheckRessources(0) -  levelThresholdRessources[newNexusLevel];
            highBar = levelThresholdRessources[newNexusLevel + 1] - ((newNexusLevel == 0)? 0 : levelThresholdRessources[newNexusLevel]);

            ressourceBar.SetHealth(ressourcesToLerp / (highBar * 1f));
        }
        else
        {
            ressourceBar.SetHealth(1);
        }

        ressourceText.GetComponent<Text>().text = Global_Ressources.instance.CheckRessources(0).ToString()+"/" + ((currentNexusLevel < 5)? levelThresholdRessources[currentNexusLevel + 1] : 3000);
    }

    private void SetFeedbackNexusLevel(Material newMaterial, float speedAnimation)
    {
        HQBehavior.instance.SetNexusMaterial(newMaterial);
        HQBehavior.instance.SetIdleAnimationSpeed(speedAnimation);
        BatteryManager.instance.SetLineRendererMaterial(newMaterial);
    }
}
