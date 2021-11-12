using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float health = 1f;

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
}
