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

    public enum SpeAttackType
    {
        TankAgressive,
        Artillery,
        Scout
    };



    [System.Serializable]
    public struct SpeAttackValues
    {
        public SpeAttackType mySpe;
        public float attackDamage;
        public float attackRange;
        public float cooldownAttack;
    };

    public SpeAttackValues myValues;


    [Header("Ne pas toucher")]
    // TANK VALUES
    public GameObject attackTank;
    public float distCone = 1f;
    public float attackRange = 1f;
    public float attackDamage = 1f;

    public GameObject poisonArea;
    public float distArea;

    public float timerInvisibility = 5f;



    private void InitTank()
    {

    }

    private void InitArtillery()
    {

    }


    private void InitScout()
    {

    }


    private void OnEnable()
    {
        if (myAgentState == null) myAgentState = GetComponent<AgentStates>();
        myAgentState.onSpeAttack += LaunchSpeAttack;
        myAgentState.canSpeAttack = true;
        attackDamage = myValues.attackDamage;
        attackRange = myValues.attackRange;
        myAgentState.ChangeAttackValue(attackRange, attackDamage);

        if (myValues.mySpe == SpeAttackType.Scout)
        {
            myHs.onDamaged += ScoutAttack;
            myAgentState.canSpeAttack = false;
        }
    }

    private void OnDisable()
    {
        myAgentState.onSpeAttack -= LaunchSpeAttack;

        myHs.onDamaged -= ScoutAttack;
    }

    public void LaunchSpeAttack()
    {
        switch (myValues.mySpe)
        {
            case SpeAttackType.TankAgressive:
                TankAttack();
                break;
            case SpeAttackType.Artillery:
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
        myAgentState.ChangeAttackValue(myAgentState.container.myClass.rangeAttaque, myAgentState.container.myClass.attackDamage);
    }




    private IEnumerator TimerSpeAttack()
    {
        myAgentState.canSpeAttack = false;
        yield return new WaitForSeconds(myValues.cooldownAttack);
        if (myValues.mySpe != SpeAttackType.Scout) myAgentState.ChangeAttackValue(myValues.attackRange, myValues.attackDamage);
        myAgentState.canSpeAttack = true;
    }




    public void SpawnAttackCone()
    {
        var pos = transform.position + transform.forward * distCone;
        var go = GameObject.Instantiate(attackTank, pos, transform.rotation) as GameObject;
        go.GetComponent<DamageOnContact>().typeToDamage = GetComponent<AIAgents>().typeToTarget;
        go.GetComponent<DamageOnContact>().damage = attackDamage;
    }

    private void ArtilleryAttack()
    {
        SpawnPoisonArea();
        StartCoroutine(TimerSpeAttack());
        myAgentState.ChangeAttackValue(myAgentState.container.myClass.rangeAttaque, myAgentState.container.myClass.attackDamage);
    }

    public void SpawnPoisonArea()
    {
        var pos = transform.position + transform.forward * distArea;
        var go = GameObject.Instantiate(poisonArea, pos, Quaternion.identity) as GameObject;
        go.GetComponent<ContinuousDamageOnContact>().typeToDamage = GetComponent<AIAgents>().typeToTarget;
        go.GetComponent<ContinuousDamageOnContact>().damage = attackDamage;
    }





    public void ScoutAttack()
    {
        SetInvisibility();
        StartCoroutine(TimerSpeAttack());
    }

    private void SetInvisibility()
    {
        myAgentType.SetIsTargetable(false);
        StartCoroutine(TimerInvisbility());
    }

    private IEnumerator TimerInvisbility()
    {
        yield return new WaitForSeconds(timerInvisibility);
        myAgentType.SetIsTargetable(true);
    }
}
