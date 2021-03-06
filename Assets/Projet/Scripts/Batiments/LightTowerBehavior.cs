using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTowerBehavior : MonoBehaviour
{
    private Animator animator;

    public enum statesBuilding { Active, Deactivated };
    public statesBuilding towerState = statesBuilding.Deactivated;

    private HealthSystem hS;

    public float timeToStart, minSize, maxSize;
    public ParticleSystem cookieFog;

    // Start is called before the first frame update
    void Start()
    {
        hS = GetComponent<HealthSystem>();

        animator = transform.GetChild(0).GetComponent<Animator>();
        cookieFog.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (hS.GetBatteryHealth() > 0)
        {
            if (towerState != statesBuilding.Active)
            {
                animator.SetBool("Activated", true);
                FMODUnity.RuntimeManager.PlayOneShot("event:/Building/Build_VisionTower/Build_VisTwr_Rise/Build_VisTwr_Rise", transform.position);
            }   

            towerState = statesBuilding.Active;
            cookieFog.gameObject.SetActive(true);
        }

        else
        {
            if (towerState != statesBuilding.Deactivated)
            {
                animator.SetBool("Activated", false);
                FMODUnity.RuntimeManager.PlayOneShot("event:/Building/Build_VisionTower/Build_VisTwr_Fall/Build_VisTwr_Fall", transform.position);
            }   

            towerState = statesBuilding.Deactivated;
            cookieFog.gameObject.SetActive(false);
        }


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
