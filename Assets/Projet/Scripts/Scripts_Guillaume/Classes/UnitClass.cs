using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Unit", menuName = "ScriptableObjects/Unit")]
public class UnitClass : ScriptableObject
{
    public string name;
    public float health = 1;
    bool isSelected = false;
}
