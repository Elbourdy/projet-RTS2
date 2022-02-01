using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneShorcuts : MonoBehaviour
{
    private bool lockCamera = false;
    private bool enabledShortcuts = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) //lock/delock camera
        {
            if (enabledShortcuts)
            {
                enabledShortcuts = false;
            }
            else
            {
                enabledShortcuts = true;
            }
        }

        if (enabledShortcuts)
        {
            if (Input.GetKeyDown(KeyCode.R)) // reset scène
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (Input.GetKeyDown(KeyCode.A))  // add ressources
            {
                Global_Ressources.instance.ModifyRessource(0, 100);
            }

            if (Input.GetKeyDown(KeyCode.Z)) // remove ressources
            {
                Global_Ressources.instance.ModifyRessource(0, -100);
            }

            if (Input.GetKeyDown(KeyCode.Q))  // slow down time (too much = error)
            {
                Time.timeScale -= 0.2f;
            }

            if (Input.GetKeyDown(KeyCode.S))  // set time back to normal
            {
                Time.timeScale = 1f;
            }

            if (Input.GetKeyDown(KeyCode.D)) // speed up time
            {
                Time.timeScale += 1f;
            }

            if (Input.GetKeyDown(KeyCode.Space)) //lock/delock camera
            {
                if (lockCamera)
                {
                    Camera.main.GetComponent<CameraMouvement>().enabled = true;
                    lockCamera = false;
                }
                else
                {
                    Camera.main.GetComponent<CameraMouvement>().enabled = false;
                    lockCamera = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.W)) // launch night attack
            {
                TickManager.instance.LaunchNightAttack();
            }

            if (Input.GetKeyDown(KeyCode.X)) // cancel night attack
            {
                NightAttack.instance.CancelNightAttack();
            }
        }
        
    }
}
