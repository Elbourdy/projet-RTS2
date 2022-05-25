using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonToggleSpeAttack : MonoBehaviour
{
    public Sprite spriteOn, spriteOff;

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
                button.GetComponent<Image>().sprite = spriteOn;
            }
            else
            {
                button.GetComponent<Image>().sprite = spriteOff;
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

                    if (item.GetComponent<ToggleSpeAttack>().GetStateAuto())
                    {
                        button.GetComponent<Image>().sprite = spriteOn;
                    }
                    else
                    {
                        button.GetComponent<Image>().sprite = spriteOff;
                    }
                    break;
                }

                else
                {
                    button.SetActive(false);
                }
            }
        }

        else
        {
            button.SetActive(false);
        }

    }

}
