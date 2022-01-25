using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SelectionPlayer))]
public class BatteryManager : MonoBehaviour
{
    [SerializeField] public float damagePerTic = -1;
    [SerializeField] public float rechargePerTic = 3;
    [SerializeField] public bool activateSystem = true;
    [SerializeField] public int energyConsumedByNexusPerTick = 1;
    [SerializeField] public int energyConsumedByUnitPerTick = 1;
    [SerializeField] public int energyConsumedByBuildingPerTick = 1;
    [SerializeField] public float timeOfATickInSeconds = 1f;

    public int energyConsumeByTick;
    public float radiusBattery = 10f;
    public static BatteryManager instance;
    public List<GameObject> batteries = new List<GameObject>();

    private float timerRechargeCount;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (timerRechargeCount > timeOfATickInSeconds)
        {
            ChargeUnit();
            timerRechargeCount = 0;
        }

        timerRechargeCount += Time.deltaTime;
    }
    public void ChargeUnit()
    {
        // Stockage des unités devant être tuées par le systeme de batterie
        // On fait ça pour ne pas fucked up la lecture de la list des agents actifs
        GameObject[] killedUnits = new GameObject[batteries.Count];

        energyConsumeByTick = CalculateEnergyConsumedNextTick();

        for (int i = 0; i < batteries.Count; i++)
        {
            HealthSystem hS = batteries[i].GetComponent<HealthSystem>();
            // Dégâts infligés si booléen est vrai. A mettre sur false ou true si l'agent est ou n'est pas dans la zone du nexus

            if (batteries[i].GetComponent<HealthSystem>().CheckDistanceNexus())
            {
                batteries[i].GetComponent<HealthSystem>().ChangeBatteryHealth(damagePerTic);
            }
            else
            {
                batteries[i].GetComponent<HealthSystem>().ChangeBatteryHealth(rechargePerTic);
            }
                
            if (batteries[i].GetComponent<HealthSystem>().GetBatteryHealth() <= 0 && !batteries[i].GetComponent<Agent_Type>().isConstruction)
            {
                killedUnits[i] = batteries[i];
            }
        }

        foreach (var units in killedUnits)
        {
            if (units != null) units.GetComponent<HealthSystem>().CheckIfKill();
        }

        Global_Ressources.instance.ModifyRessource(0, - energyConsumeByTick);
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

    public int CalculateEnergyConsumedNextTick()
    {
        int count = energyConsumedByNexusPerTick;

        foreach (GameObject e in batteries)
        {
            if (!e.GetComponent<HealthSystem>().CheckDistanceNexus())
            {
                if (!e.GetComponent<Agent_Type>().isConstruction)
                    count += energyConsumedByUnitPerTick;
                else
                    count += energyConsumedByBuildingPerTick;
            }
        }

        return Mathf.RoundToInt(count * NexusLevelManager.instance.GetMultiplicatorConsomption());
    }
}
