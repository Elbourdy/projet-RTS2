using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class AgentStates : MonoBehaviour
{
    public enum states { Idle, Agressif, Recolte, Construction};
    public states myState = states.Idle;


    [Header("Stats Agent")]
    public float speed = 1;
    public float rangeAttaque = 1;

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
        switch (myState)
        {
            case states.Idle:
                break;
            case states.Agressif:
                if (objectDestination != null)
                {
                    navM.SetDestination(objectDestination.transform.position);

                    if (navM.remainingDistance > rangeAttaque)
                    {
                        navM.isStopped = false;
                    }
                    else if (navM.hasPath && navM.remainingDistance < rangeAttaque)
                    {
                        navM.isStopped = true;
                        AttaqueEnnemi();
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
        navM.isStopped = false;
        myState = newState;

        switch (myState)
        {
            case states.Idle:
                GetComponent<MeshRenderer>().material.color = IdleColor;
                break;
            case states.Agressif:
                GetComponent<MeshRenderer>().material.color = AgressifColor;
                break;
            case states.Recolte:
                GetComponent<MeshRenderer>().material.color = RecolteColor;
                break;
            case states.Construction:
                GetComponent<MeshRenderer>().material.color = ConstructionColor;
                break;
            default:
                break;
        }
    }
    public void SetObjectDestination (GameObject newObject)
    {
        objectDestination = newObject;
    }
    private void AttaqueEnnemi ()
    {
        Debug.Log("Attaque");
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

    
}
