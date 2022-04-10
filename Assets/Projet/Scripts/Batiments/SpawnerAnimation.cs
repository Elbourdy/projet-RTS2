using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerAnimation : MonoBehaviour
{
    public Animator animator;
    
    public void StartNight()
    {
        animator.SetBool("Charging", true);
    }

    public void EndNight()
    {
        animator.SetBool("Charging", false);
    }
}
