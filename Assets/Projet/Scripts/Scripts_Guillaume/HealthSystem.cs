using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthSystem : MonoBehaviour
{
    public delegate void HealthEvent();
    public HealthEvent onHealthEvent;
    
    [SerializeField] private float health;

    private float maxHealth;
    private HealthBar myBar;



    // Batterie system
    [SerializeField] private float batteryHealth = 100;
    private float maxBatteryHealth;



    private void OnEnable()
    {
        maxBatteryHealth = batteryHealth;
        onHealthEvent += ChangeHealthBar;
        //check la classe de l'objet en question et récupère la valeur dans la classe
        if (GetComponent<ClassBatimentContainer>() != null) health = GetComponent<ClassBatimentContainer>().myClass.health;
        if (GetComponent<ClassAgentContainer>() != null) health = GetComponent<ClassAgentContainer>().myClass.health;

        maxHealth = health;

        myBar = GetComponentInChildren<HealthBar>();
    }

    private void OnDisable()
    {
        onHealthEvent -= ChangeHealthBar;
    }


    private void ChangeHealthBar()
    {
        var percentageHealth = health / maxHealth;
        myBar.SetHealth(percentageHealth);
    }

    private void FeedBackDamage ()
    {
        Debug.Log("Feedback");
        StartCoroutine(FeedbackColorChangeTimer());
    }

    IEnumerator FeedbackColorChangeTimer()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        GetComponent<MeshRenderer>().material.color = Color.white;
    }

    public void HealthChange(float damageNumber)
    {
        FeedBackDamage();
        health += damageNumber;
        onHealthEvent?.Invoke();
        CheckIfKill();
    }

    private void CheckIfKill()
    {
        if (health <= 0 || batteryHealth <= 0)
        {
            KillGo();
        }
    }

    private void KillGo()
    {
        Destroy(gameObject);
    }


    public float GetHealth()
    {
        return health;
    }
    public float GetBatteryHealth()
    {
        return batteryHealth;
    }

    public void ChangeBatteryHealth(float value)
    {
        batteryHealth += value;
        if (batteryHealth > maxBatteryHealth) batteryHealth = maxBatteryHealth;
        CheckIfKill();
    }
}
