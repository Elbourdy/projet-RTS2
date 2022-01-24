using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerProjectileBehavior : MonoBehaviour
{
    public float speed;
    public int damage;
    public GameObject target;
    public Rigidbody rB;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            Destroy(gameObject);

        Vector3 newDirection = target.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(newDirection);

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

        speed += speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == target)
        {
            target.GetComponent<HealthSystem>().HealthChange(-damage);
        }
        Destroy(gameObject);
    }
}
