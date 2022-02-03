using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionMouse : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit[] hit = Physics.RaycastAll(ray);

        foreach (RaycastHit e in hit)
        {
            if (e.collider.tag == "Building")
            {
                e.collider.GetComponent<Building>();
            }
        }

    }
}
