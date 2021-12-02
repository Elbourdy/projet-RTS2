using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSquareStorage : MonoBehaviour
{
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
