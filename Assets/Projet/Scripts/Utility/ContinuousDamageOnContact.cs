using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousDamageOnContact : MonoBehaviour
{

    public Agent_Type.TypeAgent typeToDamage;
    public float damage = 1f;
    public float damageTick = 1f;

    public bool canDamage = false;


    private float timerTick = 0f;


    private void Awake()
    {
        canDamage = true;
    }

    private void Update()
    {
        if (!canDamage)
        {
            timerTick += Time.deltaTime;
            if (timerTick >= damageTick)
            {
                canDamage = true;
                timerTick = 0f;
            }
        }
    }




    private void OnTriggerStay(Collider other)
    {
        if (canDamage)
        {
            if (other.gameObject.TryGetComponent(out Agent_Type type))
            {
                if (type.Type == typeToDamage)
                {
                    other.gameObject.GetComponent<HealthSystem>().HealthChange(-damage);
                    canDamage = false;
                }
            }
        }
    }
}
