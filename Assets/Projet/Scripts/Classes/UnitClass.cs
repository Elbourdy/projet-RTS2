using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Unit", menuName = "ScriptableObjects/Unit")]
public class UnitClass : ScriptableObject
{
    [Header("Identité")]
    public int ID;
    public string nameAgent;
    [Header("Valeurs de combats et exploration")]
    public float health = 1;
}
