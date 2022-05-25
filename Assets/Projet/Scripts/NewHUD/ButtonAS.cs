using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonAS : MonoBehaviour
{
    public enum typeButton {ShopCase, Skill, Selection, GroupSelection, Flag, WaitingList }
    public typeButton type = typeButton.ShopCase;

    public int ID;
    //private SelectionPlayer selectionPlayer;
    private NewSelectionManager selectionManager;

    void Start()
    {
        //selectionPlayer = GameObject.Find("GameManager").GetComponent<SelectionPlayer>();
        selectionManager = NewSelectionManager.instance;
    }

    public void ButtonPress()
    {
        switch(type)
        {
            case typeButton.ShopCase:
                if (HQBehavior.instance.SetActionShopCases(ID))
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Action/UI_Act_Click/UI_Act_Click");
                }
                else
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Action/UI_Act_NotPossible/UI_Act_Not");
                }
                break;

            case typeButton.Flag:
                HQBehavior.instance.MoveFlagInit();
                break;

            case typeButton.WaitingList:
                HQBehavior.instance.RemoveFromQueue(ID);
                break;

        }
    }
}
