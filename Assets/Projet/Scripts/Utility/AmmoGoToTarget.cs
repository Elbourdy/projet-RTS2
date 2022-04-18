using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AmmoGoToTarget : MonoBehaviour
{
    private GameObject targetToHit;
    private float damage = 1f;
    private Vector3 dir;

    public float speed = 3f;

    private void Update()
    {
        GoToTarget();
    }


    private void GoToTarget()
    {
        dir = targetToHit.transform.position - transform.position;
        transform.Translate(dir * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == targetToHit)
        {
            targetToHit.GetComponent<HealthSystem>().HealthChange(-damage);
            Destroy(gameObject);
        }
    }

    public void SetupAmmo(GameObject _target, float _damage)
    {
        targetToHit = _target;
        damage = _damage;
    }
}
