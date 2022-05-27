using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionModule : MonoBehaviour
{
    public LosangeBehavior bigLosange, losangeSkill, losangeSkillToggle;
    public List<LosangeBehavior> losangeSelection = new List<LosangeBehavior>();
    public List<LosangeBehavior> losangeSup = new List<LosangeBehavior>();
    public List<LosangeBehavior> losangeShop = new List<LosangeBehavior>();
    public List<LosangeBehavior> waitingList = new List<LosangeBehavior>();
    public enum selected { Empty, Nexus, SoloAlly, SupEleven, InfEleven, Ennemi, Building, Ressources };
    public selected typeSelection = selected.Empty;
    public float hudOut, hudIn;
    public Image waitingBar;


    public static SelectionModule instance;
    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        NewSelectionManager.instance.onChangeSelection += CheckSelection;

        foreach (LosangeBehavior e in losangeSelection)
            e.HideText();

        ReinitialiseHUD();
        GetComponent<RectTransform>().localPosition = new Vector3(0, hudIn, 0);
    }

    private void OnDisable()
    {
        NewSelectionManager.instance.onChangeSelection -= CheckSelection;
    }

    private void LateUpdate() //update la selection en fonction de typeSelection
    {
        List<SelectableObject> list = NewSelectionManager.instance.SelectedObjects;

        switch (typeSelection)
        {
            case selected.Empty:
                break;

            case selected.Nexus:
                bigLosange.SetHealth(list[0].GetComponent<HealthSystem>().GetHealth() / list[0].GetComponent<HealthSystem>().GetMaxHealth(), list[0].GetComponent<HealthSystem>().GetHealth());
                break;

            case selected.InfEleven:

                for(int i = 0; i < list.Count; i++)
                {
                    if(list[i] != null)
                    {
                        losangeSelection[i].SetHealth(list[i].GetComponent<HealthSystem>().GetHealth() / list[i].GetComponent<HealthSystem>().GetMaxHealth(), 0);
                        losangeSelection[i].SetEnergy(list[i].GetComponent<HealthSystem>().GetBatteryHealth() / list[i].GetComponent<HealthSystem>().GetMaxBatteryHealth(), 0);
                        if (list[i].GetComponent<SpeAttackClass>() != null) losangeSelection[i].SetCooldown(list[i].GetComponent<SpeAttackClass>().GetRemainingTime());
                    }
                }
                break;

            case selected.Ennemi:
                bigLosange.SetHealth(list[0].GetComponent<HealthSystem>().GetHealth() / list[0].GetComponent<HealthSystem>().GetMaxHealth(), list[0].GetComponent<HealthSystem>().GetHealth());
                break;

            case selected.SoloAlly:
                bigLosange.SetHealth(list[0].GetComponent<HealthSystem>().GetHealth() / list[0].GetComponent<HealthSystem>().GetMaxHealth(), list[0].GetComponent<HealthSystem>().GetHealth());
                bigLosange.SetEnergy(list[0].GetComponent<HealthSystem>().GetBatteryHealth() / list[0].GetComponent<HealthSystem>().GetMaxBatteryHealth(), list[0].GetComponent<HealthSystem>().GetBatteryHealth());
                if (list[0].GetComponent<SpeAttackClass>() != null) bigLosange.SetCooldown(list[0].GetComponent<SpeAttackClass>().GetRemainingTime());
                break;
        }
    }

    public void CheckSelection() // check la selection actuelle
    {
        List<SelectableObject> list = NewSelectionManager.instance.SelectedObjects;

        if (list.Count == 0)
            typeSelection = selected.Empty;

        else if(list.Count == 1)
        {
            if (list[0].gameObject.GetComponent<ClassAgentContainer>() != null);
            {
                if (list[0].gameObject.GetComponent<Agent_Type>().Type == Agent_Type.TypeAgent.Ally)
                    typeSelection = selected.SoloAlly;
                else
                    typeSelection = selected.Ennemi;
            }

            if (list[0].gameObject.GetComponent<ClassBatimentContainer>() != null)
                if (list[0].gameObject.name == "Nexus")
                    typeSelection = selected.Nexus;
                else
                    typeSelection = selected.Building;
        }
        else
        {
            if (list.Count > 11)
                typeSelection = selected.SupEleven;
            else
                typeSelection = selected.InfEleven;
        }

        InitialiseSelection(list);
    }

    public void InitialiseSelection(List<SelectableObject> list) //initialise le HUD en fonction de typeSelection
    {
        ReinitialiseHUD();

        switch (typeSelection)
        {
            case selected.Empty:
                GetComponent<RectTransform>().localPosition = new Vector3(0, hudIn, 0);

                break;

            case selected.Nexus:
                bigLosange.gameObject.SetActive(true);
                bigLosange.ResetLosange();
                bigLosange.HideEnergy();
                bigLosange.SetSprite(list[0].GetComponent<ClassBatimentContainer>().myClass.unitSprite);

                List<AgentClass> rosterList = HQBehavior.instance.GetRoasterUnits();

                for (int i = 0; i < rosterList.Count; i++)
                {
                    losangeShop[i].HideEverything();
                    losangeShop[i].gameObject.SetActive(true);
                    losangeShop[i].gameObject.GetComponent<ButtonAS>().ID = i;
                    losangeShop[i].SetSprite(rosterList[i].unitSprite);
                    losangeShop[i].SetRightText(rosterList[i].ressourcesCost[0]);
                }

                //DRAPEAU

                losangeShop[4].HideEverything();
                losangeShop[4].gameObject.SetActive(true);
                losangeShop[4].SetSprite(HQBehavior.instance.gameObject.GetComponent<ClassBatimentContainer>().myClass.rallyPointSprite);

                break;

            case selected.InfEleven:
                for (int i = 0; i < list.Count; i++)
                {
                    if(list[i] != null)
                    {
                        losangeSelection[i].ResetLosange();
                        losangeSelection[i].gameObject.SetActive(true);
                        losangeSelection[i].HideText();
                        losangeSelection[i].SetSprite(list[i].GetComponent<ClassAgentContainer>().myClass.unitSprite);
                        losangeSelection[i].cooldown.gameObject.SetActive(true);
                        if (list[i].GetComponent<SpeAttackClass>() != null) losangeSelection[i].SetCooldown(list[i].GetComponent<SpeAttackClass>().GetRemainingTime());
                    }
                }
                break;

            case selected.SupEleven:
                int[] count = new int[4];

                for (int i = 0; i < list.Count; i++)
                {
                    count[list[i].GetComponent<ClassAgentContainer>().myClass.ID]++;
                }
                for(int i = 0; i < count.Length; i++)
                    if (count[i] != 0)
                    {
                        losangeSup[i].ResetLosange();
                        losangeSup[i].gameObject.SetActive(true);
                        losangeSup[i].SetSprite(GameDataStorage.instance.mainAgentClassStorage[i].unitSprite);
                        losangeSup[i].HideEnergy();
                        losangeSup[i].HideHealth();
                        losangeSup[i].SetRightText(count[i]);
                    }
                break;

            case selected.Ennemi:
                bigLosange.gameObject.SetActive(true);
                bigLosange.ResetLosange();
                bigLosange.HideEnergy();
                bigLosange.SetSprite(list[0].GetComponent<ClassAgentContainer>().myClass.unitSprite);
                bigLosange.IsEnnemi();
                break;

            case selected.SoloAlly:
                bigLosange.gameObject.SetActive(true);
                bigLosange.ResetLosange();
                bigLosange.SetSprite(list[0].GetComponent<ClassAgentContainer>().myClass.unitSprite);
                bigLosange.cooldown.gameObject.SetActive(true);
                if (list[0].GetComponent<SpeAttackClass>() != null) bigLosange.SetCooldown(list[0].GetComponent<SpeAttackClass>().GetRemainingTime());
                break;
        }
    }

    public void ReinitialiseHUD()
    {
        foreach(LosangeBehavior e in losangeSelection)
        {
            e.gameObject.SetActive(false);
        }
        foreach (LosangeBehavior e in losangeSup)
        {
            e.gameObject.SetActive(false);
        }

        foreach (LosangeBehavior e in losangeShop)
        {
            e.gameObject.SetActive(false);
        }

        foreach (LosangeBehavior e in waitingList)
        {
            e.gameObject.SetActive(false);
        }

        bigLosange.gameObject.SetActive(false);
        GetComponent<RectTransform>().localPosition = new Vector3(0, hudOut, 0);
        //tout set sur false
    }
}
