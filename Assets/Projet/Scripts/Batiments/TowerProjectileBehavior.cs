using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerProjectileBehavior : MonoBehaviour
{
    //comportement du projectile de la tour apr?s instanciation

    public float speed;
    public int damage;
    public GameObject target;

    FMOD.Studio.EventInstance soundProjectile;


    private void Start()
    {
        soundProjectile = FMODUnity.RuntimeManager.CreateInstance("event:/Building/Build_Turret/Build_Turr_Impact/Build_Turr_Impact");
        soundProjectile.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
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
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        soundProjectile.start();
    }
}
