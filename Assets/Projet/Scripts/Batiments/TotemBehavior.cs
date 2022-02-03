using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemBehavior : MonoBehaviour
{
    //script du totem
    //ajouter une verif au S2 si le joueur a déja l'unité ajoutée

    [SerializeField] private int rosterUnit;
    [SerializeField] private float timeToCollect;
    [SerializeField] private float rangeCollection;

    private float count;
    private bool activated = true;


    void Update()
    {
        if (!activated)
            return;

        if (Vector3.Distance(HQBehavior.instance.gameObject.transform.position, transform.position) < rangeCollection)
        {
            count += Time.deltaTime;
            if (count > timeToCollect)
            {
                HQBehavior.instance.AddToRoaster(rosterUnit);
                activated = false;
            }
        }
        else
        {
            count = 0;
        }
    }
}
