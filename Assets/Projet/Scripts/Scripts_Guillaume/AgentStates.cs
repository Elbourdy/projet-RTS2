using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class AgentStates : MonoBehaviour
{
    public enum states { Idle, Agressif, Recolte, Construction, Follow};
    public states myState = states.Idle;


    [Header("Stats Agent")]
    [SerializeField] private float speed = 1;
    [SerializeField] private float rangeAttaque = 1;
    [SerializeField] private float damage = 1;
    [SerializeField] private float rateOfFire = 0.5f;
    private float rateOfFireCD = 0;


    [Header("Constructions Stats")]
    public GameObject constructionObjet;
    public float rangeConstruction = 1;
    

    [Header("Ressources Agent Setup")]
    public float maxRessources = 10;
    [SerializeField]
    private float currentRessources = 0;


    [Header("Color Agent State")]
    public Color RecolteColor;
    public Color IdleColor;
    public Color AgressifColor;
    public Color ConstructionColor;
    public Color FollowColor;


    private NavMeshAgent navM;
    private GameObject objectDestination;


    private void Start()
    {
        navM = GetComponent<NavMeshAgent>();
        navM.speed = speed;
        navM.acceleration = 60f;
    }

    private void Update()
    {
        rateOfFireCD -= Time.deltaTime;
        switch (myState)
        {
            case states.Idle:
                break;
            case states.Agressif:
                if (objectDestination != null)
                {
                    //FollowTarget(objectDestination);
                    if (navM.remainingDistance > rangeAttaque)
                    {
                        navM.isStopped = false;
                        Debug.Log("nav not stopped : "+ navM.remainingDistance + ">" + rangeAttaque);
                    }
                    else if (navM.hasPath && navM.remainingDistance < rangeAttaque)
                    {
                        if (Vector3.Distance(gameObject.transform.position, objectDestination.transform.position) > rangeAttaque)
                        {
                            FollowTarget(objectDestination);
                        }
                        navM.isStopped = true;
                        Debug.Log("nav stopped");
                        AttaqueEnnemi(objectDestination);
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
                    if (currentRessources >= maxRessources)
                    {
                        currentRessources = maxRessources;
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
                    Debug.Log("dist = " + navM.remainingDistance);
                    SetState(states.Idle);
                }
                    break;
            default:
                break;
        }
    }


    public void MoveAgent (Vector3 destination)
    {
        navM.SetDestination(destination);
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
    private void AttaqueEnnemi(GameObject target)
    {
        Debug.Log("Attaque");
        if (target.GetComponent<HealthSystem>() != null)
        {
            if (rateOfFireCD <= 0)
            {
                rateOfFireCD = rateOfFire;
                target.GetComponent<HealthSystem>().HealthChange(-damage);
            }
        }
    }
    
    private void RecolteRessources()
    {
        currentRessources += 0.01f;
        Debug.Log("Recolte !!");
    }
    private void Construire()
    {
        navM.isStopped = true;
        navM.ResetPath();
        SetState(states.Idle);
        Vector3 constructionPos = transform.position + transform.forward * rangeConstruction;
        Instantiate(constructionObjet, constructionPos, transform.rotation);
    }
    private void FollowTarget(GameObject target)
    {
        navM.SetDestination(target.transform.position);
    }
    
}
