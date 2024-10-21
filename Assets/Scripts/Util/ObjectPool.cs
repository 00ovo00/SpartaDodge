using System.Collections.Generic;
using UnityEngine;

public partial class ObjectPool : MonoBehaviour
{
    // 오브젝트 풀을 정의하기 위한 직렬화 가능 클래스
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
        PoolDictionary = new Dictionary<string, Queue<GameObject>>(); // 오브젝트 풀을 위한 딕셔너리 초기화
        InitializePools(); // 정의된 목록에 따라 오브젝트 풀 생성
    }

    protected void InitializePools() // 오브젝트 풀을 초기화하는 메서드
    {
        if (Pools.Count <= 0) return; // 정의된 풀이 없으면 메서드 종료

        foreach (var pool in Pools)
        {
            CreatePool(pool.tag, pool.prefab, pool.size);
        }
    }

    public virtual void CreatePool(string tag, GameObject prefab, int size) // 특정 오브젝트 풀을 생성하는 메서드
    {
        Queue<GameObject> objectPool = new Queue<GameObject>(); // 풀을 위한 새 큐 생성
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab, transform); // 프리팹을 인스턴스화하고 비활성화한 후 풀 큐에 추가
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }
        PoolDictionary.Add(tag, objectPool);
    }

    public GameObject SpawnFromPool(string tag) // 태그를 사용해 풀에서 오브젝트를 생성하는 메서드
    {
        if (!PoolDictionary.ContainsKey(tag)) // 풀이 존재하는지 확인
            return null;

        GameObject obj = PoolDictionary[tag].Dequeue(); // 풀에서 객체를 제거하고 재삽입 후 활성화
        PoolDictionary[tag].Enqueue(obj);
        obj.SetActive(true);
        return obj;
    }

    public GameObject SpawnFromPool(string tag, GameObject spawnPoint) // 특정 스폰 포인트에서 오브젝트를 생성하는 오버로딩 메서드
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