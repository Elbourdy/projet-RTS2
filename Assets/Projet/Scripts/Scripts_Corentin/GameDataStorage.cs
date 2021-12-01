using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataStorage : MonoBehaviour  //Plus tard automatiser le processus
{
    public List<DataStorage> mainDataStorage = new List<DataStorage>();

   /* [Header ("Worker")]
    public float timerWorker;
    public GameObject prefabWorker;
    public Sprite spriteWorker;
    public int IDWorker;

    [Header("Soldier")]
    public float timerSoldier;
    public GameObject prefabSoldier;
    public Sprite spriteSoldier;
    public int IDSoldier;

    

    private void Awake()
    {
        mainDataStorage.Add(new DataStorage(timerWorker, prefabWorker, spriteWorker, IDWorker));
        mainDataStorage.Add(new DataStorage(timerSoldier, prefabSoldier, spriteSoldier, IDSoldier));
    }*/
}
