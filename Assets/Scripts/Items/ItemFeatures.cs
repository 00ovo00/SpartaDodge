using UnityEngine;
using static UnityEditor.Progress;

public class ItemFeatures : MonoBehaviour
{
    private ItemSO itemData;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
                break;

            case ItemType.Invincibility:
                statHandler.ActivateInvincibility(item.duration);
                break;

        }
    }
}
