using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



// Addon pour healthSystem et Nexus
// Permet l'ajout de feedbacks et fonctions uniques pour le Nexus
[RequireComponent(typeof(HealthSystem))]
public class NexusHealthSystemAddon : MonoBehaviour
{
    private HealthSystem nexusHealthSystem;

    [FMODUnity.EventRef]
    public string SoundDeath;


    [Header("Regen System")]
    [SerializeField] private float regenValue = 1;
    [SerializeField] private bool regenIsActivated = false;

    // Timer System
    [SerializeField] private float timeBeforeRegen = 5;




    private void Awake()
    {
        nexusHealthSystem = GetComponent<HealthSystem>();
        nexusHealthSystem.onHealthEvent += ActivateTimerToAllowRegen;
        nexusHealthSystem.onHealthEvent += CheckIfKilled;
    }

    private void OnDisable()
    {
        nexusHealthSystem.onHealthEvent -= ActivateTimerToAllowRegen;
        nexusHealthSystem.onHealthEvent -= CheckIfKilled;
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
            LaunchSoundDeath();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }




    IEnumerator TimerRegen()
    {
        // Active
        yield return new WaitForSeconds(timeBeforeRegen);
        ActivateNexusRegen();
    }


    private void ActivateTimerToAllowRegen()
    {
        // On évite que le nexus puisse regen au moment d'un GameOver
        if (nexusHealthSystem.GetHealth() > 0) 
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


    private void LaunchSoundDeath()
    {
        FMODUnity.RuntimeManager.PlayOneShot(SoundDeath);
    }

    

}
