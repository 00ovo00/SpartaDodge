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
    [SerializeField] private float spawnTime = 2;
    private string[] poolNameArray;
    private float lastSpawnTime = 0f;

   
    [SerializeField] private GameObject bat;

    private void Awake()
    {
     
        objectPool = GetComponent<ObjectPool>();
        spawnPointArray = GameObject.FindGameObjectsWithTag("SpawnPoint");
        objectPool.CreatePool("Bat", bat, 20);
        poolNameArray = new string[objectPool.Pools.Count];
        for (int i = 0; i < objectPool.Pools.Count; i++)
        {
            poolNameArray[i] = objectPool.Pools[i].tag;
            Debug.Log(poolNameArray[i]);

        }
    
    }

    private void Update()
    {
        
    }

    public void Spawn()
    {
        lastSpawnTime += Time.deltaTime;

        if (lastSpawnTime < spawnTime) return;
        
        int randomIndex = Random.Range(0, spawnPointArray.Length);

        lastSpawnTime = 0f;
       
       
    }

    private void SpawnHandlerByKillCount()
    {


    }

    public string RandomSpawnPrefabs()
    {
        int randomIndex = Random.Range(0, poolNameArray.Length);
        return poolNameArray[randomIndex];

    }

}
