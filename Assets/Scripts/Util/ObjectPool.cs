using System.Collections.Generic;
using UnityEngine;

public partial class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> Pools;
    protected Dictionary<string, Queue<GameObject>> PoolDictionary;

    protected virtual void Awake()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();
        InitializePools();
    }

    protected void InitializePools()
    {
        if (Pools.Count <= 0) return;

        foreach (var pool in Pools)
        {
            CreatePool(pool.tag, pool.prefab, pool.size);
        }
    }

    public virtual void CreatePool(string tag, GameObject prefab, int size)
    {
        Queue<GameObject> objectPool = new Queue<GameObject>();
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }
        PoolDictionary.Add(tag, objectPool);
    }

    public GameObject SpawnFromPool(string tag)
    {
        if (!PoolDictionary.ContainsKey(tag))
            return null;

        GameObject obj = PoolDictionary[tag].Dequeue();
        PoolDictionary[tag].Enqueue(obj);
        obj.SetActive(true);
        return obj;
    }

    public GameObject SpawnFromPool(string tag, GameObject spawnPoint)
    {
        if (!PoolDictionary.ContainsKey(tag))
            return null;

        GameObject obj = PoolDictionary[tag].Dequeue();
        PoolDictionary[tag].Enqueue(obj);
        obj.transform.position = spawnPoint.transform.position; 
        obj.SetActive(true);
        return obj;
    }
}