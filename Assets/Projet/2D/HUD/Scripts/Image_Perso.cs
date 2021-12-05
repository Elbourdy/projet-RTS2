using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Damien SAENZ

public class Image_Perso : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite SP0, SP1, SP2, SP3, SP10;
    private float identity;
    public Text vie;
    public string valeurvie;

    void Start()
    {
        gameObject.GetComponent<Image>().sprite = SP0;
    }

    // Update is called once per frame
    void Update()
    {
         
        identity = GameObject.Find("Canvas").GetComponent<Gestion_HUD>().Perso;
        vie.GetComponent<Text>().text = valeurvie;

        if (identity == 1)
        {
            gameObject.GetComponent<Image>().sprite = SP1;
        }
        else if (identity == 2)
        {
            gameObject.GetComponent<Image>().sprite = SP2;
        }
        else if (identity == 10)
        {
            gameObject.GetComponent<Image>().sprite = SP10;
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = SP0;
        }
        
    }
}
