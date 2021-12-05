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
        //DisplayRoaster();
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
                SetConstructionHealth(GetConstructionHealth() + 10f);
            }
        }

        else
        {
            if (GetIsSelected())
            {
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
        DisplayRoaster();
    }
}
