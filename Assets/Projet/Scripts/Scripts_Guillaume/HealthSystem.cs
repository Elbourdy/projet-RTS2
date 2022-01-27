using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthSystem : MonoBehaviour
{
    public delegate void HealthEvent();
    public HealthEvent onHealthEvent;
    public HealthEvent onBatteryEvent;

    public HealthEvent onDeathEvent;
    
    [Header("Syestème de vie")]
    [SerializeField] private float health;

    private float maxHealth;
    private HealthBar myHealthBar;

    private GameObject nexus;


    // Batterie system
    [Header("Syestème de batterie")]
    [SerializeField] private float batteryHealth = 100;
    private float maxBatteryHealth;
    [SerializeField] private HealthBar myBatteryBar;
    public bool damageBattery = true, startAtZeroBattery = false;


    private void Awake()
    {
        nexus = GameObject.Find("Nexus");

        maxBatteryHealth = batteryHealth;
        if (startAtZeroBattery)
            batteryHealth = 0;
        //check la classe de l'objet en question et récupère la valeur dans la classe
        if (GetComponent<ClassBatimentContainer>() != null) health = GetComponent<ClassBatimentContainer>().myClass.health;
        if (GetComponent<ClassAgentContainer>() != null) health = GetComponent<ClassAgentContainer>().myClass.health;

        maxHealth = health;
        FindBarInChild();
        if (gameObject.name != "Nexus")
            ChangeBatteryBar();
    }

    public bool CheckDistanceNexus()
    {
        if (Vector3.Distance(transform.position, nexus.transform.position) > BatteryManager.instance.radiusBattery * NexusLevelManager.instance.GetMultiplicatorRangeNexus())
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
        if (canvas.parent.parent.gameObject.name != "Nexus")
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
        //StartCoroutine(FeedbackColorChangeTimer());
    }

    IEnumerator FeedbackColorChangeTimer()
    {
        if (GetComponent<ClassAgentContainer>() != null)
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            GetComponent<MeshRenderer>().material.color = Color.white;
        }
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
        onDeathEvent?.Invoke();
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

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetMaxBatteryHealth()
    {
        return maxBatteryHealth;
    }

    // CheckKill() pour la battery est utilisé dans BatteryManager
    public void ChangeBatteryHealth(float value)
    {
        batteryHealth += value;
        if (batteryHealth > maxBatteryHealth) batteryHealth = maxBatteryHealth;
        if (batteryHealth <= 0 && startAtZeroBattery) batteryHealth = 0;
        onBatteryEvent?.Invoke();
    }
}
