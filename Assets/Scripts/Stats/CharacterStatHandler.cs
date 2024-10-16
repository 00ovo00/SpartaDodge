using UnityEngine;
using System.Collections.Generic;

public class CharacterStatHandler : MonoBehaviour
{
    // 기본 스탯과 버프 스탯들의 능력치를 종합해서 스탯을 계산하는 컴포넌트
    [SerializeField] private CharacterStat baseStats;
    public CharacterStat CurrentStat { get; private set; }
    public List<CharacterStat> statsModifiers = new List<CharacterStat>();

    private void Awake()
    {
        UpdateCharacterStat();
    }

    private void UpdateCharacterStat()
    {
        // statModifier를 반영하기 위해 baseStat을 먼저 받아옴
        AttackSO attackSO = null;
        // attackSO 있으면
        if (baseStats.attackSO != null)
        {
            // 새로운 attackSO 생성, 기존의 것과 다른 새로운 객체
            attackSO = Instantiate(baseStats.attackSO);
        }

        // 기본 능력치 적용 (초기에 기본 능력치 값을 복사하여 적용)
        CurrentStat = new CharacterStat { attackSO = attackSO };    
        // TODO : 지금은 기본 능력치만 적용되고 있지만, 향후 능력치 강화 기능등이 추가될 것임!
        CurrentStat.statsChangeType = baseStats.statsChangeType;
        CurrentStat.maxHealth = baseStats.maxHealth;
        CurrentStat.speed = baseStats.speed;
    }
}