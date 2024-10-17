using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // ������Ʈ Ǯ �����͸� ������ ������ ���� ����
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;    // �̸� ������ �� ������Ʈ ����
    }
    public string[] poolNameArray { get; private set; }
    public List<Pool> Pools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;

    private void Awake()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();
        InitializePools();
    }

    private void InitializePools()
    {
        foreach (var pool in Pools)
        {
            CreatePool(pool.tag, pool.prefab, pool.size);
        }
    }

    public void CreatePool(string tag, GameObject prefab, int size)
    {
        
        Queue<GameObject> objectPool = new Queue<GameObject>();
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }
        PoolDictionary.Add(tag, objectPool);
        UpdatePoolNameArray();
    }

    private void UpdatePoolNameArray()
    {
        poolNameArray = new string[PoolDictionary.Count];
        int index = 0;
        foreach (var key in PoolDictionary.Keys)
        {
            poolNameArray[index++] = key;
        }
    }


    public GameObject SpawnFromPool(string tag)
    {
        // ���ʿ� Pool�� �������� �ʴ� ���
        if (!PoolDictionary.ContainsKey(tag))
            return null;

        // ���� ������ ��ü�� ��Ȱ��
        GameObject obj = PoolDictionary[tag].Dequeue();
        PoolDictionary[tag].Enqueue(obj);
        obj.SetActive(true);
        return obj;
    }

    public GameObject SpawnFromPool(string tag , GameObject spawnPoint)
    {
        // ���ʿ� Pool�� �������� �ʴ� ���
        if (!PoolDictionary.ContainsKey(tag))
            return null;

        // ���� ������ ��ü�� ��Ȱ��
        GameObject obj = PoolDictionary[tag].Dequeue();
        PoolDictionary[tag].Enqueue(obj);
        obj.transform.position = spawnPoint.transform.position; // ������ ������Ʈ ��ġ ����
        obj.SetActive(true);
        return obj;
    }
}