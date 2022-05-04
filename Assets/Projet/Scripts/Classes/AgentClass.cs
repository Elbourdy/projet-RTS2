using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Agent", menuName = "ScriptableObjects/Unit/Agent")]
public class AgentClass : UnitClass
{

    public float attackDamage;
    public float rangeAttaque;
    public float rateOfFire;
    public float movementSpeed;
    public float radiusVision = 5;
    public bool isRanged = false;

    [DrawIf("isRanged", true)] public GameObject ammo;

    [Header("Valeurs de création")]
    public int[] ressourcesCost  = new int[]{50, 0, 0};
    public int spawnerCost = 0;
    public float timerCreation;

    [Header("Visuels")]
    public GameObject unitPrefab;
    public Sprite unitSprite;

    public enum AgentSpe
    {
        None, Tank, Artillery, Scout
    };
    [Header("Attack Spéciale")]
    public AgentSpe mySpe;
    [Space(10)]
    #region Tank
    [Tooltip("Le GameObject représentant la zone de dégâts")]
    [DrawIf("mySpe", AgentSpe.Tank)] public GameObject tankAttackGo;
    [Tooltip("Les dégâts à l'impact qu'inflige la zône")]
    [DrawIf("mySpe", AgentSpe.Tank)] public float damageTank;
    [Tooltip("Le CD de l'attaque spé en elle-même")]
    [DrawIf("mySpe", AgentSpe.Tank)] public float cooldownAttackTank;
    [Tooltip("La distance à laquelle l'agent peut déclencher son attaque spéciale")]
    [DrawIf("mySpe", AgentSpe.Tank)] public float attackRangeTank;
    [Tooltip("La distance à laquelle la zone dégâts doit apparaître par rapport à l'agent")]
    [DrawIf("mySpe", AgentSpe.Tank)] public float distanceSpawnTankAttack;

    #endregion


    #region Artillery
    [Tooltip("Le GameObject représentant la zone de dégâts")]
    [DrawIf("mySpe", AgentSpe.Artillery)] public GameObject poisonArea;
    [Tooltip("Les dégâts de la zone par tick")]
    [DrawIf("mySpe", AgentSpe.Artillery)] public float damagePoison;
    [Tooltip("Le CD de l'attaque spé en elle-même")]
    [DrawIf("mySpe", AgentSpe.Artillery)] public float cooldownAttackPoison;
    [Tooltip("Le nombre de secondes pour le prochain tick de dégâts de la zône")]
    [DrawIf("mySpe", AgentSpe.Artillery)] public float speedTick;
    [Tooltip("La distance à laquelle l'agent peut déclencher son attaque spéciale")]
    [DrawIf("mySpe", AgentSpe.Artillery)] public float attackRangePoison;
    [Tooltip("La distance à laquelle la zone dégâts doit apparaître par rapport à l'agent")]
    [DrawIf("mySpe", AgentSpe.Artillery)] public float distanceSpawnPoison;

    #endregion


    #region Scout
    [Tooltip("Combien de temps l'agent reste-il invisible ?")]
    [DrawIf("mySpe", AgentSpe.Scout)] public float timerInvisibility;
    [Tooltip("Le CD de l'attaque spé en elle-même")]
    [DrawIf("mySpe", AgentSpe.Scout)] public float cooldownInvisibility;


    #endregion




    public static AgentClass.AgentSpe GetSpe(GameObject go)
    {
        if (go.TryGetComponent(out ClassAgentContainer container))
        {
            return container.myClass.mySpe;
        }

        return AgentSpe.None;
    }
}
