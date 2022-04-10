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

    //[Header("Attaque de nuit")]
    //[SerializeField] private Material matSpawnerActive, matSpawnerInactive;

    public NightAttackScriptable nAS;

    /*public List<int> numSpawnerActivatedByNight = new List<int>();
    public List<int> nightBeforeSpawn = new List<int>();
    public List<int> costByNight = new List<int>();*/

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
        //FeedbackSpawnerReset();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            CheckIfEnnemiesDead();
        }
    }

    public void SpawnEnnemies(int night)
    {
        if (!isActive)
        {
            isActive = true;

            List<GameObject> ennemiesAvailable = new List<GameObject>();

            int x = 0;

            foreach(GameObject e in GameDataStorage.instance.tempEnnemiesAgentClassStorage)
            {
                if (nAS.nightBeforeUnitSpawn[x] <= night)
                    ennemiesAvailable.Add(e);
                x++;
            }

            for (int j = 0; j < (night >= nAS.numSpawnerActive.Length ? nAS.numSpawnerActive[nAS.numSpawnerActive.Length - 1] : nAS.numSpawnerActive[night]); j++)
            {
                List<GameObject> ennemiesToSpawn = new List<GameObject>();

                Debug.Log(night >= nAS.height ? nAS.height - 1 : night);
                if (nAS.customWaves[(night >= nAS.height ? nAS.height-1 : night) * nAS.width] == 1)
                {
                    ennemiesAvailable = GameDataStorage.instance.tempEnnemiesAgentClassStorage;
                    Debug.Log(ennemiesAvailable.Count);
                    for(int i = 0; i < ennemiesAvailable.Count; i++)
                    {
                        for (int y = 0; y < nAS.customWaves[i + nAS.width * (night >= nAS.height ? nAS.height - 1 : night) + 1]; y++)
                        {
                            Debug.Log("Creating ennemies custom");
                            ennemiesToSpawn.Add(ennemiesAvailable[i]);
                        }
                    }
                }
                else
                {
                    Debug.Log("Creating ennemies random");
                    ennemiesToSpawn = CreateListEnnemiesToSpawn(night, ennemiesAvailable);
                }
                
                for (int i = 0; i < ennemiesToSpawn.Count; i++)
                {
                    GameObject instance = Instantiate(ennemiesToSpawn[i], spawnerList[j].spawnerGameObject.transform.position, Quaternion.identity);
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

    private List<GameObject> CreateListEnnemiesToSpawn(int night, List<GameObject> ennemies)
    {
        List<GameObject> list = new List<GameObject>();

        int cost = night >= nAS.costByNight.Length ? nAS.costByNight[nAS.costByNight.Length - 1] : nAS.costByNight[night];
        int actualCost = 0;

        while (actualCost < cost)
        {
            int rand = Random.Range(0, ennemies.Count);
            list.Add(ennemies[rand]);
            actualCost += ennemies[rand].GetComponent<ClassAgentContainer>().myClass.spawnerCost;
        }

        return list;
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
        int night = nAS.numSpawnerActive[nAS.numSpawnerActive.Length < TickManager.instance.numberOfDaysPassed ? nAS.numSpawnerActive[nAS.numSpawnerActive.Length - 1]
            : nAS.numSpawnerActive[TickManager.instance.numberOfDaysPassed]];

        for (int i = 0; i < night; i++)
        {
            spawnerList[i].spawnerGameObject.GetComponent<SpawnerAnimation>().StartNight();
        }
    }

    private void FeedbackSpawnerReset()
    {
        for (int i = 0; i < spawnerList.Count; i++)
        {
            spawnerList[i].spawnerGameObject.GetComponent<SpawnerAnimation>().EndNight();
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
