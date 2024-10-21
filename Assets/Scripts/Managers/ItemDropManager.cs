using UnityEngine;
using static UnityEditor.Progress;

public class ItemDropManager : MonoBehaviour
{
    public GameObject itemPrefab; // 드랍할 아이템 프리팹
    public ItemSO[] droppableItems; // 드랍 가능한 아이템 목록
    public float dropChance = 1f; // 아이템 드랍 확률

    public void DropItem(Vector3 dropPosition)
    {
        if (Random.value <= dropChance) // 드랍 조건 여기서 변경
        {
            Debug.Log("아이템이 떨어졌습니다.");
            // 랜덤 아이템 선택
            int randomIndex = Random.Range(0, droppableItems.Length);
            ItemSO droppedItem = droppableItems[randomIndex];

            // 아이템 생성
            GameObject itemObject = Instantiate(itemPrefab, dropPosition, Quaternion.identity);
            // 아이템 설정
            ItemFeatures itemFeatures = itemObject.GetComponent<ItemFeatures>();
            itemFeatures.SetItem(droppedItem); 
        }
    }
}
