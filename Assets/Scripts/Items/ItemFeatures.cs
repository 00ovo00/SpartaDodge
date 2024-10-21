using UnityEngine;
using static UnityEditor.Progress;

public class ItemFeatures : MonoBehaviour
{
    private ItemSO itemData;
    private SpriteRenderer spriteRenderer;
    private PlayerInfoUI playerInfoUI;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerInfoUI = FindObjectOfType<PlayerInfoUI>();
    }

    public void SetItem(ItemSO item)
    {
        itemData = item;
        if (spriteRenderer != null && itemData.SpriteImage != null)
        {
            spriteRenderer.sprite = itemData.SpriteImage;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌 대상이 플레이어면 아이템 효과 적용 후 파괴
        if (other.CompareTag("Player"))
        {
            CharacterStatHandler statHandler = other.GetComponent<CharacterStatHandler>();
            HealthSystem healthSystem = other.GetComponent<HealthSystem>();

            ApplyItemEffect(statHandler, healthSystem, itemData);

            Destroy(gameObject); 
        }
    }

    private void ApplyItemEffect(CharacterStatHandler statHandler, HealthSystem healthSystem, ItemSO item)
    {
        switch (item.itemType)
        {
            case ItemType.HealthRecovery:
                healthSystem.Heal(item.effectIncreaseAmount);
                playerInfoUI.UpdateHealth(healthSystem.CurrentHealth, healthSystem.MaxHealth);
                break;

            case ItemType.SpeedBoost:
                statHandler.OverrideSpeed(item.effectIncreaseAmount, item.duration); 
                break;

            case ItemType.Invincibility:
                healthSystem.Invincibility(item.duration);
                break;
        }
    }
}
