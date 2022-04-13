using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ClassAgentContainer))]
public class AgentStates : MonoBehaviour
{
    // Event d'actions
    public delegate void EventAction();
    public EventAction onAttack;
    public EventAction onSpeAttack;
    public EventAction onFollowEnter;
    public EventAction onIdleEnter;


    // Etats possible de nos agents, ennemis et/ou alliés
    // Changement d'état d'un agent par la fonction SetStates uniquement !
    public enum states { Idle, Aggressive, Follow};
    public states myState = states.Idle;


    // Etat spécial de l'agression. Uniquement pour ennemis pour les attaques nocturnes
    // Permet un tunnel vision sur le nexus, sauf si l'agent trouve une cible sur son chemin. Si cible détruite => retourne sur Nexus
    public bool isSuperAggressive = false;

    // Stats de nos agents. On récupère ces données depuis les classes crées
    [Header("Stats Agent Read-Only")]
    [SerializeField] private float speed = 10;
    [SerializeField] private float attackRange = 1;
    [SerializeField] private float damage = 1;
    [SerializeField] private float rateOfFire = 0.5f;
    [SerializeField] private float radiusVision = 5;
    public float rateOfFireCD = 0;

    
    // Navmesh et vaiable de destination/objectif de nos agents
    private NavMeshAgent navM;
    private Vector3 restDestination;
    [SerializeField] private GameObject targetToAttack;


    // Permet de récupérer les stats de nos agents
    public ClassAgentContainer container;

    private AIAgents myAI;


    // Spé-Attack 
    [Header("Spe Attack READ ONLY")]
    public bool canSpeAttack = false;

    private void Awake()
    {
        InitStats();
        navM = GetComponent<NavMeshAgent>();
        navM.speed = speed;
        navM.acceleration = 60f;
        navM.avoidancePriority = Random.Range(1, 100);
        restDestination = transform.position;
        if (GetComponent<AIAgents>()) myAI = GetComponent<AIAgents>();
    }


    // Récupération des statistiques depuis la classe
    private void InitStats ()
    {
        container = GetComponent<ClassAgentContainer>();
        speed = container.myClass.movementSpeed;
        attackRange = container.myClass.rangeAttaque + 0.7f;
        damage = container.myClass.attackDamage;
        rateOfFire = container.myClass.rateOfFire;
        radiusVision = container.myClass.radiusVision;
    }

    private void Update()
    {
        rateOfFireCD -= Time.deltaTime;
        // Ce que fait notre agent lorsqu'il est dans tel ou tel état en runtime
        switch (myState)
        {
            case states.Idle:
                if (Vector3.Distance(transform.position, restDestination) > 1f)
                {
                    MoveAgent(restDestination);
                    SetState(states.Follow);
                }
                break;
            case states.Aggressive:
                // Update position si la target a bougé de sa position initiale
                UpdatePositionTarget();
                if (canSpeAttack && container.myClass.mySpe != AgentClass.AgentSpe.Scout)
                {
                    var spe = GetComponent<SpeAttackClass>();
                    ChangeAttackValue(spe.speAttackRange, spe.speAttackDamage);
                }
                if (targetToAttack != null)
                {
                    if (navM.remainingDistance > attackRange)
                    {
                        navM.isStopped = false;
                    }
                    else if (navM.hasPath && navM.remainingDistance < attackRange && !navM.pathPending)
                    {
                        // Update position quand la target bouge pendant une attaque
                        if (Vector3.Distance(gameObject.transform.position, targetToAttack.transform.position) > attackRange)
                        {
                            MoveAgent(targetToAttack.transform.position);
                        }
                        // On arrête l'agent quand il est à porter d'attaque
                        onIdleEnter?.Invoke();
                        navM.isStopped = true;
                        AttaqueEnnemi(targetToAttack);
                    }
                }
                // Super agressive seulement utilisé pour les ennemis
                else if (isSuperAggressive)
                {
                    SetTarget(HQBehavior.instance.gameObject);
                }
                else
                {
                    // Remet en idle si plus aucune cible à attaquer
                    SetState(states.Idle);
                }
                break;
            case states.Follow:
                if (navM.hasPath && navM.remainingDistance < 0.5f)
                {
                    if (myAI.hasTargetInSight)
                    {
                        SetState(states.Aggressive);
                    }

                    else SetState(states.Idle);
                }
                break;

            default:
                break;
        }
    }


