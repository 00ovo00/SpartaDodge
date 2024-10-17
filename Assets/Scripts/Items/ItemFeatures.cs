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
            statsChangeType = StatsChangeType.Add, // (ADD ��� �ٸ��� ���� ����)
            maxHealth = item.itemType == ItemType.HealthRecovery ? (int)item.effectIncreaseAmount : statHandler.CurrentStat.maxHealth,
            speed = item.itemType == ItemType.SpeedBoost ? statHandler.CurrentStat.speed + item.effectIncreaseAmount : statHandler.CurrentStat.speed
        };

        // ���ο� ���� ������ ����
        statHandler.AddStatModifier(newStatModifier);

        // ���� ���� ȿ��
        if (item.itemType == ItemType.Invincibility)
        {
            statHandler.ActivateInvincibility(item.duration);
        }

        // �ٸ� ������ ȿ�� ó��
    }
}
