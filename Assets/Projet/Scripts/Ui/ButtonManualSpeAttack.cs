using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonManualSpeAttack : MonoBehaviour
{
    [SerializeField] private GameObject button;

    private void OnEnable()
    {
        NewSelectionManager.instance.onChangeSelection += CheckSelectionList;
        SpeAttackUse.instance.OnVisualUseChange += ChangeText;
        button.SetActive(false);
    }

    private void OnDisable()
    {
        NewSelectionManager.instance.onChangeSelection -= CheckSelectionList;
        SpeAttackUse.instance.OnVisualUseChange -= ChangeText;
    }


    private void ChangeText()
    {
        if (SpeAttackUse.instance.isUsingVisualAttack)
        {
            button.GetComponentInChildren<TextMeshProUGUI>().text = "Lancement manuel ON";
        } 

        else
        {
            button.GetComponentInChildren<TextMeshProUGUI>().text = "Lancement manuel OFF";
        }
    }



    private void CheckSelectionList()
    {
        if (NewSelectionManager.instance.SelectedObjects.Count > 0)
        {
            foreach (var item in NewSelectionManager.instance.SelectedObjects)
            {
                if (item.GetComponent<SpeAttackClass>())
                {
                    button.SetActive(true);
                    Debug.Log(item);
                    break;
                }

                else
                {
                    button.SetActive(false);
                }
            }
            Debug.Log("Check Selection");
        }

        else
        {
            button.SetActive(false);
        }

    }
}
