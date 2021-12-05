using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndividualCase : MonoBehaviour
{
    // Start is called before the first frame update

    public int ID;
    public Sprite SP0, SP1, SP2, SP3;
    private float Nvie;
    public int RID;
    public GameObject Stocage;
    public GameObject txt;


    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        

        if (GameObject.Find("SelectionLisrt").GetComponent<GestionSelect>().NBRS >= ID)
        {


            try
            {
                RID = Stocage.GetComponent<SelectionPlayer>().selectedUnits[ID].GetComponent<ClassAgentContainer>().myClass.ID; //// modif coco
            }
            catch 
            {
                print("error");
            }


            try
            {
                Nvie = Stocage.GetComponent<SelectionPlayer>().selectedUnits[ID].GetComponent<HealthSystem>().GetHealth();
            }
            catch
            {
                print("error");
            }



            txt.GetComponent<Text>().text = "" + Nvie;



            if (RID >= 0)
                {
                /*if (RID == 0)
                {
                    gameObject.GetComponent<Image>().sprite = SP0;
                }
                else if (RID == 1)
                {
                    gameObject.GetComponent<Image>().sprite = SP1;

                }
                else if (RID == 2)
                {
                    gameObject.GetComponent<Image>().sprite = SP2;
                }

                else if (RID == 3)
                {
                    gameObject.GetComponent<Image>().sprite = SP3;

                }
                else
                {
                    gameObject.GetComponent<Image>().sprite = SP0;
                }*/

                GetComponent<Image>().sprite = GameObject.Find("GameManager").GetComponent<GameDataStorage>().mainAgentClassStorage[RID].unitSprite; ////modif coco

                }
                else
                {

                    gameObject.GetComponent<Image>().sprite = SP0;
                }

           




        }
        else
        {

            gameObject.GetComponent<Image>().sprite = null;
        }


    }
}
