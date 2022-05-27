using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.EventSystems;

public class SpeAttackUse : MonoBehaviour
{

    public static SpeAttackUse instance;

    [SerializeField] private GameObject visualAttack;
    [SerializeField] private GameObject tmpTargetSetupTemplate;


    // Permet de créer l'attaque de nos unités sur le sol
    [SerializeField] private bool canCreateAttack = false;

    // Permet d'activer le mode de visualisation
    public bool isUsingVisualAttack = false;

    // Permet d'activer le script quand on sélectionne une unité avec attaque spéciale
    [SerializeField] private bool launchBehaviourAndControl = false;


    [System.Serializable]
    public struct atkSpeData
    {
       public GameObject visual;
       public GameObject trueAtk;
    }

    public atkSpeData[] allAtkSpeData;

    public delegate void SpeAttackUseEvent();
    public SpeAttackUseEvent OnVisualUseChange;


    private void Awake()
    {
        instance = this;
        NewSelectionManager.instance.onChangeSelection += CheckSelectionList;
    }


    private void Update()
    {
        if (launchBehaviourAndControl)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                ChangeBoolIsVisualAttack();
            }
            if (isUsingVisualAttack)
            {
                SpawnVisualSpeAttack();
            }

            if (Input.GetMouseButtonDown(0) && canCreateAttack)
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                    LaunchAttackOrder();
            }
        }
    }

    


    public void CheckSelectionList()
    {

        if (NewSelectionManager.instance.SelectedObjects.Count == 0)
        {
            isUsingVisualAttack = false;
            launchBehaviourAndControl = false;
            if (visualAttack != null)
            {
                Destroy(visualAttack);
            }
        }

        foreach (var item in NewSelectionManager.instance.SelectedObjects)
        {
            if (item.GetComponent<SpeAttackClass>())
            {
                launchBehaviourAndControl = true;
                break;
            }

            else
            {
                launchBehaviourAndControl = false;
            }
        }
    }

    private void LaunchAttackOrder()
    {

        foreach (var item in NewSelectionManager.instance.SelectedObjects)
        {
            AgentClass.AgentSpe speItem = AgentClass.GetSpe(item.gameObject);
            if (speItem != AgentClass.AgentSpe.Scout && speItem != AgentClass.AgentSpe.None)
            {

                Vector3 newPosition = GetVisualPositionFromMouse();
                var target = GameObject.Instantiate(tmpTargetSetupTemplate, newPosition, Quaternion.identity) as GameObject;
                target.name = "TmpTargetSetup";
                var statesItem = item.gameObject.GetComponent<AgentStates>();
                statesItem.ChangeAttackValue(statesItem.container.myClass.attackRangePoison, statesItem.container.myClass.damagePoison);
                statesItem.SetTarget(target);
                statesItem.SetState(AgentStates.states.Aggressive);
                statesItem.MoveAgent(target.transform.position);
            }
        }


       ChangeBoolIsVisualAttack();
       canCreateAttack = false;

    }

    public void ChangeBoolIsVisualAttack()
    {
        isUsingVisualAttack = !isUsingVisualAttack;
        OnVisualUseChange?.Invoke();
        canCreateAttack = false;
        if (!isUsingVisualAttack)
        {
            DeleteFeedBack();
        }
    }

    public void SpawnVisualSpeAttack()
    {
        Vector3 newPosition = GetVisualPositionFromMouse();

        if (visualAttack == null)
        {
            visualAttack = GameObject.Instantiate(allAtkSpeData[0].visual, newPosition, Quaternion.identity) as GameObject;
        }

        else
        {
            visualAttack.transform.position = newPosition;
        }


        ChangeColorFeedBack();

    }


    public void DeleteFeedBack()
    {
        if (visualAttack != null)
        {
            Destroy(visualAttack.gameObject);
            visualAttack = null;
        }
    }


    private Vector3 GetVisualPositionFromMouse()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.layer == 3)
            {
                canCreateAttack = true;
            }

            else
            {
                canCreateAttack = false;
            }
        }
        Debug.Log(hit.collider.gameObject.layer + " " + LayerMask.GetMask("Ground"));


        return hit.point;
    }


    private void ChangeColorFeedBack()
    {
        var vfxComp = visualAttack.transform.GetComponentInChildren<VisualEffect>();

        if (canCreateAttack)
            vfxComp.SetVector4("CircleColor", Color.white * 3.4f);

        else vfxComp.SetVector4("CircleColor", Color.red * 3.4f);
    }

}
