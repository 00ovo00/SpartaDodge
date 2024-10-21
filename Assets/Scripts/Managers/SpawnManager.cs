using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    private GameObject[] spawnPointArray; // 스폰 포인트 배열
    public ObjectPool objectPool;
    private MonsterObjectPool monsterObjectPool;
    [SerializeField] private float spawnTime = 4;
    private List<string> poolNameList; // 풀 이름 리스트
    private float lastSpawnTime = 0f;

    public static SpawnManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        Instance = this;
    }
    [System.Serializable]
    public class SpawnInfo    // 몬스터 스폰 정보 저장 클래스

    {
        public int killCount;   // 스폰을 위한 킬 카운트 조건
        public string tag;     // 몬스터 태그
        public GameObject prefab;  // 몬스터 프리팹
        public int size;       // 생성할 몬스터의 수
    }

    public List<SpawnInfo> spawnInpos; // 스폰 정보 리스트

    private void Start()
    {

        objectPool = GetComponent<ObjectPool>();
        monsterObjectPool = GetComponent<MonsterObjectPool>();
        spawnPointArray = GameObject.FindGameObjectsWithTag("SpawnPoint");

        // 처치 수 변화에 따른 스폰 핸들러 등록
        DataManager.Instance.OnKillCountChanged += SpawnHandlerByKillCount;
        UpdateArray(); // 풀 이름 리스트 업데이트
    }

    private void Update()
    {
        SpawnTimeChecker(); // 스폰 시간 체크
    }

    public void SpawnTimeChecker()
    {
        lastSpawnTime += Time.deltaTime;

        if (lastSpawnTime < spawnTime) return; // 스폰 주기 미충족 시 종료

        Spawn();    // 몬스터 스폰

        lastSpawnTime = 0f; // 마지막 스폰 시간 초기화

    }

    public void Spawn()
    {
        string currentSelectedPool = SelectRandomPool(); // 랜덤 풀 선택
        Debug.Log("선택된 풀" + currentSelectedPool);
        int randomIndex = Random.Range(0, spawnPointArray.Length); // 랜덤 스폰 포인트 선택

        monsterObjectPool.SpawnFromPool(currentSelectedPool, spawnPointArray[randomIndex]); // 몬스터 스폰 , 랜덤풀, 랜덤스폰 활용

    }

    private void SpawnHandlerByKillCount(int killCount) // 처치 수에 따라 스폰 핸들러
    {
        foreach (var info in spawnInpos)
        {
            if (killCount == info.killCount && !poolNameList.Contains(info.tag)) // 처치 수와 일치하는 스폰 정보가 있으면 풀 추가

            {
                AddPool(info.tag, info.prefab, info.size);
                break;
            }
        }
    }

    public string SelectRandomPool()   // 랜덤 풀 선택 메서드

    {
        if (poolNameList == null || poolNameList.Count == 0) return null;  // 풀 이름 리스트가 없으면 종료

        int randomIndex = Random.Range(0, poolNameList.Count);  // 랜덤 인덱스 선택
        Debug.Log("랜덤풀" + randomIndex);
        return poolNameList[randomIndex];  // 선택된 풀 반환

    }

    private void AddPool(string tag, GameObject prefab, int size)   // 새로운 풀 추가 메서드
    {
        monsterObjectPool.CreatePool(tag, prefab, size);  // 몬스터 풀 생성
        UpdateArray(); // 풀 이름 리스트 업데이트
    }

    private void UpdateArray()
    {
        poolNameList = monsterObjectPool.GetPoolNameList(); // 현재 이름 리스트 가져오기 from MonsterObjectPool

    }
}
