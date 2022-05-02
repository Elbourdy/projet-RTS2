using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeAttackUse : MonoBehaviour
{

    [SerializeField] private GameObject visualAttack;

    [SerializeField] private bool canCreateAttack = false;
    [SerializeField] private bool isUsingVisualAttack = false;

    [SerializeField] private GameObject exemple;

    public Color[] colorFeedback;


    private void Update()
    {
        if (isUsingVisualAttack)
        {
            SpawnVisualSpeAttack();
        }
    }




    public void SpawnVisualSpeAttack()
    {
        Vector3 newPosition = GetVisualPositionFromMouse();

        if (visualAttack == null)
        {
            visualAttack = GameObject.Instantiate(exemple, newPosition, Quaternion.identity) as GameObject;
        }

        else
        {
            visualAttack.transform.position = newPosition;
        }



        

    }


    private Vector3 GetVisualPositionFromMouse()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.layer == LayerMask.GetMask("Ground"))
                canCreateAttack = true;

            else canCreateAttack = false;
        }

        return hit.point;
    }


}
