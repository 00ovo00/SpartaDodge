using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "TopDownController/Items/DefaultItem", order = 0)]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite icon;
    public ItemType itemType;
    public float duration; //���ӽð�
    public float effectIncreaseAmount; // ȿ�� ������
}

public enum ItemType
{
    HealthRecovery,
    SpeedBoost,
    Invincibility,
    AttackBoost
}
