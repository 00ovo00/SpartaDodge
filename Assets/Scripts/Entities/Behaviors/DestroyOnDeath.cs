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
        // ���� ���� ��ü�� healthSystem��
        healthSystem.OnDeath += OnDeath;
    }

    void OnDeath()
    {
        // ���ߵ��� ����
        rb2d.velocity = Vector3.zero;

        // �ణ �������� �������� ����
        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            // �÷� �Ӽ� �� ���� ä�θ� �� �����ϰ� �ٽ� �� ����
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        // ��ũ��Ʈ ���̻� �۵� ���ϵ��� ��
        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = false;
        }

        // 2�ʵڿ� �ı�
        Destroy(gameObject, 2f);

        // ���� ��ġ���� ������ ���
        itemDropManager.DropItem(transform.position); 
    }
}