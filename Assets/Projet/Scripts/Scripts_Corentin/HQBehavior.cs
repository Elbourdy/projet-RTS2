using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HQBehavior : Building
{
    public float BatType;
    public List<int> desiredRoaster;
    public int speed = 10;

    public int timerConsumption = 1, ressourcesConsumed = 1;
    private float timerConsumptionCount;

    private Vector3 targetPosition;
    // Start is called before the first frame update
    private void Awake()
    {
        foreach (int e in desiredRoaster)
        {
            AddToRoaster(e);
        }
    }

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfSelected();

        if (!GetIsConstructed())
        {
            CheckIfBuildingConstructed();

            if (Input.GetKeyDown(KeyCode.C))
            {
                SetConstructionHealth(GetConstructionHealth() + 100f);
            }
        }
        else
        {
            if (GetIsSelected())
            {
                if (Input.GetMouseButtonUp(1))
                {
                    if (GetIsMovingRallyPoint())
                    {
                        RaycastHit hit;
                        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
                        SetRallyPoint(hit.point);
                        gameManager.GetComponent<SelectionPlayer>().canSelect = true;
                        SetIsMovingRallyPoint(false);
                    }
                    else
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
            if (targetPosition != transform.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            }

            //consommation énergie Nexus

            if (timerConsumption < timerConsumptionCount)
            {
                if (Global_Ressources.instance.CheckIfEnoughRessources(0, ressourcesConsumed))
                {
                    Global_Ressources.instance.ModifyRessource(0, -ressourcesConsumed);
                }
                else
                {
                    Destroy(gameObject);
                    Debug.Log("Gameover");
                }
                timerConsumptionCount = 0f;
            }

            timerConsumptionCount += Time.deltaTime;






        }

        SetFeedbackUI();
    }
}
