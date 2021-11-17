using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Test_mvt : MonoBehaviour
{
    public GameObject GO;
    NavMeshAgent NAV;
    void Update()
    {
        GetComponent<NavMeshAgent>().SetDestination(GO.transform.position);
    }
}
