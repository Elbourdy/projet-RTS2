using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SelectionPlayer))]
public class BatteryManager : MonoBehaviour
{
    public float damagePerTic = -1;

    public bool activateSystem = true;
    public float timeForTic = 4;


    private SelectionPlayer mySelec;
    [SerializeField] private float timer = 0;



    private void Awake()
    {
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
            // Stockage des unités devant être tuées par le systeme de batterie
            // On fait ça pour ne pas fucked up la lecture de la list des agents actifs
            GameObject[] killedUnits = new GameObject[mySelec.allFriendlyUnits.Count];
            for (int i = 0; i < mySelec.allFriendlyUnits.Count; i++)
            {
                // Dégâts infligés si booléen est vrai. A mettre sur false ou true si l'agent est ou n'est pas dans la zone du nexus
                if (mySelec.allFriendlyUnits[i].GetComponent<HealthSystem>().damageBattery) mySelec.allFriendlyUnits[i].GetComponent<HealthSystem>().ChangeBatteryHealth(damagePerTic);
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





}
