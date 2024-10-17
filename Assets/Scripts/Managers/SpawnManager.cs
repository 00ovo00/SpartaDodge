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

    private void Start()
    {

        objectPool = GetComponent<ObjectPool>();
        spawnPointArray = GameObject.FindGameObjectsWithTag("SpawnPoint");
        AddPool("Bat", bat, 20);
        poolNameArray = objectPool.poolNameArray;


    }
    private void Awake()
    {
        
    }
    private void Update()
    {
        Spawn();
    }

    public void Spawn()
    {
        lastSpawnTime += Time.deltaTime;
        //Debug.Log(lastSpawnTime);

        if (lastSpawnTime < spawnTime) return;

        string currentSelectedPool = SelectRandomPool();

        int randomIndex = Random.Range(0, spawnPointArray.Length);

        objectPool.SpawnFromPool(currentSelectedPool, spawnPointArray[randomIndex]);

        lastSpawnTime = 0f;


    }

    private void SpawnHandlerByKillCount()
    {


    }

    public string SelectRandomPool()
    {
        if (poolNameArray == null || poolNameArray.Length == 0) return null;

        int randomIndex = Random.Range(0, poolNameArray.Length);
        return poolNameArray[randomIndex];

    }

    private void AddPool(string tag, GameObject prefab, int size)
    {
        objectPool.CreatePool(tag, prefab, size);

    }
  
}
