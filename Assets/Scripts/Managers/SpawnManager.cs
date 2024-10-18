using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    private GameObject[] spawnPointArray;
    private ObjectPool objectPool;
    [SerializeField] private float spawnTime = 4;
    private List<string> poolNameList;
    private float lastSpawnTime = 0f;
    

    [SerializeField] private GameObject bat;
    [SerializeField] private GameObject crab;
    [SerializeField] private GameObject golem;


    private void Start()
    {

        objectPool = GetComponent<ObjectPool>();
        spawnPointArray = GameObject.FindGameObjectsWithTag("SpawnPoint");
        AddPool("Bat", bat, 10);
        
   

    }
    
    private void Update()
    {
        SpawnTimeChecker();
        SpawnHandlerByKillCount(DataManager.Instance.GetKillCount());
    }

    public void SpawnTimeChecker()
    {
        lastSpawnTime += Time.deltaTime;

        if (lastSpawnTime < spawnTime) return;

        Spawn();


        lastSpawnTime = 0f;

    }

    public void Spawn()
    {
        string currentSelectedPool = SelectRandomPool();

        int randomIndex = Random.Range(0, spawnPointArray.Length);

        objectPool.SpawnFromPool(currentSelectedPool, spawnPointArray[randomIndex]);
        
    }

    private void SpawnHandlerByKillCount(int killCount)  // ¸®ÆÑÅä¸µ ¿¹Á¤
    {

        Debug.Log(killCount);
        switch(killCount)
        {
            case 10:
                if (poolNameList.Contains("Crab")) return;

                AddPool("Crab", crab, 10);              
                break;

            case 20:
                if (poolNameList.Contains("Golem")) return;
                AddPool("Golem", golem, 10);               
                break;

        }

    }

    public string SelectRandomPool()
    {
        if (poolNameList == null || poolNameList.Count == 0) return null;

        int randomIndex = Random.Range(0, poolNameList.Count);
        Debug.Log("·£´ýÇ®" + randomIndex);
        return poolNameList[randomIndex];

    }

    private void AddPool(string tag, GameObject prefab, int size)
    {
        objectPool.CreatePool(tag, prefab, size);
        UpdateArray();
    }

    private void UpdateArray()
    {
        poolNameList = objectPool.GetPoolNameList();
        
    }

  
  
}
