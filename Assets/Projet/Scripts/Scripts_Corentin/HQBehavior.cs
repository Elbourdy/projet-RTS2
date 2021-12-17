using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HQBehavior : Building
{
    public float BatType;
    public List<int> desiredRoaster;
    public int speed = 10;

    public int timerConsumption = 1, ressourcesConsumed = 1, energyRestored = 1, rangeRefill = 10;
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

        SetConstructionHealth(100f); //FOR NEXUS
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
                    RestockUnitEnergy();
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

    public void RestockUnitEnergy() //refill ally battery
    {
        RaycastHit[] hit = Physics.SphereCastAll(transform.position, rangeRefill, transform.forward);

        foreach(RaycastHit e in hit)
        {
            if (e.transform.GetComponent<ClassAgentContainer>() != null)
            {
                if (e.transform.GetComponent<AIEnemy>() == null)
                {
                    e.transform.GetComponent<HealthSystem>().ChangeBatteryHealth(energyRestored);
                }
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, rangeRefill);
    }
}
