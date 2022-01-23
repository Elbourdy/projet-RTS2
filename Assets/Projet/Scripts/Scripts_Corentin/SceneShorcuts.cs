using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneShorcuts : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Global_Ressources.instance.ModifyRessource(0, 100);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Global_Ressources.instance.ModifyRessource(0, -100);
        }
    }
}
