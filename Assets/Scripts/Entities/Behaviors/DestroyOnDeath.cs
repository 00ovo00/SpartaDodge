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
    }

    void OnDeath()
    {
        // 멈추도록 수정
        rb2d.velocity = Vector3.zero;

        // 약간 반투명한 느낌으로 변경
        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            // 컬러 속성 중 알파 채널만 값 변경하고 다시 값 설정
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        // 스크립트 더이상 작동 안하도록 함
        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = false;
        }

        // 2초뒤에 파괴
        Destroy(gameObject, 2f);

        // 적의 위치에서 아이템 드랍
        itemDropManager.DropItem(transform.position); 
    }
}