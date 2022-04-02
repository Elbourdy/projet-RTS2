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

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("GameplayUnits")))
        {
            if (hit.collider.GetComponent<Agent_Type>() != null)
            {
                if (hit.collider.GetComponent<Agent_Type>().Type == Agent_Type.TypeAgent.Enemy)
                {
                    Debug.Log("attaque");
                    AttaqueWithAgent(hit);
                }
            }
        }

        else if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            GoToTarget(hit);
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
                var asAgent = agent.GetComponent<AgentStates>();
                asAgent.MoveAgent(target);
                if(asAgent.myState != AgentStates.states.Follow)
                asAgent.SetState(AgentStates.states.Follow);
                asAgent.SetRestDest(target);
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
                var agentState = agent.GetComponent<AgentStates>();
                if (agentState.ReturnTarget() != hit.collider.gameObject)
                {
                    agentState.SetTarget(hit.collider.gameObject);
                    agentState.SetState(AgentStates.states.Aggressive);
                    agentState.MoveAgent(hit.point);
                }
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
