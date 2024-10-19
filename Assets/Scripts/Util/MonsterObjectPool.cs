
using System.Collections.Generic;
using UnityEngine;

public class MonsterObjectPool : ObjectPool 
{

    public List<string> PollNameList { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }
    public override void CreatePool(string tag, GameObject prefab, int size)
    {
        base.CreatePool(tag, prefab, size);
        UpdatePoolNameList(tag);
    }

    private void UpdatePoolNameList(string tag)
    {
        if (PollNameList == null)
        {
            PollNameList = new List<string>();
        }
        PollNameList.Add(tag);
        Debug.Log("네임리스트" + tag);
    }

    public List<string> GetPoolNameList()
    {
        return PollNameList;
    }

}
