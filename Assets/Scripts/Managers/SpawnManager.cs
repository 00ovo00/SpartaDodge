using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    private GameObject[] spawnPointArray;
    private ObjectPool objectPool;
    [SerializeField] private float spawnTime = 4;
    private string[] poolNameArray;
    private float lastSpawnTime = 0f;
    

    [SerializeField] private GameObject bat;
    [SerializeField] private GameObject crab;
    [SerializeField] private GameObject golem;


    private void Start()
    {

        objectPool = GetComponent<ObjectPool>();
        spawnPointArray = GameObject.FindGameObjectsWithTag("SpawnPoint");
        AddPool("Bat", bat, 10);
        UpdateArray();
   

    }
    
    private void Update()
    {
        Spawn();
        SpawnHandlerByKillCount(DataManager.Instance.GetKillCount());
    }

    public void Spawn()
    {
        lastSpawnTime += Time.deltaTime;

        if (lastSpawnTime < spawnTime) return;

        string currentSelectedPool = SelectRandomPool();

        int randomIndex = Random.Range(0, spawnPointArray.Length);

        objectPool.SpawnFromPool(currentSelectedPool, spawnPointArray[randomIndex]);

        lastSpawnTime = 0f;


    }

    private void SpawnHandlerByKillCount(int killCount)  // 리팩토링 예정
    {

        Debug.Log(killCount);
        switch(killCount)
        {
            case 20:
                if (objectPool.PoolDictionary.ContainsKey("Crab")) return;

                AddPool("Crab", crab, 10);
                UpdateArray();
                break;

            case 50:
                if (objectPool.PoolDictionary.ContainsKey("Golem")) return;

                AddPool("Golem", golem, 10);
                UpdateArray();
                break;

        }

    }

    public string SelectRandomPool()
    {
        if (poolNameArray == null || poolNameArray.Length == 0) return null;

        int randomIndex = Random.Range(0, poolNameArray.Length);
        Debug.Log(randomIndex);
        return poolNameArray[randomIndex];

    }

    private void AddPool(string tag, GameObject prefab, int size)
    {
        objectPool.CreatePool(tag, prefab, size);

    }

    private void UpdateArray()
    {
        poolNameArray = objectPool.poolNameArray;
        
    }

  
  
}
