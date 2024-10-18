using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // 오브젝트 풀 데이터를 정의할 데이터 모음 정의
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;    // 미리 생성해 둘 오브젝트 개수
    }
    public List<string> PollNameList { get; private set; }
    public List<Pool> Pools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;

    private void Awake()
    {
        PollNameList = new List<string>();
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
        UpdatePoolNameList(tag);
    }

    private void UpdatePoolNameList(string tag)
    {
        PollNameList.Add(tag);
    }

    public List<string> GetPoolNameList()
    {
        return PollNameList;
    }


    public GameObject SpawnFromPool(string tag)
    {
        // 애초에 Pool이 존재하지 않는 경우
        if (!PoolDictionary.ContainsKey(tag))
            return null;

        // 제일 오래된 객체를 재활용
        GameObject obj = PoolDictionary[tag].Dequeue();
        PoolDictionary[tag].Enqueue(obj);
        obj.SetActive(true);
        return obj;
    }

    public GameObject SpawnFromPool(string tag , GameObject spawnPoint)
    {
        // 애초에 Pool이 존재하지 않는 경우
        if (!PoolDictionary.ContainsKey(tag))
            return null;

        // 제일 오래된 객체를 재활용
        GameObject obj = PoolDictionary[tag].Dequeue();
        PoolDictionary[tag].Enqueue(obj);
        obj.transform.position = spawnPoint.transform.position; // 생성된 오브젝트 위치 조정
        obj.SetActive(true);
        return obj;
    }
}