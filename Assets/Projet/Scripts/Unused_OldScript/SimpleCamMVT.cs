using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SimpleCamMVT : MonoBehaviour
{
    //A attacher au gameobject de la caméra, il en créera une sinon.
    [SerializeField]float camSpeed;
    void Update()
    {
        Vector3 displacement = new Vector3();
        if (Input.GetKey(KeyCode.UpArrow)) displacement.z++;
        if (Input.GetKey(KeyCode.DownArrow)) displacement.z--;
        if (Input.GetKey(KeyCode.LeftArrow)) displacement.x--;
        if (Input.GetKey(KeyCode.RightArrow)) displacement.x++;
        transform.position += displacement * camSpeed * Time.deltaTime;
    }
}
