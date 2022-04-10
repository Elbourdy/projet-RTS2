using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Agent", menuName = "ScriptableObjects/Unit/Agent")]
public class AgentClass : UnitClass
{
    public enum AgentJob { Worker, Soldier};
    public AgentJob Job;




    public float timerCreation;
    public GameObject unitPrefab;
    public Sprite unitSprite;

    public float attackDamage;
    public float rangeAttaque;
    public float rateOfFire;
    public float movementSpeed;
    public float radiusVision = 5;

    public int[] ressourcesCost  = new int[]{50, 0, 0};
    public int spawnerCost = 0;


    
    public enum AgentSpe
    {
        None, Tank, Artillery, Scout
    };
    [Header("Attack Spéciale")]
    public AgentSpe mySpe;

    #region Tank
    [DrawIf("mySpe", AgentSpe.Tank)] public GameObject areaToSpawn;
    [DrawIf("mySpe", AgentSpe.Tank)] public float damageTank;
    [DrawIf("mySpe", AgentSpe.Tank)] public float cooldownAttack;

    #endregion


    #region Artillery
    [DrawIf("mySpe", AgentSpe.Artillery)] public GameObject poisonArea;
    [DrawIf("mySpe", AgentSpe.Artillery)] public float damagePoison;
    [DrawIf("mySpe", AgentSpe.Artillery)] public float cooldownAttackPoison;
    [DrawIf("mySpe", AgentSpe.Artillery)] public float speedTick;

    #endregion

}
