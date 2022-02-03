using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScreen : MonoBehaviour
{
    //affiche / enl�ve l'�cran de tuto 

    public void EnableTutorialScreen()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void DisableTutorialScreen()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Action/UI_Act_Click/UI_Act_Click");
    }

}
