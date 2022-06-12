using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class UpdatePosRessourceVFX : MonoBehaviour
{
    public UpdatePosTotemVfx.positionsVFX myPos;

    [SerializeField] private Transform nexusPos;
    [SerializeField] private VisualEffect myVFX;

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
        if (myVFX.isActiveAndEnabled)
        {
            myPos.pos3.position = nexusPos.position;
            var newPos2 = (myPos.pos1.position + myPos.pos3.position) / 2;
            myPos.pos2.position = newPos2;
        }
    }
}
