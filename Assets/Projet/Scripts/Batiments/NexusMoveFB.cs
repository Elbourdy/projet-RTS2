using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NexusMoveFB : MonoBehaviour
{
    public float speedToRotate, heightVariation = 1f, speedVariation;
    float currentHeight;
    bool up = true;

    private void Start()
    {
        currentHeight = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        UpAndDown();
    }

    private void UpAndDown()
    {
        if(TickManager.instance.dayState == TickManager.statesDay.Day)
        {
            if (up)
                transform.position += Vector3.up * heightVariation * speedVariation * Time.deltaTime;
            else
                transform.position -= Vector3.up * heightVariation * speedVariation * Time.deltaTime;

            if (currentHeight + heightVariation < transform.localPosition.y) up = false;
            if (currentHeight - heightVariation > transform.localPosition.y) up = true;
        }
    }

    private void Rotate()
    {
        if (HQBehavior.instance.gameObject.GetComponent<NavMeshAgent>().path.corners.Length > 1)
        {
            Vector3 posToLook = HQBehavior.instance.gameObject.GetComponent<NavMeshAgent>().path.corners[1];
            posToLook.y = transform.position.y;

            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (posToLook-transform.position).normalized, speedToRotate*Time.deltaTime, 0f));
            //transform.LookAt(posToLook);
        }
    }
}