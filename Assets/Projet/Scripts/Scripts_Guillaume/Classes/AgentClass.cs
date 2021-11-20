using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Agent", menuName = "ScriptableObjects/Unit/Agent")]
public class AgentClass : UnitClass
{
    public enum AgentJob { Worker, Soldier};
    public AgentJob Job;

    
    

    public float attackDamage;
    public float rateOfFire;
    public float movementSpeed;


    [Header("Stats Worker")]
    [DrawIf("Job", AgentJob.Worker)] public float constructionSpeed = 1;
    [DrawIf("Job", AgentJob.Worker)] public float rangeConstruction = 1;
    [DrawIf("Job", AgentJob.Worker)] public float healthToConstruction = 1;

}
