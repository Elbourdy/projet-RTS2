using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class UpdateNavOnSpawn : MonoBehaviour
{

    public NavMeshSurface surface;

    private void Awake()
    {
        surface.BuildNavMesh();
    }

}
