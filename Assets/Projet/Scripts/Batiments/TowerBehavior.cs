using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour
{
    // script qui gère le comportement de la tour
    // pour le s2 modifier le script pou qu'il soit enfant de building et classe de base des différentes tours à implementer

    public enum statesBuilding {Active, Deactivated};
    public statesBuilding towerState= statesBuilding.Deactivated;

    [Header("Variables")]
    [SerializeField] Agent_Type.TypeAgent typeToTarget = Agent_Type.TypeAgent.Enemy;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] private float reloadSpeed;
    [SerializeField] float projectileSpeed;
    [SerializeField] int projectileDamage;
    [SerializeField] float range;
    
    [Header("Feedback")]
    [SerializeField] private Animator animator;
    [SerializeField] private List<MeshRenderer> towerRenderers = new List<MeshRenderer>();
    [SerializeField] private Material activeMaterial, inactiveMaterial;

    private float reloadSpeedCount;

    private HealthSystem hS;
    private LineRenderer lRBattery;

    private float towerMaterialLerpValue = 0f, lerpMaterialTimer = 5f, lerpmaterialCount;

    [Header("RadiusVision")]
    [SerializeField] private GameObject cookieFog;
    [SerializeField] private float minSize, maxSize, timeToStart;

    private statesBuilding bufferForSound = statesBuilding.Deactivated;

    private string soundTowerActivate = "event:/Building/Build_Turret/Build_Turr_Rise/Build_Turr_Rise";
    private string soundTowerDeactivate = "event:/Building/Build_Turret/Build_Turr_Fall/Build_Turr_Fall";
    private string soundProjectileFire = "event:/Building/Build_Turret/Build_Turr_Shot/Build_Turr_Shot";

    // Start is called before the first frame update
    void Start()
    {
        lRBattery = GetComponent<LineRenderer>();

        DisplayRange(range, Color.cyan);
        lRBattery.enabled = false;

        hS = GetComponent<HealthSystem>();

        lerpmaterialCount = lerpMaterialTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (hS.GetBatteryHealth() > 0)
        {
            if (bufferForSound == statesBuilding.Deactivated)
            {
                FMODUnity.RuntimeManager.PlayOneShot(soundTowerActivate, transform.position);
                bufferForSound = statesBuilding.Active;
                lerpmaterialCount = 0;
            }

            towerState = statesBuilding.Active;
        }
        else
        {
            if (bufferForSound == statesBuilding.Active)
            {
                FMODUnity.RuntimeManager.PlayOneShot(soundTowerDeactivate, transform.position);
                bufferForSound = statesBuilding.Deactivated;
                lerpmaterialCount = 0;
            }

            towerState = statesBuilding.Deactivated;
        }

        if (lerpmaterialCount < lerpMaterialTimer)
        {
            lerpmaterialCount += Time.deltaTime;
        }

        if (towerState == statesBuilding.Active)
            towerMaterialLerpValue = lerpmaterialCount / lerpMaterialTimer;
        if (towerState == statesBuilding.Deactivated)
            towerMaterialLerpValue = 1 - (lerpmaterialCount / lerpMaterialTimer);

        SetTowerMaterial(towerMaterialLerpValue);


        if (towerState == statesBuilding.Active)
        {
            reloadSpeedCount += Time.deltaTime;

            if (reloadSpeedCount > reloadSpeed)
            {
                Fire();
            }
            animator.SetBool("ActivateTower", true);
            lRBattery.enabled = true;

            
        }

        if (towerState == statesBuilding.Deactivated)
        {
            animator.SetBool("ActivateTower", false);
            lRBattery.enabled = false;

            SetTowerMaterial(0);
        }

        UpdateVisionRange();
    }

    private void Fire()
    {
        GameObject target = CheckEnnemiesInRange();
        if (target != null)
        {
            GameObject instance = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            TowerProjectileBehavior tPB = instance.GetComponent<TowerProjectileBehavior>();
            tPB.GetComponent<Rigidbody>().AddForce(Vector3.up * projectileSpeed * 2);
            tPB.speed = projectileSpeed;
            tPB.damage = projectileDamage;
            tPB.target = target;

            reloadSpeedCount = 0f;

            FMODUnity.RuntimeManager.PlayOneShot(soundProjectileFire, transform.position);
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

    public void DisplayRange(float range, Color color)
    {
        lRBattery.positionCount = 50;
        lRBattery.useWorldSpace = false;
        lRBattery.SetColors(color, color);

        float x;
        float y = 0f;
        float z;

        float angle = 20f;

        for (int i = 0; i < 50; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * range;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * range;

            lRBattery.SetPosition(i, new Vector3(x, y, z));

            angle += (360f / 49f);
        }
    }

    public void SetTowerMaterial(float lerpValue)
    {
        Material[] matList = new Material[2];
        matList[0] = towerRenderers[0].materials[0];

        int i = 0;

        foreach (MeshRenderer e in towerRenderers)
        {
            if (e.materials.Length == 2)
            {
                if (i != 3 && i != 4 && i != 7)
                    e.materials[1].Lerp(inactiveMaterial, activeMaterial, lerpValue);
                else
                    e.materials[0].Lerp(inactiveMaterial, activeMaterial, lerpValue);
            }
            i++;
        }
    }

    public void UpdateVisionRange()
    {
        if (hS.GetBatteryHealth() > 0)
            towerState = statesBuilding.Active;
        else
            towerState = statesBuilding.Deactivated;

        if (towerState == statesBuilding.Active && cookieFog.transform.localScale.x < maxSize)
        {
            float speed = ((maxSize - minSize) / timeToStart) * Time.deltaTime;
            Vector3 newScale = cookieFog.transform.localScale;
            newScale += Vector3.one * speed;
            newScale.y = 0.2f;
            cookieFog.transform.localScale = newScale;

        }
        else if (towerState == statesBuilding.Deactivated && cookieFog.transform.localScale.x > minSize)
        {
            float speed = ((maxSize - minSize) / timeToStart) * Time.deltaTime;
            Vector3 newScale = cookieFog.transform.localScale;
            newScale -= Vector3.one * speed;
            newScale.y = 0.2f;
            cookieFog.transform.localScale = newScale;
        }
    }
}
