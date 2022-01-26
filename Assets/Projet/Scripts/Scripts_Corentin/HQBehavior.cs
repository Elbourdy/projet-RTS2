using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HQBehavior : Building
{
    public static HQBehavior instance;

    public enum statesNexus {Move, Immobilize, ForcedImmobilize}
    public statesNexus currentNexusState = statesNexus.Move;

    public float BatType;
    public List<int> desiredRoaster;
    public int speed = 10;

    public Color colorRadiusBattery = Color.blue;

    private Vector3 targetPosition;
    private LineRenderer lRBattery;

    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this; 
    }

    void Start()
    {
        lRBattery = GetComponent<LineRenderer>();

        foreach (int e in desiredRoaster)
        {
            AddToRoaster(e);
        }

        targetPosition = transform.position;

        SetConstructionHealth(100f); //FOR NEXUS
        DisplayRange(BatteryManager.instance.radiusBattery * NexusLevelManager.instance.GetMultiplicatorRangeNexus(), colorRadiusBattery);
    }

    // Update is called once per frame
    void Update()
    {
        DisplayRange(BatteryManager.instance.radiusBattery * NexusLevelManager.instance.GetMultiplicatorRangeNexus(), colorRadiusBattery);
        SetAnimator();

        CheckIfSelected();

        if (GetIsSelected())
        {
            Debug.Log("IsSelected");
            if (Input.GetMouseButtonUp(1))
            {
                if (GetIsMovingRallyPoint())
                {
                    RaycastHit hit;
                    Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
                    SetRallyPoint(hit.point);
                    SelectionPlayer.instance.canSelect = true;
                    SetIsMovingRallyPoint(false);
                }
                else // means moving nexus
                {
                    RaycastHit hit;
                    Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
                    targetPosition = hit.point;
                    targetPosition.y = transform.position.y;
                }
            }      
        }

        ProcessQueue();

        //déplacement nexus
        if (targetPosition != transform.position  && currentNexusState == statesNexus.Move)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime * NexusLevelManager.instance.GetVitesseNexus());
        }

        SetFeedbackUI();
    }

    public void DisplayRange(float range, Color color) 
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

    public void SetAnimator()
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
}
