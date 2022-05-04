using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonToggleSpeAttack : MonoBehaviour
{


    [SerializeField] private GameObject button;



    private void OnEnable()
    {
        NewSelectionManager.instance.onChangeSelection += CheckSelectionList;
        button.SetActive(false);
    }

    private void OnDisable()
    {
        NewSelectionManager.instance.onChangeSelection -= CheckSelectionList;
    }


    public void ActivateButton()
    {
        if (NewSelectionManager.instance.SelectedObjects.Count > 0)
        {
            foreach (var item in NewSelectionManager.instance.SelectedObjects)
            {
                CheckAndToggleSpeAttack(item);
            }
        }
    }


    private void CheckAndToggleSpeAttack(SelectableObject selectGo)
    {
        if (selectGo.gameObject.TryGetComponent(out ToggleSpeAttack myToggle))
        {
            myToggle.toggleButtonSpeAttack();
            if (myToggle.GetStateAuto())
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = "Atk Spéciale Auto ON";
            }
            else
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = "Atk Spéciale Auto OFF";
            }
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
