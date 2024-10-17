using UnityEngine;
using static UnityEditor.Progress;

public class ItemDropManager : MonoBehaviour
{
    public GameObject itemPrefab; // 드랍할 아이템 프리팹
    public ItemSO[] possibleItems; // 드랍 가능한 아이템 목록
    public float dropChance = 1f; // 아이템 드랍 확률

    public void DropItem(Vector3 dropPosition)
    {
        if (Random.value <= dropChance) // 드랍 조건 여기서 변경
        {
            // 랜덤 아이템 선택
            int randomIndex = Random.Range(0, possibleItems.Length);
            ItemSO droppedItem = possibleItems[randomIndex];

            // 아이템 생성
            GameObject itemObject = Instantiate(itemPrefab, dropPosition, Quaternion.identity);
            ItemFeatures itemFeatures = itemObject.GetComponent<ItemFeatures>();
            // 아이템 데이터 설정
            itemFeatures.SetItem(droppedItem); 
        }
    }
}
