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
        itemDropManager = FindObjectOfType<ItemDropManager>();
    }

    void OnDeath()
    {
        // ���ߵ��� ����
        rb2d.velocity = Vector3.zero;
        gameObject.SetActive(false);

        // ųī��Ʈ ����
        DataManager.Instance.IncrementKillCount();
        //// ���� ��ġ���� ������ ���
        itemDropManager.DropItem(transform.position); 
    }
}