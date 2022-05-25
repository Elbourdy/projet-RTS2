using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class HQBehavior : Building
{
    public enum statesNexus {Move, Immobilize, ForcedImmobilize}
    [Header("OnReadOnly")]
    [SerializeField] public statesNexus currentNexusState = statesNexus.Move;

    [Header("Valeurs RGD")] // à initaliser avec agent class
    [SerializeField] private float BatType;
    [SerializeField] private List<int> desiredRoaster;
    [SerializeField] private int speed = 10;

    [Header("Feedback Visuel")]
    [SerializeField] private List<MeshRenderer> nexusRenderers = new List<MeshRenderer>();
    [SerializeField] private Animator animator;
    [SerializeField] private Color colorRadiusBattery = Color.blue;
    private LineRenderer lRBattery;

    // NavmeshSystem
    private NavMeshAgent navM;
    [Header("Movement System")]
    [SerializeField] private bool ActivateNewMovementSystem = true;

    [Header("Selection System")]
    [SerializeField] private bool AllowMultipleSelection = false;

    private Vector3 targetPosition;

    public static HQBehavior instance;

    [Header("VisionFog")]
    [SerializeField] private float radiusVision = 30f;
    [SerializeField] private GameObject cookie;

    private void Awake()
    {
        instance = this;
        if (GetComponent<NavMeshAgent>()) navM = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        //if (AllowMultipleSelection) GameObject.Find("GameManager").GetComponent<SelectionPlayer>().allFriendlyUnits.Add(gameObject);
    }

    private void OnDisable()
    {
        //if (AllowMultipleSelection) GameObject.Find("GameManager").GetComponent<SelectionPlayer>().allFriendlyUnits.Remove(gameObject);
    }

    void Start()
    {
        UIInitialisation();

        lRBattery = GetComponent<LineRenderer>();

        foreach (int e in desiredRoaster)
        {
            AddToRoaster(e);
        }

        targetPosition = transform.position;

        SetConstructionHealth(100f); //annule le système de construction

        //InitialisationRange();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayRange(BatteryManager.instance.radiusBattery * NexusLevelManager.instance.GetMultiplicatorRangeNexus(), colorRadiusBattery);
        SetAnimator();

        CheckIfSelected();
        if (GetIsSelected()) // si nexus selectionné déplace le point d'arrivée / le rally point
        {
            if (Input.GetMouseButtonUp(1))
            {
                if (GetIsMovingRallyPoint())
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Interface/UI_Interf_Flag/UI_Interf_Flag");

                    RaycastHit hit;
                    Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
                    SetRallyPoint(hit.point);
                    NewSelectionManager.instance.canSelect = true;
                    SetIsMovingRallyPoint(false);
                }
                else // means moving nexus
                {
                    RaycastHit hit;
                    Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
                    targetPosition = hit.point;
                    targetPosition.y = transform.position.y;
                    if (ActivateNewMovementSystem) NewMovement();
                }
            }      
        }

        ProcessQueue(); //Update production queue

        //déplacement nexus
        if (!ActivateNewMovementSystem)
        {
            if (targetPosition != transform.position && currentNexusState == statesNexus.Move)
            {
                OldMovement();
            }
        }
        else 
        {
            if (navM.hasPath && navM.remainingDistance < 1.5f)
            {
                navM.isStopped = true;
                navM.ResetPath();
                navM.isStopped = false;
            }
        }

        if (currentNexusState == statesNexus.Move) navM.isStopped = false;
        else
        {
            navM.isStopped = true;
            navM.ResetPath();
        }

        navM.speed = speed * NexusLevelManager.instance.GetVitesseNexus();
        SetFeedbackUI();
    }


    #region Movement

    private void OldMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime * NexusLevelManager.instance.GetVitesseNexus());
    }


    private void NewMovement()
    {
        navM.SetDestination(targetPosition);
    }

    #endregion


    public void DisplayRange(float range, Color color) //affiche le cercle autour du nexus 
    {
        lRBattery.positionCount = 50;
        lRBattery.useWorldSpace = false;
        lRBattery.SetColors(color, color);

        float x;
        float y = 0f + transform.position.y;
        float z;

        float angle = 20f;

        for (int i = 0; i < 50; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * range;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * range;

            lRBattery.SetPosition(i, new Vector3(x, y, z));

            angle += (360f / 49f);
        }
    }

    public void SetAnimator() // actualise l'animator
    {
        switch(currentNexusState)
        {
            case statesNexus.Move:
                animator.SetBool("Stop", false);
                break;

            case statesNexus.Immobilize:
                animator.SetBool("Stop", true);
                break;

            case statesNexus.ForcedImmobilize:
                animator.SetBool("Stop", true);
                break;
        }
    }

    public void SetNexusMaterial(Material newMaterial) //feedback visuel du nexus quand change de niveau
    {
        Material[] matList = new Material[2];
        matList[0] = nexusRenderers[0].materials[0];
        matList[1] = newMaterial;

        foreach (MeshRenderer e in nexusRenderers)
        {
            if (e.materials.Length == 2)
            {
                e.materials = matList;
            }
            else
            {
                e.material = newMaterial;
            }
        }
    }

    public void SetIdleAnimationSpeed(float newSpeed) 
    {
        animator.speed = 1 * newSpeed;
    }

    public void InitialisationRange()
    {
        Vector3 newScale = cookie.transform.localScale;
        newScale.x = radiusVision;
        newScale.z = radiusVision;
        cookie.transform.localScale = newScale;
    }
}
