using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMatR : MonoBehaviour
{
    // Start is called before the first frame update
    public Material A, B;
    Renderer rend;

    public GameObject Ressource;
    

    public float indic;
    void Start()
    {
        rend = GetComponent<Renderer>();
        indic = 100;
    }

    // Update is called once per frame
    /*void Update()
    {
        indic = Ressource.GetComponent<RessourcesObject>().ValeurRestante;

        
    }*/

    public void FeedbackRessourcesCrystal(float n)
    {
        rend.material.Lerp(B, A, n);
    }
}
