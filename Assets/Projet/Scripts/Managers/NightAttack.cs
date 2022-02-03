using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightAttack : MonoBehaviour
{
    //script qui gère les attaques de nuit

    public static NightAttack instance;

    private void Awake()
    {
        instance = this;
    }

    private struct Spawner //structure pour calculer pour calculer plus facilement les spawners actifs ou non en fonction de leur proximité au nexus
    {
        public GameObject spawnerGameObject;
        public float distanceFromNexus;
    }

    [Header("Attaque de nuit")]
    [SerializeField] private int totalNumEnnemyToSpawn = 5;
    [SerializeField] private int numSpawnerActivatedByNight = 3;
    [SerializeField] private Material matSpawnerActive, matSpawnerInactive;

    private List<Spawner> spawnerList = new List<Spawner>();
    private List<GameObject> ennemiesRemaining = new List<GameObject>();
    private bool isActive = false;
    private GameObject nexus;

    private string soundNexusOnMouvement = "event:/Building/Build_Nexus/Build_Nex_OnMouvment/Build_Nex_OnMouvment";
    private string soundEnnemiesSpawnAtNight = "event:/Unit/Unit_Enemy/UnitE_Global/UnitE_Glob_Spawn";

    private void Start()
    {
        nexus = GameObject.Find("Nexus");
        InitializeSpawnerList();
        FeedbackSpawnerReset();
    }

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

            for (int j = 0; j < numSpawnerActivatedByNight; j++)
            {
                for (int i = 0; i < totalNumEnnemyToSpawn; i++)
                {
                    GameObject instance = Instantiate(GameDataStorage.instance.tempEnnemiesAgentClassStorage[0], spawnerList[j].spawnerGameObject.transform.position, Quaternion.identity);
                    instance.GetComponent<AgentStates>().SetTarget(HQBehavior.instance.gameObject);

                    instance.GetComponent<AgentStates>().isSuperAggressive = true;
                    instance.GetComponent<AgentStates>().onFollowEnter?.Invoke();
                    instance.GetComponent<AgentStates>().SetState(AgentStates.states.Aggressive);
                    
                    ennemiesRemaining.Add(instance);
                }
            }

            FMODUnity.RuntimeManager.PlayOneShot(soundEnnemiesSpawnAtNight);
        }
    }

    private void CheckIfEnnemiesDead()
    {
        if (ennemiesRemaining.Count > 0)
        {
            int i = 0;
            foreach (GameObject e in ennemiesRemaining)
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
            TickManager.instance.ResetTickCounter();
            TickManager.instance.dayState = TickManager.statesDay.Dawn;

            HQBehavior.instance.currentNexusState = HQBehavior.statesNexus.Move;

            FeedbackSpawnerReset();

            FMODUnity.RuntimeManager.PlayOneShot(soundNexusOnMouvement, HQBehavior.instance.transform.position);
            isActive = false;
        }
    }

    private void InitializeSpawnerList()
    {
        GameObject[] spawnerSearch = GameObject.FindGameObjectsWithTag("SpawnerNight");

        for (int i = 0; i < spawnerSearch.Length; i++)
        {
            Spawner tmp = new Spawner();
            tmp.spawnerGameObject = spawnerSearch[i];
            tmp.distanceFromNexus = 0;
            spawnerList.Add(tmp);
        }
    }

    private void SetDistanceSpawnerNexus()
    {
        for (int i = 0; i < spawnerList.Count; i++)
        {
            Spawner tmp = new Spawner();
            tmp.spawnerGameObject = spawnerList[i].spawnerGameObject;
            tmp.distanceFromNexus = Vector3.Distance(spawnerList[i].spawnerGameObject.transform.position, nexus.transform.position);
            spawnerList[i] = tmp;
        }
    }

    private void SortListSpawner()
    {
        bool sort = true;

        for (int i = 0; i < spawnerList.Count - 1; i++)
        {
            if (spawnerList[i].distanceFromNexus > spawnerList[i + 1].distanceFromNexus)
            {
                Spawner tmp = new Spawner();
                tmp = spawnerList[i];
                spawnerList[i] = spawnerList[i + 1];
                spawnerList[i + 1] = tmp;
                sort = false;
            }
        }
        if (!sort)
            SortListSpawner();
    }

    public void PreparationStartAttack()
    {
        SetDistanceSpawnerNexus();
        SortListSpawner();
        FeedbackSpawnerActive();
    }

    private void FeedbackSpawnerActive()
    {
        for (int i = 0; i < numSpawnerActivatedByNight; i++)
        {
            spawnerList[i].spawnerGameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = matSpawnerActive;
        }
    }

    private void FeedbackSpawnerReset()
    {
        for (int i = 0; i < spawnerList.Count; i++)
        {
            spawnerList[i].spawnerGameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = matSpawnerInactive;
        }
    }

    public void CancelNightAttack() // pour le debug uniquement
    {
        for (int i = 0; i < ennemiesRemaining.Count; i++)
        {
            Destroy(ennemiesRemaining[i]);
        }

        ennemiesRemaining.Clear();
    }
}
