using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Damien SAENZ

public class Image_Map : MonoBehaviour
{
    private float NumMap;
    public Sprite Mp1, Mp2, Mp3;

    void Start()
    {
        
    }

    // Update is called once per frame-
    void Update()
    {
        NumMap = GameObject.Find("Canvas").GetComponent<Gestion_HUD>().Map;

        if (NumMap == 1)
        {
            gameObject.GetComponent<Image>().sprite = Mp1;
        }
        else if (NumMap == 2)
        {
            gameObject.GetComponent<Image>().sprite = Mp2;
        }
        else if (NumMap == 3)
        {
            gameObject.GetComponent<Image>().sprite = Mp3;
        }

    }
}
