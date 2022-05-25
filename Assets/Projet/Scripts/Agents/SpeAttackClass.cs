using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentStates))]
public class SpeAttackClass : MonoBehaviour
{

    [SerializeField] private AgentStates myAgentState;
    [SerializeField] private ClassAgentContainer myContainer;
    [SerializeField] private Agent_Type myAgentType;
    [SerializeField] private HealthSystem myHs;


    // Type d'attaque
    private AgentClass.AgentSpe agentSpe;

    private float cooldownAttack;
    private float timerInvisibility;


    public float speAttackRange = 1f;
    public float speAttackDamage = 1f;
    public GameObject attackToSpawn;
    private float distAttack;

    private float timeRemaining = 20;

    // myContainer.myClass
    private void InitTank()
    {
        speAttackDamage = myContainer.myClass.damageTank;
        speAttackRange = myContainer.myClass.attackRangeTank;
        cooldownAttack = myContainer.myClass.cooldownAttackTank;
        attackToSpawn = myContainer.myClass.tankAttackGo;
        distAttack = myContainer.myClass.distanceSpawnTankAttack;
        myAgentState.ChangeAttackValue(speAttackRange, speAttackDamage);

    }

    private void InitArtillery()
    {
        speAttackDamage = myContainer.myClass.damagePoison;
        speAttackRange = myContainer.myClass.attackRangePoison;
        cooldownAttack = myContainer.myClass.cooldownAttackPoison;
        attackToSpawn = myContainer.myClass.poisonArea;
        distAttack = myContainer.myClass.distanceSpawnPoison;
        myAgentState.ChangeAttackValue(speAttackRange, speAttackDamage);
    }


    private void InitScout()
    {
        cooldownAttack = myContainer.myClass.cooldownInvisibility + timerInvisibility;
        timerInvisibility = myContainer.myClass.timerInvisibility;
    }


    private void InitValues()
    {
        if (myAgentState == null) myAgentState = GetComponent<AgentStates>();
        if (myContainer == null) myContainer = GetComponent<ClassAgentContainer>();
        agentSpe = myContainer.myClass.mySpe;
        myAgentState.canSpeAttack = true;
        switch (agentSpe)
        {
            case AgentClass.AgentSpe.None:
                break;
            case AgentClass.AgentSpe.Tank:
                InitTank();
                myAgentState.onSpeAttack += LaunchSpeAttack;
                break;
            case AgentClass.AgentSpe.Artillery:
                InitArtillery();
                myAgentState.onSpeAttack += LaunchSpeAttack;
                break;
            case AgentClass.AgentSpe.Scout:
                InitScout();
                myHs.onDamaged += ScoutAttack;
                myAgentState.onAttack += DeactivateInvisibilitySooner;
                break;
            default:
                break;
        }
    }


    private void OnEnable()
    {
        InitValues();
    }

    private void OnDisable()
    {
        if (agentSpe != AgentClass.AgentSpe.Scout) myAgentState.onSpeAttack -= LaunchSpeAttack;
        else
        {
            myHs.onDamaged -= ScoutAttack;
            myAgentState.onAttack -= DeactivateInvisibilitySooner;
        }
    }

    private void Update()
    {
        timeRemaining += Time.deltaTime;
    }

    public void LaunchSpeAttack()
    {
        switch (agentSpe)
        {
            case AgentClass.AgentSpe.Tank:
                TankAttack();
                break;
            case AgentClass.AgentSpe.Artillery:
                ArtilleryAttack();
                break;
            default:
                break;
        }
    }

    



    private void TankAttack()
    {
        SpawnAttackCone();
        StartCoroutine(TimerSpeAttack());
        myAgentState.ChangeAttackValue(myContainer.myClass.rangeAttaque, myContainer.myClass.attackDamage);
    }




    private IEnumerator TimerSpeAttack()
    {
        myAgentState.canSpeAttack = false;
        yield return new WaitForSeconds(cooldownAttack);
        if (agentSpe != AgentClass.AgentSpe.Scout) myAgentState.ChangeAttackValue(speAttackRange, speAttackDamage);
        myAgentState.canSpeAttack = true;
    }

   


    public void SpawnAttackCone()
    {
        var pos = transform.position + transform.forward * distAttack;
        var go = GameObject.Instantiate(attackToSpawn, pos, transform.rotation) as GameObject;
        go.GetComponent<DamageOnContact>().typeToDamage = GetComponent<AIAgents>().typeToTarget;
        go.GetComponent<DamageOnContact>().damage = speAttackDamage;
    }

    private void ArtilleryAttack()
    {
        SpawnPoisonArea();
        StartCoroutine(TimerSpeAttack());
        myAgentState.ChangeAttackValue(myContainer.myClass.rangeAttaque, myContainer.myClass.attackDamage);
        timeRemaining = 0;
    }

    public void SpawnPoisonArea()
    {
        var pos = transform.position + transform.forward * distAttack;
        var go = GameObject.Instantiate(attackToSpawn, pos, Quaternion.identity) as GameObject;
        go.GetComponent<ContinuousDamageOnContact>().typeToDamage = GetComponent<AIAgents>().typeToTarget;
        go.GetComponent<ContinuousDamageOnContact>().damage = speAttackDamage;
    }





    public void ScoutAttack()
    {
        if (myAgentState.canSpeAttack)
        {
            SetInvisibility();
            StartCoroutine(TimerSpeAttack());
        }
    }

    private void SetInvisibility()
    {
        myAgentType.SetIsTargetable(false);
        SetAIandTarget(false);
        StartCoroutine(TimerInvisbility());
    }


    private void SetAIandTarget(bool _bool)
    {
        if(!_bool) myAgentState.SetTarget(null);
        GetComponent<AIAgents>().canSearch = _bool;
    }

    private IEnumerator TimerInvisbility()
    {
        yield return new WaitForSeconds(timerInvisibility);
        myAgentType.SetIsTargetable(true);
        SetAIandTarget(true);
    }

    private void DeactivateInvisibilitySooner()
    {
        if (!myAgentType.GetIsTargetable())
        {
            StopCoroutine(TimerInvisbility());
            myAgentType.SetIsTargetable(true);
            SetAIandTarget(true);
        }
    }

    public float GetRemainingTime()
    {
        return timeRemaining / cooldownAttack;
    }
}
