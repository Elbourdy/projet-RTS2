using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NightAttack", menuName = "ScriptableObjects/Managers/NightAttack")]
public class NightAttackScriptable : ScriptableObject
{
    public int[] numSpawnerActive = new int[10];
    public int[] costByNight = new int[10];
    public int[] nightBeforeUnitSpawn = new int[4];
    public int[] customWaves = new int[50];
    public int height, width;
}
