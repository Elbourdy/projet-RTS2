using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NightAttack", menuName = "ScriptableObjects/Managers/NightAttack")]
public class NightAttackScriptable : ScriptableObject
{
    public List<int> numSpawnerActive = new List<int>();
    public List<int> costByNight = new List<int>();
    public int[] nightBeforeUnitSpawn = new int[4];
    [SerializeField]public int[,] customWaves = new int[10, 5];
}
