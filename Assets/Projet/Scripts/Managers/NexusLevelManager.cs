using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NexusLevelManager : MonoBehaviour
{
    //script qui gère le niveau du nexus et change ses variables en fonction
    public static NexusLevelManager instance;

    public void Awake()
    {
        instance = this;
    }

    [Header("Nexus Niveau Changement")]
    [SerializeField] public List<int> levelThresholdRessources = new List<int>();
    [SerializeField] private List<float> vitesseNexus = new List<float>();
    [SerializeField] private List<float> multiplicatorConsumption = new List<float>();
    
    [SerializeField] private List<float> rangeNexusMultiplier = new List<float>();
    [SerializeField] private List<Material> materialNexusLevel = new List<Material>();
    [SerializeField] private List<float> animationSpeedNexus = new List<float>();
    [SerializeField] private float pityTimerLevel = 2f;

    [Header("Nexus Niveau Feedback")]
    [SerializeField] private List<Image> feedbackLevel = new List<Image>();
    [SerializeField] private Sprite levelOn, levelOff, levelTemp;
    [SerializeField] private HealthBar ressourceBar;
    [SerializeField] private GameObject ressourceText;

    [Header("OnReadOnly")]
    [SerializeField] public int currentNexusLevel = 0;
    [SerializeField] private float pityTimerCount;

    [Header("Don't touch")]
    [SerializeField] private List<float> multiplicatorSpeedProd = new List<float>();
    [SerializeField] private List<float> vitesseCollecte = new List<float>();

    private int maxNexusLevel, newNexusLevel;
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
        newNexusLevel = CheckNexusLevel();

        if (newNexusLevel < currentNexusLevel)
        {
            EnergyModule.instance.InitialiseLevelNexus(newNexusLevel+1, false);
            pityTimerCount += Time.deltaTime;

            if (pityTimerCount > pityTimerLevel) //le nexus conserve son niveau quelque temps après une baisse
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
            EnergyModule.instance.InitialiseLevelNexus(newNexusLevel, true);
            currentNexusLevel = newNexusLevel;
            SetFeedbackNexusLevel(materialNexusLevel[currentNexusLevel], animationSpeedNexus[currentNexusLevel]);
            
            soundNexusLevelChange.setParameterByName("Nex_Level_Up", currentNexusLevel * 2 - 1);
            soundNexusLevelChange.start();

            stopSound = true;
            pityTimerCount = 0;
        }
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

    private void SetFeedbackNexusLevel(Material newMaterial, float speedAnimation)
    {
        HQBehavior.instance.SetNexusMaterial(newMaterial);
        HQBehavior.instance.SetIdleAnimationSpeed(speedAnimation);
        BatteryManager.instance.SetLineRendererMaterial(newMaterial);
    }
}
