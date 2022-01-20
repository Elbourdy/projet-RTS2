using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightAttack : MonoBehaviour
{
    public static NightAttack instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private int totalNumEnnemyToSpawn = 5;
    [SerializeField] private List<GameObject> spawners = new List<GameObject>();
    [SerializeField] private List<GameObject> ennemiesRemaining = new List<GameObject>();

    private bool isActive = false;

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            CheckIfEnnemiesDead();
        }
    }

    public void SpawnEnnemies()
    {
        if (!isActive)
        {
            isActive = true;

            foreach (GameObject e in spawners)
            {
                for (int i = 0; i < totalNumEnnemyToSpawn; i++)
                {
                    GameObject instance = Instantiate(GameDataStorage.instance.tempEnnemiesAgentClassStorage[0], e.transform.position, Quaternion.identity);
                    instance.GetComponent<AgentStates>().SetState(AgentStates.states.Follow);
                    instance.GetComponent<AgentStates>().MoveAgent(HQBehavior.instance.transform.position);
                    ennemiesRemaining.Add(instance);
                }
            }
        }
    }

    private void CheckIfEnnemiesDead()
    {
        if (ennemiesRemaining.Count > 0)
        {
            int i = 0;
            foreach(GameObject e in ennemiesRemaining)
            {
                if (e == null)
                {
                    ennemiesRemaining.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }
        else
        {
            TickManager.instance.ResetTickFeedback();
            TickManager.instance.dayState = TickManager.statesDay.Day;

            HQBehavior.instance.currentNexusState = HQBehavior.statesNexus.Immobilize;

            isActive = false;
        } 
    }
}
