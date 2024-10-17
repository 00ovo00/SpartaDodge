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
    [SerializeField] private float spawnTime;
    private string[] poolNameArray;
    private System.Random random;
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

}
