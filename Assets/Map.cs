using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject GMap;


    public void OnMapActive()
    {

        GMap.SetActive(true);
    }

    public void OnMapClose()
    {

        GMap.SetActive(false);
    }
}
