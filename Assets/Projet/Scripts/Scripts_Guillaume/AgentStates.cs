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
    public EventAction onFollowEnter;
    public EventAction onIdleEnter;


    // Etats possible de nos agents, ennemis et/ou alliés
    // Changement d'état d'un agent par la fonction SetStates uniquement !
    public enum states { Idle, Agressif, Recolte, Construction, Follow};
    public states myState = states.Idle;
    public bool isSuperAggressif = false;

    // Stats de nos agents. On récupère ces données depuis les classes crées
    [Header("Stats Agent Read-Only")]
    [SerializeField] private float speed = 10;
    [SerializeField] private float rangeAttaque = 1;
    [SerializeField] private float damage = 1;
    [SerializeField] private float rateOfFire = 0.5f;
    [SerializeField] private float radiusVision = 5;
    public float rateOfFireCD = 0;


    [Header("Constructions Stats Read-Only")]
    public GameObject constructionObjet;
    public float rangeConstruction = 1;
    

    private float timerRessource = 0;

    // Feedback de couleur de nos agents. A RETIRER QUAND NOOUS AURONS DE VERITABLES FEEDBACKS
    [Header("Color Agent State")]
    public Color RecolteColor;
    public Color IdleColor;
    public Color AgressifColor;
    public Color ConstructionColor;
    public Color FollowColor;


    
    // Navmesh et vaiable de destination/objectif de nos agents
    private NavMeshAgent navM;
    private GameObject objectDestination;
    private GameObject targetToAttack;
    private GameObject ressourceTarget;



    // Permet de récupérer les stats de nos agents
    private ClassAgentContainer container;

    private void Awake()
    {
        InitStats();
        navM = GetComponent<NavMeshAgent>();
        navM.speed = speed;
        navM.acceleration = 60f;
        navM.avoidancePriority = Random.Range(1, 100);
    }


    // Récupération des statistiques depuis la classe
    private void InitStats ()
    {
        container = GetComponent<ClassAgentContainer>();
        speed = container.myClass.movementSpeed;
        rangeAttaque = container.myClass.rangeAttaque + 0.7f;
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
                break;
            case states.Agressif:
                // Update position si la target a bougé de sa position initiale
                UpdatePositionTarget();
                if (targetToAttack != null)
                {
                    if (navM.remainingDistance > rangeAttaque)
                    {
                        navM.isStopped = false;
                    }
                    else if (navM.hasPath && navM.remainingDistance < rangeAttaque)
                    {
                        // Update position quand la target bouge pendant une attaque
                        if (Vector3.Distance(gameObject.transform.position, targetToAttack.transform.position) > rangeAttaque)
                        {
                            MoveAgent(targetToAttack.transform.position);
                        }
                        // On arrête l'agent quand il est à porter d'attaque
                        navM.isStopped = true;
                        AttaqueEnnemi(targetToAttack);
                    }
                }

                else if (isSuperAggressif)
                {
                    SetState(states.Follow);
                    MoveAgent(HQBehavior.instance.transform.position);
                }
                else
                {
                    // Remet en idle si plus aucune cible à attaquer
                    SetState(states.Idle);
                }
                break;
            case states.Recolte:
                if (navM.remainingDistance < 1.5f)
                {
                    if (ressourceTarget == null)
                    {
                        SetState(states.Idle);
                        break;
                    }
                    RecolteRessources();
                }
                break;
            case states.Construction:
                if (navM.hasPath && navM.remainingDistance < rangeConstruction)
                {
                    Construire();
                }
                break;

            case states.Follow:
                if (navM.hasPath && navM.remainingDistance < 0.5f)
                {
                    SetState(states.Idle);
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

                if (distance > 1f)
                {
                    //onFollowEnter?.Invoke();
                    Debug.Log("Coucou");
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

    public void SetRessourceTarget(GameObject target)
    {
        ressourceTarget = target;
    }

    // Fonction permettant de modifier l'état d'un agent. C'est cette fonction que l'on appelle dans une IA ou par les inputs du player
    // pour donner un changement d'état de nos agents
    public void SetState (states newState)
    {
        if (newState != states.Follow)
        navM.ResetPath();
        navM.isStopped = false;
        myState = newState;

        switch (myState)
        {
            case states.Idle:
                onIdleEnter?.Invoke();
                navM.isStopped = true;
                break;
            case states.Agressif:
                break;
            case states.Recolte:
                if (GetComponent<ClassAgentContainer>().myClass.Job != AgentClass.AgentJob.Worker)
                {
                    SetState(states.Idle);
                    break;
                }
                break;
            case states.Construction:
                break;
            case states.Follow:
                break;
            default:
                break;
        }
    }
    public void SetObjectDestination (GameObject newObject)
    {
        objectDestination = newObject;
    }

    public void SetTarget (GameObject newGO)
    {
        targetToAttack = newGO;
    }
    private void AttaqueEnnemi(GameObject target)
    {
        if (target.GetComponent<HealthSystem>() != null)
        {
            if (rateOfFireCD <= 0)
            {
                onAttack?.Invoke();
                rateOfFireCD = rateOfFire;
                transform.LookAt(target.transform);
                target.GetComponent<HealthSystem>().HealthChange(-damage);
            }
        }
    }
    
    private void RecolteRessources()
    {
        if (CheckTimerRessource())
        {
            ressourceTarget.GetComponent<RessourcesObject>().AddRessourceToPlayer();
        }
    }

    private bool CheckTimerRessource()
    {

        if (timerRessource >= ressourceTarget.GetComponent<RessourcesObject>().GetTimer())
        {
            timerRessource = 0;
            return true;
        }
        timerRessource += Time.deltaTime;
        return false;
    }

    private void Construire()
    {
        navM.isStopped = true;
        navM.ResetPath();
        SetState(states.Idle);
        Vector3 constructionPos = transform.position + transform.forward * rangeConstruction;
        Instantiate(constructionObjet, constructionPos, transform.rotation);
    }


    private void OnDrawGizmos()
    {
        DrawRangeAttack();
    }

    private void DrawRangeAttack ()
    {
        if(container == null || rangeAttaque != container.myClass.rangeAttaque)
        {
            container = GetComponent<ClassAgentContainer>();
            rangeAttaque = container.myClass.rangeAttaque;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeAttaque);
    }

}
