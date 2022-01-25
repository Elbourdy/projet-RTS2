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

    public List<int> levelThresholdRessources = new List<int>();
    public List<float> vitesseNexus = new List<float>();
    public List<float> vitesseCollecte = new List<float>();
    public List<float> multiplicatorConsumption = new List<float>();
    public List<float> multiplicatorSpeedProd = new List<float>();

    [SerializeField] private List<Image> feedbackLevel = new List<Image>();
    [SerializeField] private Sprite levelOn, levelOff, levelTemp;
    [SerializeField] private HealthBar ressourceBar;
    [SerializeField] private GameObject ressourceText;

    [SerializeField] public int currentNexusLevel = 0;

    private int maxNexusLevel, newNexusLevel;
    [SerializeField] private float pityTimerCount;

    // Start is called before the first frame update
    void Start()
    {
        maxNexusLevel = levelThresholdRessources.Count - 1;

        currentNexusLevel = CheckNexusLevel();
    }

    // Update is called once per frame
    void Update()
    {
        newNexusLevel = CheckNexusLevel();

        if (newNexusLevel < currentNexusLevel)
        {
            pityTimerCount += Time.deltaTime;

            if (pityTimerCount > pityTimerLevel)
            {
                currentNexusLevel = newNexusLevel;
            }
        }
        else
        {
            currentNexusLevel = newNexusLevel;
            pityTimerCount = 0;
        }

        SetFeedbackLevelNexus();
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

    private void SetFeedbackLevelNexus()
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


    void RessourcesDisplay()
    {
        int ressourcesToLerp = 0, highBar = 0;

        if (newNexusLevel < 5)
        {
            ressourcesToLerp = Global_Ressources.instance.CheckRessources(0) -  levelThresholdRessources[newNexusLevel];
            Debug.Log(ressourcesToLerp);
            highBar = levelThresholdRessources[newNexusLevel + 1] - ((newNexusLevel == 0)? 0 : levelThresholdRessources[newNexusLevel]);

            ressourceBar.SetHealth(ressourcesToLerp / (highBar * 1f));
        }
        else
        {
            ressourceBar.SetHealth(1);
        }

        ressourceText.GetComponent<Text>().text = Global_Ressources.instance.CheckRessources(0).ToString();
    }
}
