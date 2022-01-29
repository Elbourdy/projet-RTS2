using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Afficheboutique : MonoBehaviour
{



    public GameObject SelectCaseOne;
    public GameObject ConstuctionHUD;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       


        if (SelectCaseOne.activeInHierarchy == true)
        {

            ConstuctionHUD.SetActive(false);
           
        }
        else
        {
            ConstuctionHUD.SetActive(true);
            

        }

    }
}
