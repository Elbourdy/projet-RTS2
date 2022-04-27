using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCameraOOB : MonoBehaviour
{
    [SerializeField] private GameObject[] OOBPoints = new GameObject[4];


    private void LateUpdate()
    {
        if (transform.position.x > OOBPoints[2].transform.position.x)
        {
            var currentPos = transform.position;
            transform.position = new Vector3(OOBPoints[2].transform.position.x, transform.position.y, transform.position.z);
        }
        if (transform.position.x < OOBPoints[3].transform.position.x)
        {
            var currentPos = transform.position;
            transform.position = new Vector3(OOBPoints[3].transform.position.x, transform.position.y, transform.position.z);
        }

        if (transform.position.z > OOBPoints[0].transform.position.z)
        {
            var currentPos = transform.position;
            transform.position = new Vector3(transform.position.x, transform.position.y, OOBPoints[0].transform.position.z);
        }
        if (transform.position.z < OOBPoints[1].transform.position.z)
        {
            var currentPos = transform.position;
            transform.position = new Vector3(transform.position.x, transform.position.y, OOBPoints[1].transform.position.z);
        }
    }
}
