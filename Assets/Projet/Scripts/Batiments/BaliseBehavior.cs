using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaliseBehavior : MonoBehaviour
{
    private NavMeshSurface navMesh;
    public enum statesBalise { Classic, CountDown, Simultaneous }
    [SerializeField] public statesBalise baliseBehavior = statesBalise.Classic;

    public List<SwitchBehavior> switchList = new List<SwitchBehavior>();
    public GameObject door;

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
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Destroy(door);
        navMesh.BuildNavMesh();
        foreach (SwitchBehavior e in switchList)
        Destroy(e);
        Destroy(this);
    }
}
