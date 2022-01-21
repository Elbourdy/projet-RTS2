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

    public List<int> levelThresholdRessources = new List<int>();
    public List<float> vitesseNexus = new List<float>();
    public List<float> vitesseCollecte = new List<float>();
    public List<float> multiplicatorCunsumption = new List<float>();
    public List<float> multiplicatorVitesseProd = new List<float>();

    private int maxNexusLevel;
    public int currentNexusLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        maxNexusLevel = levelThresholdRessources.Count;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public int CheckNexusLevel(int n)
    {
        switch (n)
        {
            case n > levelThresholdRessources[4]:
                break;
        }
    }*/
}
