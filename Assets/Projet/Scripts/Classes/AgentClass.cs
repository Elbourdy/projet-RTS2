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

    [Header("Valeurs de cr�ation")]
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
    [Header("Attack Sp�ciale")]
    public AgentSpe mySpe;
    [Space(10)]
    #region Tank
    [Tooltip("Le GameObject repr�sentant la zone de d�g�ts")]
    [DrawIf("mySpe", AgentSpe.Tank)] public GameObject tankAttackGo;
    [Tooltip("Les d�g�ts � l'impact qu'inflige la z�ne")]
    [DrawIf("mySpe", AgentSpe.Tank)] public float damageTank;
    [Tooltip("Le CD de l'attaque sp� en elle-m�me")]
    [DrawIf("mySpe", AgentSpe.Tank)] public float cooldownAttackTank;
    [Tooltip("La distance � laquelle l'agent peut d�clencher son attaque sp�ciale")]
    [DrawIf("mySpe", AgentSpe.Tank)] public float attackRangeTank;
    [Tooltip("La distance � laquelle la zone d�g�ts doit appara�tre par rapport � l'agent")]
    [DrawIf("mySpe", AgentSpe.Tank)] public float distanceSpawnTankAttack;

    #endregion


    #region Artillery
    [Tooltip("Le GameObject repr�sentant la zone de d�g�ts")]
    [DrawIf("mySpe", AgentSpe.Artillery)] public GameObject poisonArea;
    [Tooltip("Les d�g�ts de la zone par tick")]
    [DrawIf("mySpe", AgentSpe.Artillery)] public float damagePoison;
    [Tooltip("Le CD de l'attaque sp� en elle-m�me")]
    [DrawIf("mySpe", AgentSpe.Artillery)] public float cooldownAttackPoison;
    [Tooltip("Le nombre de secondes pour le prochain tick de d�g�ts de la z�ne")]
    [DrawIf("mySpe", AgentSpe.Artillery)] public float speedTick;
    [Tooltip("La distance � laquelle l'agent peut d�clencher son attaque sp�ciale")]
    [DrawIf("mySpe", AgentSpe.Artillery)] public float attackRangePoison;
    [Tooltip("La distance � laquelle la zone d�g�ts doit appara�tre par rapport � l'agent")]
    [DrawIf("mySpe", AgentSpe.Artillery)] public float distanceSpawnPoison;

    #endregion


    #region Scout
    [Tooltip("Combien de temps l'agent reste-il invisible ?")]
    [DrawIf("mySpe", AgentSpe.Scout)] public float timerInvisibility;
    [Tooltip("Le CD de l'attaque sp� en elle-m�me")]
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
