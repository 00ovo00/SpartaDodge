using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "TopDownController/Items/DefaultItem", order = 0)]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite icon;
    public ItemType itemType;
    public float duration; //지속시간
    public float effectIncreaseAmount; // 효과 증가량
}

public enum ItemType
{
    HealthRecovery,
    SpeedBoost,
    Invincibility,
    AttackBoost
}
