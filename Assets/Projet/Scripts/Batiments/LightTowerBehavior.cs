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
    }

    // Update is called once per frame
    void Update()
    {
        if (hS.GetBatteryHealth() > 0)
        {
            if (towerState != statesBuilding.Active)
                animator.SetBool("Activated", true);

            towerState = statesBuilding.Active;
        }

        else
        {
            if (towerState != statesBuilding.Deactivated)
                animator.SetBool("Activated", false);

            towerState = statesBuilding.Deactivated;
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
