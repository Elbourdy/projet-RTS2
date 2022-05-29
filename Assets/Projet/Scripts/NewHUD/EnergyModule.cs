using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyModule : MonoBehaviour
{
    public static EnergyModule instance;

    private void Awake()
    {
        instance = this;
    }

    public Image[] levelList;
    public Image consoBar, energyBar;
    public Text energyText, consoText;
    public float rangeMinConso = 0.1f, rangeMaxConso = 0.9f;
    public float levelTwoThreshold = 0.345f, levelThreeThreshold = 0.815f;
    private float energyLevelTwo, energyLevelThree;
    public Color[] colorLevel;
    public AnimationCurve levelFeedbackCurve;
    public float animationLevelTime = 2f;
    public float animationLevelTimeCount = 2f;
    public int currentLevelNexus;
    public bool rise = false;

    private void Start()
    {
        animationLevelTimeCount = animationLevelTime;
        energyLevelTwo = NexusLevelManager.instance.levelThresholdRessources[1];
        energyLevelThree = NexusLevelManager.instance.levelThresholdRessources[2];

        levelList[0].color = colorLevel[3];
        levelList[1].color = colorLevel[1];
        levelList[2].color = colorLevel[2];
    }

    private void Update()
    {
        UpdateEnergyBar(Global_Ressources.instance.CheckRessources(0));

        if (animationLevelTimeCount < animationLevelTime)
        {
            UpdateLevelNexus();
            animationLevelTimeCount += Time.deltaTime;
        }
    }

    private void UpdateEnergyBar(float energy)
    {
        float fillValue = 0;

        if (energy < energyLevelTwo)
        {
            fillValue = ((energy * 1.0f) / energyLevelTwo) * levelTwoThreshold;
        }

        else if (energy < energyLevelThree)
        {
            fillValue = levelTwoThreshold + ((energy * 1.0f - energyLevelTwo) / (energyLevelThree - energyLevelTwo)) * (levelThreeThreshold - levelTwoThreshold);
        }

        else if (energy > energyLevelThree)
        {
            fillValue = levelThreeThreshold + ((energy * 1.0f - energyLevelThree) / (1000) * (1 - levelThreeThreshold));
        }

        energyBar.fillAmount = fillValue;
        energyText.text = energy.ToString();
    }

    public void UpdateConsoBar(float consoLerp, float consoValue)
    {
        float fillValue = (consoLerp * (rangeMaxConso - rangeMinConso) + rangeMinConso);
        consoBar.fillAmount = fillValue;
        consoText.text = consoValue.ToString();
    }

    public void InitialiseLevelNexus(int currentLevel, bool rise)
    {
        currentLevelNexus = currentLevel;
        this.rise = rise;
        animationLevelTimeCount = 0;
    }

    private void UpdateLevelNexus()
    {
        if (rise)
        {
            float r = levelFeedbackCurve.Evaluate(animationLevelTimeCount / animationLevelTime);
            levelList[currentLevelNexus].color = Color.Lerp(colorLevel[currentLevelNexus], colorLevel[currentLevelNexus + 3], r);
        }
        else
        {
            float r = levelFeedbackCurve.Evaluate(1 - animationLevelTimeCount / animationLevelTime);
            levelList[currentLevelNexus].color = Color.Lerp(colorLevel[currentLevelNexus], colorLevel[currentLevelNexus + 3], r);
        }
    }
}
