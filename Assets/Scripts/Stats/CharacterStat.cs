using System;
using UnityEngine;

// Add 먼저하고, Multiple하고, 마지막에 Override하는 개념
// enum에는 각각 정수가 매핑되어있기 때문에 가능 (0, 1, 2,...) 
// => 차후에 정렬 활용하면 오름차순으로 정렬활용해서 체계적으로 버프효과 적용순서 관리 가능
// Q: Override가 마지막에 있으면 무슨 효과가 있을까요?
// A: 공격력을 고정해야하는 특정 로직이나 기본 공격력 적용에 활용 가능
public enum StatsChangeType
{
    Add, // 0, 스탯을 n만큼 더함
    Multiple, // 1 , 스탯을 n% 증가
    Override, // 2 , 스탯을 n으로 변경
}

// 클래스가 Serializable한 멤버로만 구성되어 있으면 [Serializable]을 붙여 에디터에서 확인 가능!
// [Serializable]
[System.Serializable]   // 데이터 폴더처럼 사용할 수 있게 만들어주는 Attribute
public class CharacterStat
{
    public StatsChangeType statsChangeType;
    // [Range(,)] 슬라이더
    [Range(1, 100)] public int maxHealth;
    [Range(1f, 20f)] public float speed;
    public AttackSO attackSO;
}