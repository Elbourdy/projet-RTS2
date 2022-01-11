using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SelectionPlayer))]
public class BatteryManager : MonoBehaviour
{
    [SerializeField]public float damagePerTic = -1;
    [SerializeField]public bool activateSystem = true;
    [SerializeField]public float timeForTic = 4;


    private SelectionPlayer mySelec;
    [SerializeField] private float timer = 0;

    public float radiusBattery = 10f;

    public static BatteryManager instance;


    private void Awake()
    {
        instance = this;
        mySelec = GetComponent<SelectionPlayer>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        CheckTimer();
    }

    private void CheckTimer()
    {
        
        if (timer >= timeForTic)
        {
            timer = 0;
            // Stockage des unit�s devant �tre tu�es par le systeme de batterie
            // On fait �a pour ne pas fucked up la lecture de la list des agents actifs
            GameObject[] killedUnits = new GameObject[mySelec.allFriendlyUnits.Count];
            for (int i = 0; i < mySelec.allFriendlyUnits.Count; i++)
            {
                // D�g�ts inflig�s si bool�en est vrai. A mettre sur false ou true si l'agent est ou n'est pas dans la zone du nexus
                if (mySelec.allFriendlyUnits[i].GetComponent<HealthSystem>().CheckDistanceNexus()) mySelec.allFriendlyUnits[i].GetComponent<HealthSystem>().ChangeBatteryHealth(damagePerTic);
                else mySelec.allFriendlyUnits[i].GetComponent<HealthSystem>().ChangeBatteryHealth(-damagePerTic);
                if (mySelec.allFriendlyUnits[i].GetComponent<HealthSystem>().GetBatteryHealth() <= 0)
                {
                    killedUnits[i] = mySelec.allFriendlyUnits[i];
                }
            }
            foreach (var units in killedUnits)
            {
                if (units != null) units.GetComponent<HealthSystem>().CheckIfKill();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        DrawRadiusBattery();
    }

    private void DrawRadiusBattery()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(GameObject.Find("Nexus").transform.position, radiusBattery);
    }


}