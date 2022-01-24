using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour
{
    public enum statesBuilding {Active, Deactivated};
    public statesBuilding towerState= statesBuilding.Active;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] private float reloadSpeed;
    [SerializeField] float projectileSpeed;
    [SerializeField] int projectileDamage;
    [SerializeField] float range;
    [SerializeField] Agent_Type.TypeAgent typeToTarget = Agent_Type.TypeAgent.Enemy;

    private float reloadSpeedCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(towerState == statesBuilding.Active)
        {
            reloadSpeedCount += Time.deltaTime;

            if (reloadSpeedCount > reloadSpeed)
            {
                Fire();
            }
        }
    }

    private void Fire()
    {
        GameObject target = CheckEnnemiesInRange();
        if (target != null)
        {
            Debug.Log(target.name);
            GameObject instance = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            //instance.GetComponent<Rigidbody>().velocity = new Vector3(0, 1, 0) * projectileSpeed;

            TowerProjectileBehavior tPB = instance.GetComponent<TowerProjectileBehavior>();
            tPB.speed = projectileSpeed;
            tPB.damage = projectileDamage;
            tPB.target = target;

            reloadSpeedCount = 0f;
        }
    }

    private GameObject CheckEnnemiesInRange()
    {
        List<GameObject> possibleTarget = new List<GameObject>();

        Collider[] hits = Physics.OverlapSphere(transform.position, range);
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].GetComponent<Agent_Type>() != null && hits[i].GetComponent<Agent_Type>().Type == typeToTarget)
                {
                    possibleTarget.Add(hits[i].gameObject);
                }
            }

            if (possibleTarget.Count > 0)
            {
                int rand = Mathf.RoundToInt(Random.Range(0, possibleTarget.Count - 1));

                return possibleTarget[rand];
            }
        }

        return null;
    }

    private void ResetTower()
    {
        reloadSpeedCount = 0f;
    }
}
