using UnityEngine;
using static UnityEditor.Progress;

public class ItemDropManager : MonoBehaviour
{
    public GameObject itemPrefab; // ����� ������ ������
    public ItemSO[] possibleItems; // ��� ������ ������ ���
    public float dropChance = 1f; // ������ ��� Ȯ��

    public void DropItem(Vector3 dropPosition)
    {
        if (Random.value <= dropChance) // ��� ���� ���⼭ ����
        {
            // ���� ������ ����
            int randomIndex = Random.Range(0, possibleItems.Length);
            ItemSO droppedItem = possibleItems[randomIndex];

            // ������ ����
            GameObject itemObject = Instantiate(itemPrefab, dropPosition, Quaternion.identity);
            ItemFeatures itemFeatures = itemObject.GetComponent<ItemFeatures>();
            // ������ ������ ����
            itemFeatures.SetItem(droppedItem); 
        }
    }
}
