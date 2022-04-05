using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentStates))]
public class SpeAttackClass : MonoBehaviour
{

    [SerializeField] private AgentStates myAgentState;



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
    };

    public SpeAttackValues myValues;


    [Header("Ne pas toucher")]
    public GameObject attackTank;
    public float distCone = 1f;
    public float attackRange = 1f;
    public float attackDamage = 1f;

    private void OnEnable()
    {
        if (myAgentState == null) GetComponent<AgentStates>();
        myAgentState.onSpeAttack += LaunchSpeAttack;
        myAgentState.canSpeAttack = true;
        attackDamage = myValues.attackDamage;
        attackRange = myValues.attackRange;
        myAgentState.ChangeAttackValue(attackRange, attackDamage);
    }

    private void OnDisable()
    {
        myAgentState.onSpeAttack -= LaunchSpeAttack;
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
            case SpeAttackType.Scout:
                ScoutAttack();
                break;
            default:
                break;
        }
    }



    private void TankAttack()
    {
        SpawnAttackCone();
        StartCoroutine(TimerSpeAttack());
    }



    private IEnumerator TimerSpeAttack()
    {
        myAgentState.canSpeAttack = false;
        yield return new WaitForSeconds(3f);
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

    }

    private void ScoutAttack()
    {

    }
}
