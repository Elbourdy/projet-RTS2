using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ClassAgentContainer))]
public class AgentStates : MonoBehaviour
{
    public enum states { Idle, Agressif, Recolte, Construction, Follow};
    public states myState = states.Idle;


    [Header("Stats Agent Read-Only")]
    [SerializeField] private float speed = 10;
    [SerializeField] private float rangeAttaque = 1;
    [SerializeField] private float damage = 1;
    [SerializeField] private float rateOfFire = 0.5f;
    private float rateOfFireCD = 0;


    [Header("Constructions Stats Read-Only")]
    public GameObject constructionObjet;
    public float rangeConstruction = 1;
    

    private float timerRessource = 0;


    [Header("Color Agent State")]
    public Color RecolteColor;
    public Color IdleColor;
    public Color AgressifColor;
    public Color ConstructionColor;
    public Color FollowColor;


    [Header("Info pour HUD")]
    public float TypeUnit;
    public float NbrCase;
    

    private NavMeshAgent navM;
    private GameObject objectDestination;
    private GameObject targetToAttack;
    private GameObject ressourceTarget;



    private ClassAgentContainer container;

    private void OnEnable()
    {
        InitStats();
        navM = GetComponent<NavMeshAgent>();
        navM.speed = speed;
        navM.acceleration = 60f;
    }



    private void InitStats ()
    {
        container = GetComponent<ClassAgentContainer>();
        speed = container.myClass.movementSpeed;
        rangeAttaque = container.myClass.rangeAttaque;
        damage = container.myClass.attackDamage;
        rateOfFire = container.myClass.rateOfFire;
    }

    private void Update()
    {
        rateOfFireCD -= Time.deltaTime;

        switch (myState)
        {
            case states.Idle:
                break;
            case states.Agressif:

                UpdatePosition();
                if (targetToAttack != null)
                {
                    //FollowTarget(objectDestination);
                    if (navM.remainingDistance > rangeAttaque)
                    {
                        navM.isStopped = false;
                    }
                    else if (navM.hasPath && navM.remainingDistance < rangeAttaque)
                    {
                        if (Vector3.Distance(gameObject.transform.position, targetToAttack.transform.position) > rangeAttaque)
                        {
                            MoveAgent(targetToAttack.transform.position);
                        }
                        navM.isStopped = true;
                        AttaqueEnnemi(targetToAttack);
                    }
                }
                else
                {
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


    private void UpdatePosition()
    {
        if (targetToAttack != null)
        {
            if (!navM.pathPending)
            {
                if (targetToAttack.transform.position != navM.pathEndPosition)
                {
                    navM.SetDestination(targetToAttack.transform.position);
                }
            }
        }
    }

    public void MoveAgent (Vector3 destination)
    {
        navM.SetDestination(destination);
    }

    public void SetRessourceTarget(GameObject target)
    {
        ressourceTarget = target;
    }
    public void SetState (states newState)
    {
 
        navM.ResetPath();
        navM.isStopped = false;
        myState = newState;

        switch (myState)
        {
            case states.Idle:
                GetComponent<MeshRenderer>().material.color = IdleColor;
                navM.isStopped = true;
                break;
            case states.Agressif:
                GetComponent<MeshRenderer>().material.color = AgressifColor;
                break;
            case states.Recolte:
                if (GetComponent<ClassAgentContainer>().myClass.Job != AgentClass.AgentJob.Worker)
                {
                    SetState(states.Idle);
                    break;
                }
                else GetComponent<MeshRenderer>().material.color = RecolteColor;
                break;
            case states.Construction:
                GetComponent<MeshRenderer>().material.color = ConstructionColor;
                break;
            case states.Follow:
                GetComponent<MeshRenderer>().material.color = FollowColor;
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
                Debug.Log("Attaque");
                rateOfFireCD = rateOfFire;
                target.GetComponent<HealthSystem>().HealthChange(-damage);
            }
        }
    }
    
    private void RecolteRessources()
    {
        if (CheckTimerRessource())
        {
            Global_Ressources.instance.ModifyRessource(ressourceTarget.GetComponent<RessourcesObject>().GetId(), ressourceTarget.GetComponent<RessourcesObject>().GetRessource());
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
    
}
