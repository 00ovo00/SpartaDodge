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
    private MonsterObjectPool monsterObjectPool;    
    [SerializeField] private float spawnTime = 4;
    private List<string> poolNameList;
    private float lastSpawnTime = 0f;
    



    [System.Serializable]
    public class SpawnInfo
    {
        public int killCount;  
        public string tag;     // ���� �±�
        public GameObject prefab;  // ���� ������
        public int size;       // ������ ������ ��
    }
      
    public List<SpawnInfo> spawnInpos;

    private void Start()
    {

        objectPool = GetComponent<ObjectPool>();
        monsterObjectPool = GetComponent<MonsterObjectPool>();
        spawnPointArray = GameObject.FindGameObjectsWithTag("SpawnPoint");       

        DataManager.Instance.OnKillCountChanged += SpawnHandlerByKillCount;
        UpdateArray();



    }
    
    private void Update()
    {
        SpawnTimeChecker();
        
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
        Debug.Log("���õ� Ǯ" + currentSelectedPool);
        int randomIndex = Random.Range(0, spawnPointArray.Length);

        monsterObjectPool.SpawnFromPool(currentSelectedPool, spawnPointArray[randomIndex]);
        
    }

    private void SpawnHandlerByKillCount(int killCount)
    {
        foreach (var info in spawnInpos)
        {
            if (killCount == info.killCount && !poolNameList.Contains(info.tag))
            {
                AddPool(info.tag, info.prefab, info.size);
                break; 
            }
        }
    }

    public string SelectRandomPool()
    {
        if (poolNameList == null || poolNameList.Count == 0) return null;

        int randomIndex = Random.Range(0, poolNameList.Count);
        Debug.Log("����Ǯ" + randomIndex);
        return poolNameList[randomIndex];

    }

    private void AddPool(string tag, GameObject prefab, int size)
    {    
            monsterObjectPool.CreatePool(tag, prefab, size);
            UpdateArray();    
    }

    private void UpdateArray()
    {
        poolNameList = monsterObjectPool.GetPoolNameList();
        
    }

  
  
}
