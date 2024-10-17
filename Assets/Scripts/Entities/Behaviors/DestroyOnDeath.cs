using UnityEngine;

public class DestroyOnDeath : MonoBehaviour
{
    private HealthSystem healthSystem;
    private Rigidbody2D rb2d;

    private ItemDropManager itemDropManager;

    private void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        rb2d = GetComponent<Rigidbody2D>();
        // 실제 실행 주체는 healthSystem임
        healthSystem.OnDeath += OnDeath;
        itemDropManager = FindObjectOfType<ItemDropManager>();
    }

    void OnDeath()
    {
        // 멈추도록 수정
        rb2d.velocity = Vector3.zero;
        gameObject.SetActive(false);

        // 킬카운트 증가
        DataManager.Instance.IncrementKillCount();
        //// 적의 위치에서 아이템 드랍
        itemDropManager.DropItem(transform.position); 
    }
}