using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlayer : MonoBehaviour
{

    private SelectionPlayer sp;


    private int myLayer = 1 << 3;

    public delegate void Event();
    public Event onGlobalOrder;

    public static InputPlayer instance;




    [Header("Feedbacks")]
    [FMODUnity.EventRef]
    public string SoundOrder;
    public GameObject FeedbackOnMovementOrder;



    private GameObject attenuationPoint;


    private void Awake()
    {
        instance = this;
        sp = GetComponent<SelectionPlayer>();
        onGlobalOrder += LaunchSoundOrder;

        attenuationPoint = Camera.main.transform.Find("attenuationpoint").gameObject;
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (sp.selectedUnits.Count > 0)
            {
                CheckHitType();
            }
        }
    }



    private void CheckHitType ()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // GROSSE BAGARRE NECESSAIRE AVEC LES LAYERS APRES SOUTENANCE
        // IMPOSSIBLE DE COMPRENDRE POURQUOI AUCUN LAYER FONCTIONNE
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.name == "Sol")
            {
                GoToTarget(hit);
            }

            if (hit.collider.GetComponent<Agent_Type>() != null)
            {
                if (hit.collider.GetComponent<Agent_Type>().Type == Agent_Type.TypeAgent.Enemy)
                {
                    AttaqueWithAgent(hit);
                }
            }
        }
    }


    public static LayerMask NamesToMask(params string[] layerNames)
    {
        LayerMask ret = (LayerMask)0;
        foreach (var name in layerNames)
        {
            ret |= (1 << LayerMask.NameToLayer(name));
        }
        return ret;
    }


    private void GoToTarget (RaycastHit hit)
    {
        Vector3 target = hit.point;
        foreach (var agent in sp.selectedUnits)
        {
            if (sp.selectedUnits.Count > 1)
            {
                target = RandomizeTargetLocation(target, 2);
            }
            if (agent.GetComponent<AgentStates>() != null && agent.GetComponent<Agent_Type>().Type == Agent_Type.TypeAgent.Ally)
            {
                agent.GetComponent<AgentStates>().MoveAgent(target);
                if(agent.GetComponent<AgentStates>().myState != AgentStates.states.Follow)
                agent.GetComponent<AgentStates>().SetState(AgentStates.states.Follow);
            }
        }
        CreateVfxOnMoveOrder(hit.point);
        onGlobalOrder?.Invoke();

    }


    private Vector3 RandomizeTargetLocation(Vector3 location, float range)
    {
        
        float randomX = Random.Range(-range, range);
        float randomZ = Random.Range(-range, range);

        var newLocation = new Vector3(location.x + randomX, location.y, location.z + randomZ);


        return newLocation;
    }
    

    private void AttaqueWithAgent (RaycastHit hit)
    {

        foreach (var agent in sp.selectedUnits )
        {
            if (agent.GetComponent<AgentStates>() != null && agent.GetComponent<Agent_Type>().Type == Agent_Type.TypeAgent.Ally)
            {
                agent.GetComponent<AgentStates>().SetTarget(hit.collider.gameObject);
                agent.GetComponent<AgentStates>().SetState(AgentStates.states.Aggressive);
                agent.GetComponent<AgentStates>().MoveAgent(hit.point);
            }
        }
        onGlobalOrder?.Invoke();
    }


    #region Feedbacks

    private void LaunchSoundOrder()
    {
        if (SoundOrder != null)
            FMODUnity.RuntimeManager.PlayOneShot(SoundOrder, attenuationPoint.transform.position);
    }


    private void CreateVfxOnMoveOrder(Vector3 positionHit)
    {
        Instantiate(FeedbackOnMovementOrder, positionHit, Quaternion.identity);
    }


    #endregion
}
