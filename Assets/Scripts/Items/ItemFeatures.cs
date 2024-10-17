using UnityEngine;
using static UnityEditor.Progress;

public class ItemFeatures : MonoBehaviour
{
    private ItemSO itemData; 

    public void SetItem(ItemSO item)
    {
        itemData = item;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterStatHandler statHandler = other.GetComponent<CharacterStatHandler>();
            ApplyItemEffect(statHandler, itemData);
            Destroy(gameObject); // 
        }
    }

    private void ApplyItemEffect(CharacterStatHandler statHandler, ItemSO item)
    {
        CharacterStat newStatModifier = new CharacterStat
        {
            statsChangeType = StatsChangeType.Add, // (ADD 대신 다른거 설정 가능)
            maxHealth = item.itemType == ItemType.HealthRecovery ? (int)item.effectIncreaseAmount : statHandler.CurrentStat.maxHealth,
            speed = item.itemType == ItemType.SpeedBoost ? statHandler.CurrentStat.speed + item.effectIncreaseAmount : statHandler.CurrentStat.speed
        };

        // 새로운 스탯 변경을 적용
        statHandler.AddStatModifier(newStatModifier);

        // 무적 상태 효과
        if (item.itemType == ItemType.Invincibility)
        {
            statHandler.ActivateInvincibility(item.duration);
        }

        // 다른 아이템 효과 처리
    }
}
