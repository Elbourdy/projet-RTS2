using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Damien SAENZ

public class Shop_Cases : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject[] shopCases = new GameObject[12];
    private float nbrCases;


    void Ini()
    {
        foreach (GameObject e in shopCases)
        {
            e.SetActive(false);
        }
    }
    void Start()
    {
        shopCases = GameObject.FindGameObjectsWithTag("ShopCases");
        Ini();
    }

    // Update is called once per frame
    void Update()
    {
        nbrCases = GameObject.Find("Canvas").GetComponent<Gestion_HUD>().Cases;
        
        for (int i = 0; i < shopCases.Length; i++)
        {
            if (i < nbrCases)
            {
                shopCases[i].SetActive(true);
            }
            else
            {
                shopCases[i].SetActive(false);
            }
        }
    }
}
