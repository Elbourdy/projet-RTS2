using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] public int currentNexusLevel = 0;

    private int maxNexusLevel;
    [SerializeField] private float pityTimerCount;

    // Start is called before the first frame update
    void Start()
    {
        maxNexusLevel = levelThresholdRessources.Count;

        currentNexusLevel = CheckNexusLevel();
    }

    // Update is called once per frame
    void Update()
    {
        int newNexusLevel = CheckNexusLevel();

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
    }

    public int CheckNexusLevel()
    {
        int ressourcesAmount = Global_Ressources.instance.CheckRessources(0);

        for (int i = maxNexusLevel - 1; i >= 0; i--)
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
}
