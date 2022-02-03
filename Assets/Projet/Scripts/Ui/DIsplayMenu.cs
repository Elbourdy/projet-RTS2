using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DIsplayMenu : MonoBehaviour
{

    public GameObject Menu;
    public GameObject EcrOption;
    public GameObject EcrCam;
    public GameObject EcrSelMiss;
    public GameObject EcrMult;
    public GameObject EcrRe;
    public GameObject EcreCrea;


    // Start is called before the first frame update
    void Start()
    {

        Menu.SetActive(true);
        EcrOption.SetActive(false);
        EcrCam.SetActive(false);
        EcrSelMiss.SetActive(false);
        EcrMult.SetActive(false);
        EcrRe.SetActive(false);
        EcreCrea.SetActive(false);

    }

  

    public void EcranOption()
    {
        Menu.SetActive(false);
        EcrOption.SetActive(true);
        EcrCam.SetActive(false);
        EcrSelMiss.SetActive(false);
        EcrMult.SetActive(false);
        EcrRe.SetActive(false);
        EcreCrea.SetActive(false);


    }

    public void EcranCampagne()
    {

        Menu.SetActive(false);
        EcrOption.SetActive(false);
        EcrCam.SetActive(true);
        EcrSelMiss.SetActive(false);
        EcrMult.SetActive(false);
        EcrRe.SetActive(false);
        EcreCrea.SetActive(false);


    }

    public void EcranSelectionMulti()
    {
        Menu.SetActive(false);
        EcrOption.SetActive(false);
        EcrCam.SetActive(false);
        EcrSelMiss.SetActive(true);
        EcrMult.SetActive(false);
        EcrRe.SetActive(false);
        EcreCrea.SetActive(false);


    }

    public void EcranMultijoueur()
    {

        Menu.SetActive(false);
        EcrOption.SetActive(false);
        EcrCam.SetActive(false);
        EcrSelMiss.SetActive(false);
        EcrMult.SetActive(true);
        EcrRe.SetActive(false);
        EcreCrea.SetActive(false);



    }

    public void EcranRecherche()
    {
        Menu.SetActive(false);
        EcrOption.SetActive(false);
        EcrCam.SetActive(false);
        EcrSelMiss.SetActive(false);
        EcrMult.SetActive(false);
        EcrRe.SetActive(true);
        EcreCrea.SetActive(false);


    }


    public void EcranCreation()
    {

        Menu.SetActive(false);
        EcrOption.SetActive(false);
        EcrCam.SetActive(false);
        EcrSelMiss.SetActive(false);
        EcrMult.SetActive(false);
        EcrRe.SetActive(false);
        EcreCrea.SetActive(true);

    }



}
