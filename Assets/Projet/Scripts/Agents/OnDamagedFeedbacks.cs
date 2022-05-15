using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDamagedFeedbacks : MonoBehaviour
{
    [SerializeField] private GameObject visualEffect;

    [SerializeField] private Vector3 yOffset = new Vector3(0,0.5f,0);

    private void Awake()
    {
        var system = gameObject.GetComponent<HealthSystem>();
        system.onDamaged += SpawnVFX;
    }


    private void SpawnVFX()
    {
        GameObject.Instantiate(visualEffect, transform.position + yOffset, Quaternion.identity);
    }
}
