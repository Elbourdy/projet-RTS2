using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataStorage : MonoBehaviour  //Plus tard automatiser le processus
{
    public static GameDataStorage instance;

    public void Awake()
    {
        instance = this;
    }

    public List<AgentClass> mainAgentClassStorage = new List<AgentClass>();
    public List<GameObject> tempEnnemiesAgentClassStorage = new List<GameObject>();
}
