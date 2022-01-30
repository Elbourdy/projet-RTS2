using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(HealthSystem))]
public class NexusHealthSystemAddon : MonoBehaviour
{
    private HealthSystem nexusHealthSystem;

    [Header("Regen System")]
    [SerializeField] private float regenValue = 1;
    [SerializeField] private bool regenIsActivated = false;

    // Timer System
    [SerializeField] private float timeBeforeRegen = 5;
    private float timerCount = 0;

    private void Awake()
    {
        nexusHealthSystem = GetComponent<HealthSystem>();
        nexusHealthSystem.onHealthEvent += ActivateTimerToAllowRegen;
        nexusHealthSystem.onHealthEvent += CheckIfKilled;
    }


    private void Update()
    {
        if(regenIsActivated)
        {
            RegenNexus();
        }
    }


    private void CheckIfKilled()
    {
        if (nexusHealthSystem.GetHealth() <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


    IEnumerator TimerRegen()
    {
        yield return new WaitForSeconds(timeBeforeRegen);
        ActivateNexusRegen();
    }


    private void ActivateTimerToAllowRegen()
    {
        StopAllCoroutines();
        StartCoroutine(TimerRegen());
    }


    private void ActivateNexusRegen()
    {
        regenIsActivated = true;
    }

    private void RegenNexus() 
    {
        nexusHealthSystem.HealthChange(regenValue);
        if (nexusHealthSystem.GetHealth() >= nexusHealthSystem.GetMaxHealth())
        {
            regenIsActivated = false;
        }
    }

}
