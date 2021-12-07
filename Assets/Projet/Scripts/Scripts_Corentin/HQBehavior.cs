using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HQBehavior : Building
{
    public float BatType;
    public List<int> desiredRoaster;
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
                if (GetIsMovingRallyPoint())
                    if (Input.GetMouseButton(1))
                    {
                        RaycastHit hit;
                        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
                        SetRallyPoint(hit.point);
                        gameManager.GetComponent<SelectionPlayer>().canSelect = true;
                        SetIsMovingRallyPoint(false);
                    }

                if (Input.GetKeyDown(KeyCode.A))
                {
                    AddToQueue(0);
                }

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    AddToQueue(1);
                }   
            }

            ProcessQueue();
        }

        SetFeedbackUI();
    }
}
