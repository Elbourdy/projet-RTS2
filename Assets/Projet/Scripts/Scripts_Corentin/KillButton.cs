using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillButton : MonoBehaviour
{
    private Image image;
    private float percentageRetrieve;

    void Start()
    {
        image = GetComponent<Image>();
        percentageRetrieve = HQBehavior.instance.refundPercentageUnit;
    }

    void Update()
    {
        if (SelectionPlayer.instance.selectedUnits.Count > 0 && SelectionPlayer.instance.selectedUnits[0].GetComponent<ClassAgentContainer>() != null)
        {
            image.enabled = true;
        }
        else
        {
            image.enabled = false;
        }

    }

    private void OnButtonPressed()
    {
        List<GameObject> list = SelectionPlayer.instance.selectedUnits;
        float refundTotal = 0f;

        for(int i = 0; i < list.Count; i++)
        {
            refundTotal += list[i].GetComponent<ClassAgentContainer>().myClass.ressourcesCost[0];
        }
        Global_Ressources.instance.ModifyRessource(0, Mathf.RoundToInt(refundTotal * percentageRetrieve));

        foreach (GameObject e in list)
        {
            list.RemoveAt(0);
            Destroy(e);
        }
    }
}
