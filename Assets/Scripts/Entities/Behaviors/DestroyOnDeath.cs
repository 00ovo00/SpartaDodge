using UnityEngine;

public class DestroyOnDeath : MonoBehaviour
{
    private HealthSystem healthSystem;
    private ItemDropManager itemDropManager;

    private void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        // 실제 실행 주체는 healthSystem임
        healthSystem.OnDeath += OnDeath;
        itemDropManager = FindObjectOfType<ItemDropManager>();
    }

    void OnDeath()
    {
        // 적 객체 비활성화
        gameObject.SetActive(false);
        healthSystem.CurrentHealth += healthSystem.MaxHealth;
        // 킬카운트 증가
        DataManager.Instance.IncrementKillCount();
        // 적의 위치에서 아이템 드랍
        itemDropManager.DropItem(transform.position); 
    }
}