    private void UpdatePositionTarget()
    {
        if (targetToAttack != null)
        {
            if (!navM.pathPending)
            {
                var distance = Vector3.Distance(targetToAttack.transform.position, navM.pathEndPosition);

                if (distance > 1.5f)
                {
                    onFollowEnter?.Invoke();
                    navM.SetDestination(targetToAttack.transform.position);
                }
            }
        }
    }

    public void MoveAgent (Vector3 destination)
    {
        if (myState != states.Follow)
            onFollowEnter?.Invoke();
        navM.SetDestination(destination);
    }

    // Fonction permettant de modifier l'état d'un agent. C'est cette fonction que l'on appelle dans une IA ou par les inputs du player
    // pour donner un changement d'état de nos agents
    public void SetState (states newState)
    {
        if (newState != states.Follow)
        navM.ResetPath();
        navM.isStopped = false;
        myState = newState;
        OnEnterState();
    }

    // Que fais l'agent lorsqu'il entre dans un état. Actions jouées une seule fois
    // Permet l'utilisation de feedback sans utiliser l'Update
    // NB : ajout d'un OnExitState si besoin un jour
    private void OnEnterState()
    {
        switch (myState)
        {
            case states.Idle:
                onIdleEnter?.Invoke();
                navM.isStopped = true;
                break;
            case states.Aggressive:
                if (HasTarget())
                {
                    //MoveAgent(targetToAttack.transform.position);
                    navM.SetDestination(targetToAttack.transform.position);
                }
                break;
            case states.Follow:
                onFollowEnter?.Invoke();
                break;
            default:
                break;
        }
    }



    public void SetTarget (GameObject newGO)
    {
        targetToAttack = newGO;
    }

    private void AttaqueEnnemi(GameObject target)
    {
        transform.LookAt(target.transform);
        if (target.GetComponent<HealthSystem>() != null)
        {
            if (rateOfFireCD <= 0)
            {

                if (canSpeAttack && container.myClass.mySpe != AgentClass.AgentSpe.Scout)
                {
                    SpeAttackBehaviour(target);
                }
                else 
                {
                    AttackBehaviour(target);
                }
                
            }
        }
    }

    private void AttackBehaviour(GameObject target)
    {
        onAttack?.Invoke();
        rateOfFireCD = rateOfFire;
        target.GetComponent<HealthSystem>().HealthChange(-damage);
    }

    private void SpeAttackBehaviour(GameObject target)
    {
        Debug.Log("Spe attack");
        onSpeAttack?.Invoke();
        rateOfFireCD = rateOfFire;
        canSpeAttack = false;
        ChangeAttackValue(container.myClass.rangeAttaque, container.myClass.attackDamage);
    }

    public void ChangeAttackValue(float _attackRange, float _damage)
    {
        attackRange = _attackRange + 0.7f;
        damage = _damage;
    }

    private void OnDrawGizmos()
    {
        DrawRangeAttack();
    }

    private void DrawRangeAttack ()
    {
        if(container == null || attackRange != container.myClass.rangeAttaque)
        {
            container = GetComponent<ClassAgentContainer>();
            attackRange = container.myClass.rangeAttaque;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


    public bool HasTarget()
    {
        if (targetToAttack != null) return true;
        return false;
    }

    public GameObject ReturnTarget()
    {
        return targetToAttack;
    }

    public void SetRestDest(Vector3 dest)
    {
        restDestination = dest;
    }
}
