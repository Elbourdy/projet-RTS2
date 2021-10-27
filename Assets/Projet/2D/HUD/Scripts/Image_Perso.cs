using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Damien SAENZ

public class Image_Perso : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite SP0, SP1, SP2;
    private float identity;

    void Start()
    {
        gameObject.GetComponent<Image>().sprite = SP0;

    }

    // Update is called once per frame
    void Update()
    {

        identity = GameObject.Find("Canvas").GetComponent<Gestion_HUD>().Perso;

        if (identity == 1)
        {
            gameObject.GetComponent<Image>().sprite = SP1;
        }
        else if (identity == 2)
        {
            gameObject.GetComponent<Image>().sprite = SP2;
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = SP0;
        }
        
    }
}
