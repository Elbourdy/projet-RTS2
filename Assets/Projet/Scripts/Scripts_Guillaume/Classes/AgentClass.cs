using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Agent", menuName = "ScriptableObjects/Unit/Agent")]
public class AgentClass : UnitClass
{
    public enum AgentJob { Worker, Soldier};
    public AgentJob Job;
}
