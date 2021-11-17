using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HQBehavior : Building
{
    public Transform spawnPoint, rallyPoint;
    public GameObject selectedFeedback;
    public HealthBar productionBar;
    public List<Image> recapProduction;

    // Start is called before the first frame update
    void Start()
    {
        SetSpawnPosition(spawnPoint.position);
        SetRallyPoint(rallyPoint.position);
        SetSelectedFeedback(selectedFeedback);
        SetProductionBar(productionBar);
        SetProductionRecap(recapProduction);
        AddToRoaster(0);
        AddToRoaster(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddToQueue(0);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            AddToQueue(1);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            SetIsSelected(!GetIsSelected());
        }

        SetSelectedFeedbackActive(GetIsSelected());

        ProcessQueue();

        SetRecapProductionTotal();


    }
}
