using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP_RESSOURCECHEAT : MonoBehaviour
{
    [SerializeField] Global_Ressources ressourcemanager;
    void Update()
    {
        //un exemple de comment appeler le ressourcemanager;
        if (Input.GetKeyDown(KeyCode.A)) ressourcemanager.ModifyRessource(0, 100);
        if (Input.GetKeyDown(KeyCode.Z)) ressourcemanager.ModifyRessource(1, 100);
        if (Input.GetKeyDown(KeyCode.E)) ressourcemanager.ModifyRessource(2, 100);
        if (Input.GetKeyDown(KeyCode.R)) ressourcemanager.ModifyRessource(3, 100);
    }
}