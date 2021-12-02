using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonAS : MonoBehaviour
{

    public float ID, Perso;
    public Sprite SP, SP2, SP3, SP4, SP10;
        

    void Start()
    {
        
    }

   public  void buttonpress()
    {

        if (Perso == 0)
        {

        }
        else if (Perso == 1)
        {


        }
        else if (Perso == 2)
        {


        }

        else if (Perso == 10)
        {
            if (ID == 1)
            {
                try
                {
                    GameObject.Find("GameManager").GetComponent<SelectionPlayer>().selectedUnits[0].GetComponent<Building>().AddToQueue(0);
                }
                catch
                {
                    print("error");
                }
            }
            else if (ID == 2)
            {

            
            }
            else if (ID == 3)
            {

                
            }
            else if (ID == 4)
            {


            }
            else
            {

            }

        }
        else
        {

        }


    }

    // Update is called once per frame
    void Update()
    {
        
            Perso = GameObject.Find("Canvas").GetComponent<Gestion_HUD>().Perso;

     
        


    }
}
