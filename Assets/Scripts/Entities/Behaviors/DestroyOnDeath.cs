using UnityEngine;

public class DestroyOnDeath : MonoBehaviour
{
    private HealthSystem healthSystem;
    private ItemDropManager itemDropManager;

    private void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        // ���� ���� ��ü�� healthSystem��
        healthSystem.OnDeath += OnDeath;
        itemDropManager = FindObjectOfType<ItemDropManager>();
    }

    void OnDeath()
    {
        // ���ߵ��� ����       
        gameObject.SetActive(false);

        // ųī��Ʈ ����
        DataManager.Instance.IncrementKillCount();
        //// ���� ��ġ���� ������ ���
        itemDropManager.DropItem(transform.position); 
    }
}