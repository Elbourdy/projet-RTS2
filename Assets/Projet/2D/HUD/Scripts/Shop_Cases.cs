using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Damien SAENZ

public class Shop_Cases : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject C1, C2, C3, C4, C5, C6, C7, C8, C9, C10, C11, C12;
    private float NbrCases;


    void Ini()
    {
        C1.SetActive(false);
        C2.SetActive(false);
        C3.SetActive(false);
        C4.SetActive(false);
        C5.SetActive(false);
        C6.SetActive(false);
        C7.SetActive(false);
        C8.SetActive(false);
        C9.SetActive(false);
        C10.SetActive(false);
        C11.SetActive(false);
        C12.SetActive(false);
        

    }
    void Start()
    {
        C1 = GameObject.Find("Case1");
        C2 = GameObject.Find("Case2");
        C3 = GameObject.Find("Case3");
        C4 = GameObject.Find("Case4");
        C5 = GameObject.Find("Case5");
        C6 = GameObject.Find("Case6");
        C7 = GameObject.Find("Case7");
        C8 = GameObject.Find("Case8");
        C9 = GameObject.Find("Case9");
        C10 = GameObject.Find("Case10");
        C11 = GameObject.Find("Case11");
        C12 = GameObject.Find("Case12");
    }

    // Update is called once per frame
    void Update()
    {
        NbrCases = GameObject.Find("Canvas").GetComponent<Gestion_HUD>().Cases;
        
        if (NbrCases >= 1)
        {
            C1.SetActive(true);
        }
        else
        {
            C1.SetActive(false);
        }

        if (NbrCases >= 2)
        {
            C2.SetActive(true);
        }
        else
        {
            C2.SetActive(false);
        }

        if (NbrCases >= 3)
        {
            C3.SetActive(true);
        }
        else
        {
            C3.SetActive(false);
        }

        if (NbrCases >= 4)
        {
            C4.SetActive(true);
        }
        else
        {
            C4.SetActive(false);
        }

        if (NbrCases >= 5)
        {
            C5.SetActive(true);
        }
        else
        {
            C5.SetActive(false);
        }

        if (NbrCases >= 6)
        {
            C6.SetActive(true);
        }
        else
        {
            C6.SetActive(false);
        }

        if (NbrCases >= 7)
        {
            C7.SetActive(true);
        }
        else
        {
            C7.SetActive(false);
        }

        if (NbrCases >= 8)
        {
            C8.SetActive(true);
        }
        else
        {
            C8.SetActive(false);
        }

        if (NbrCases >= 9)
        {
            C9.SetActive(true);
        }
        else
        {
            C9.SetActive(false);
        }

        if (NbrCases >= 10)
        {
            C10.SetActive(true);
        }
        else
        {
            C10.SetActive(false);
        }

        if (NbrCases >= 11)
        {
            C11.SetActive(true);
        }
        else
        {
            C11.SetActive(false);
        }

        if (NbrCases >= 12)
        {
            C12.SetActive(true);
        }
        else
        {
            C12.SetActive(false);
        }


    }
}
