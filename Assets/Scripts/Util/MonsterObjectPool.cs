
using System.Collections.Generic;
using UnityEngine;

public class MonsterObjectPool : ObjectPool 
{

    public List<string> PollNameList { get; private set; } // 풀 이름 리스트 SpawnManager 에서 활용

    protected override void Awake()
    {
        base.Awake();
    }
    public override void CreatePool(string tag, GameObject prefab, int size) // 오버라이드된 풀 생성 메서드
    {
        base.CreatePool(tag, prefab, size);
        UpdatePoolNameList(tag);
    }

    private void UpdatePoolNameList(string tag) // 풀 이름 리스트를 업데이트하는 메서드
    {
        if (PollNameList == null)
        {
            PollNameList = new List<string>();
        }
        PollNameList.Add(tag);
        Debug.Log("네임리스트" + tag);
    }

    public List<string> GetPoolNameList() // 풀 이름 리스트를 반환하는 메서드
    {
        return PollNameList;
    }

}
