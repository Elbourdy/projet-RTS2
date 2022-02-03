using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSquareStorage : MonoBehaviour
{
    //stocke un int sur les boutons du shop

    private int IDNumber;
    
    public void SetIDNumber(int IDNumber)
    {
        this.IDNumber = IDNumber;
    }

    public int GetIDNumber(int IDNumber)
    {
        return IDNumber;
    }
}
