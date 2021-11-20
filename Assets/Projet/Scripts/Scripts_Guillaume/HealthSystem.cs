using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthSystem : MonoBehaviour
{
    private float health;

    private void OnEnable()
    {
        //check la classe de l'objet en question et récupère la valeur dans la classe
        if (GetComponent<ClassAgentContainer>() != null) health = GetComponent<ClassAgentContainer>().myClass.health;
    }


    public void HealthChange(float damageNumber)
    {
        health += damageNumber;
        CheckIfKill();
    }

    private void CheckIfKill()
    {
        if (health <= 0)
        {
            KillGo();
        }
    }

    private void KillGo()
    {
        Destroy(gameObject);
    }


    public float GetHealth()
    {
        return health;
    }
}
