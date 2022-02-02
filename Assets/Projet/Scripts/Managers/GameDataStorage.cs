using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataStorage : MonoBehaviour  //Plus tard automatiser le processus
{
    //Script qui stocke toutes les unit�s alli�es et ennemies spawnables
    //harmoniser la distinction gameObject/agentclass

    public static GameDataStorage instance;

    public void Awake()
    {
        instance = this;
    }

    public List<AgentClass> mainAgentClassStorage = new List<AgentClass>();
    public List<GameObject> tempEnnemiesAgentClassStorage = new List<GameObject>();
}
