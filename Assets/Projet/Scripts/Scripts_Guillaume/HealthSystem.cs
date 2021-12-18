using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthSystem : MonoBehaviour
{
    public delegate void HealthEvent();
    public HealthEvent onHealthEvent;
    public HealthEvent onBatteryEvent;
    
    [Header("Syestème de vie")]
    [SerializeField] private float health;

    private float maxHealth;
    private HealthBar myHealthBar;

    private GameObject nexus;


    // Batterie system
    [SerializeField] private float batteryHealth = 100;
    private float maxBatteryHealth;
    private HealthBar myBatteryBar;
    public bool damageBattery = true;


    private void Awake()
    {
        nexus = GameObject.Find("Nexus");

        maxBatteryHealth = batteryHealth;
        //check la classe de l'objet en question et récupère la valeur dans la classe
        if (GetComponent<ClassBatimentContainer>() != null) health = GetComponent<ClassBatimentContainer>().myClass.health;
        if (GetComponent<ClassAgentContainer>() != null) health = GetComponent<ClassAgentContainer>().myClass.health;

        maxHealth = health;
        FindBarInChild();
        
    }


    


    public bool CheckDistanceNexus()
    {
        if (Vector3.Distance(transform.position, nexus.transform.position) > BatteryManager.instance.radiusBattery)
        {
            return true;
        }
        return false;
    }

    // NE PAS CHANGER HIERARCHIE DE PAR CE SCRIPT
    private void FindBarInChild()
    {
        var canvas = transform.Find("UIOnWorldSpace").Find("Canvas");
        myHealthBar = canvas.Find("HealthBar").GetComponent<HealthBar>();
        myBatteryBar = canvas.Find("BatteryBar").GetComponent<HealthBar>();
    }

    private void OnEnable()
    {
        onHealthEvent += ChangeHealthBar;
        onBatteryEvent += ChangeBatteryBar;

    }

    private void OnDisable()
    {
        onHealthEvent -= ChangeHealthBar;
        onBatteryEvent -= ChangeBatteryBar;
    }



    private void ChangeHealthBar()
    {
        var percentageHealth = health / maxHealth;
        myHealthBar.SetHealth(percentageHealth);
    }

    private void ChangeBatteryBar()
    {
        var percentageBattery = batteryHealth / maxBatteryHealth;
        myBatteryBar.SetHealth(percentageBattery);
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

    public void CheckIfKill()
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

    // CheckKill() pour la battery est utilisé dans BatteryManager
    public void ChangeBatteryHealth(float value)
    {
        batteryHealth += value;
        if (batteryHealth > maxBatteryHealth) batteryHealth = maxBatteryHealth;
        onBatteryEvent?.Invoke();
    }
}
