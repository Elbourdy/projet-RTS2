using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePosTotemVfx : MonoBehaviour
{
    [System.Serializable]
    public struct positionsVFX
    {
        public Transform pos1;
        public Transform pos2;
        public Transform pos3;
    };


    public positionsVFX[] myVFX;


    [SerializeField] private Transform nexusPos;


    private void OnEnable()
    {
        if (nexusPos == null) nexusPos = GameObject.Find("Icosphere_Nexus").transform;
    }

    private void Update()
    {
        UpdatePos();
    }

    private void UpdatePos()
    {
        foreach (var item in myVFX)
        {
            item.pos3.position = nexusPos.position;
        }
    }

}
