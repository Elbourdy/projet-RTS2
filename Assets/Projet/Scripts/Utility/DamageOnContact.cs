using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class DamageOnContact : MonoBehaviour
{
    public Agent_Type.TypeAgent typeToDamage;
    public float damage = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Agent_Type type))
        {
            if (type.Type == typeToDamage)
            {
                other.gameObject.GetComponent<HealthSystem>().HealthChange(-damage);
            }
        }
    }
}
