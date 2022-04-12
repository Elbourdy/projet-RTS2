using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaliseBehavior : MonoBehaviour
{
    public Animator animatorCentral, animatorDoor;
    public RuntimeAnimatorController[] animatorTab;

    private NavMeshSurface navMesh;
    public enum statesBalise { Classic, CountDown, Simultaneous }
    [SerializeField] public statesBalise baliseBehavior = statesBalise.Classic;

    public List<SwitchBehavior> switchList = new List<SwitchBehavior>();
    public Transform spawnPoint;

    [DrawIf("baliseBehavior", statesBalise.CountDown)] public float cooldown = 5f;

    // Start is called before the first frame update
    void Awake()
    {
        navMesh = GameObject.Find("NavMeshSurface").GetComponent<NavMeshSurface>();

        foreach (SwitchBehavior e in switchList)
        {
            e.bB = this;
            e.SetSwitchType(baliseBehavior);

            if (baliseBehavior == statesBalise.CountDown)
                e.countdownTime = cooldown;
        }

        animatorCentral.runtimeAnimatorController = animatorTab[switchList.Count - 1];
    }

    // Update is called once per frame
    void Update()
    {
        SetCentralAnimator();
    }

    public void Switch()
    {
        int count = 0;
       /* switch (baliseBehavior)
        {
            case statesBalise.Classic:*/
                foreach (SwitchBehavior e in switchList)
                {
                    if (e.GetState()) count++;
                }
                if (count == switchList.Count)
                {
                    Open();
                }
            /*break;
        }*/
    }

    private void Open()
    {
        animatorDoor.SetBool("Open", true);
        SetCentralAnimator();
        navMesh.BuildNavMesh();
        foreach (SwitchBehavior e in switchList)
        Destroy(e);
        Destroy(this);
    }

    private void SetCentralAnimator()
    {
        if (Vector2.Distance(HQBehavior.instance.transform.position, transform.position) < 30)
            animatorCentral.SetBool("On", true);

        int countTotal = 0;
        foreach (SwitchBehavior e in switchList)
        {
            if (e.GetState()) countTotal++;
        }

        if (countTotal > 0)
        {
            animatorCentral.SetBool("Chris1", true);
            animatorCentral.SetBool("On", true);
        }

        else
            animatorCentral.SetBool("Chris1", false);

        if (switchList.Count > 1)
            if (countTotal > 1)
                animatorCentral.SetBool("Chris2", true);
            else
            animatorCentral.SetBool("Chris2", false);

        if (switchList.Count > 2)
            if (countTotal > 2)
            animatorCentral.SetBool("Chris3", true);
            else
            animatorCentral.SetBool("Chris3", false);
    }
}
