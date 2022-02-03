using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_List : MonoBehaviour
{
    //le but de ce script est de permettre l'inventorisation ainsi que les permissions de contructions du joueur
    [SerializeField] private List<GameObject> buildingList = new List<GameObject>();

    public List<GameObject> AccessList() { return buildingList; }
    public void AddToList(GameObject building) { buildingList.Add(building); }
    public void RemoveFromList(GameObject building) { buildingList.Remove(building); }
}
