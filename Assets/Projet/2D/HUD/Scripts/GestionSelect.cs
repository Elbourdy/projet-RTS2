using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionSelect : MonoBehaviour
{
    // Start is called before the first frame update
    public float NBRS;
    public GameObject A1;
    public GameObject A2;
    public GameObject A3;
    public GameObject A4;
    public GameObject A5;
    public GameObject A6;
    public GameObject A7;
    public GameObject A8;
    public GameObject A9;
    public GameObject A10;

    public GameObject A11;
    public GameObject A12;
    public GameObject A13;
    public GameObject A14;
    public GameObject A15;
    public GameObject A16;
    public GameObject A17;
    public GameObject A18;
    public GameObject A19;
    public GameObject A20;

    public GameObject A21;
    public GameObject A22;
    public GameObject A23;
    public GameObject A24;
    public GameObject A25;
    public GameObject A26;
    public GameObject A27;
    public GameObject A28;
    public GameObject A29;
    public GameObject A30;

   
    

    void Start()
    {
        NBRS = 0;
    }

    // Update is called once per frame
    void Update()
    {
        NBRS = GameObject.Find("GameManager").GetComponent<SelectionPlayer>().selectedUnits.Count;


        


        if (NBRS >= 2)
        {
            A1.SetActive(true);
        }
        else
        {
            A1.SetActive(false);
        }


        if (NBRS >= 3)
        {
            A2.SetActive(true);
        }
        else
        {
            A2.SetActive(false);
        }


        if (NBRS >= 4)
        {
            A3.SetActive(true);
        }
        else
        {
            A3.SetActive(false);
        }


        if (NBRS >= 5)
        {
            A4.SetActive(true);
        }
        else
        {
            A4.SetActive(false);
        }



        if (NBRS >= 6)
        {
            A5.SetActive(true);
        }
        else
        {
            A5.SetActive(false);
        }


        if (NBRS >= 7)
        {
            A6.SetActive(true);
        }
        else
        {
            A6.SetActive(false);
        }


        if (NBRS >= 8)
        {
            A7.SetActive(true);
        }
        else
        {
            A7.SetActive(false);
        }


        if (NBRS >= 9)
        {
            A8.SetActive(true);
        }
        else
        {
            A8.SetActive(false);
        }


        if (NBRS >= 10)
        {
            A9.SetActive(true);
        }
        else
        {
            A9.SetActive(false);
        }

        if (NBRS >= 11)
        {
            A10.SetActive(true);
        }
        else
        {
            A10.SetActive(false);
        }
        if (NBRS >= 12)
        {
            A11.SetActive(true);
        }
        else
        {
            A11.SetActive(false);
        }
        if (NBRS >= 13)
        {
            A12.SetActive(true);
        }
        else
        {
            A12.SetActive(false);
        }
        if (NBRS >= 14)
        {
            A13.SetActive(true);
        }
        else
        {
            A13.SetActive(false);
        }
        if (NBRS >= 15)
        {
            A14.SetActive(true);
        }
        else
        {
            A14.SetActive(false);
        }
        if (NBRS >= 16)
        {
            A15.SetActive(true);
        }
        else
        {
            A15.SetActive(false);
        }
        if (NBRS >= 17)
        {
            A16.SetActive(true);
        }
        else
        {
            A16.SetActive(false);
        }
        if (NBRS >= 18)
        {
            A17.SetActive(true);
        }
        else
        {
            A17.SetActive(false);

        }

        if (NBRS >= 19)
        {
            A18.SetActive(true);
        }
        else
        {
            A18.SetActive(false);
        }
        if (NBRS >= 20)
        {
            A19.SetActive(true);
        }
        else
        {
            A19.SetActive(false);
        }

        if (NBRS >= 21)
        {
            A20.SetActive(true);
        }
        else
        {
            A20.SetActive(false);
        }
        if (NBRS >= 22)
        {
            A21.SetActive(true);
        }
        else
        {
            A21.SetActive(false);
        }
        if (NBRS >= 23)
        {
            A22.SetActive(true);
        }
        else
        {
            A22.SetActive(false);
        }
        if (NBRS >= 24)
        {
            A23.SetActive(true);
        }
        else
        {
            A23.SetActive(false);
        }
        if (NBRS >= 25)
        {
            A24.SetActive(true);
        }
        else
        {
            A24.SetActive(false);
        }
        if (NBRS >= 26)
        {
            A25.SetActive(true);
        }
        else
        {
            A25.SetActive(false);
        }
        if (NBRS >= 27)
        {
            A26.SetActive(true);
        }
        else
        {
            A26.SetActive(false);
        }
        if (NBRS >= 28)
        {
            A27.SetActive(true);
        }
        else
        {
            A27.SetActive(false);
        }
        if (NBRS >= 29)
        {
            A28.SetActive(true);
        }
        else
        {
            A28.SetActive(false);
        }
        if (NBRS >= 30)
        {
            A29.SetActive(true);
        }
        else
        {
            A29.SetActive(false);
        }
        if (NBRS >= 31)
        {
            A30.SetActive(true);
        }
        else
        {
            A30.SetActive(false);
        }








    }
}
