using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float healthChangeDelay = 0.5f;    // ���� �ð�

    private CharacterStatHandler statsHandler;
    private float timeSinceLastChange = float.MaxValue; // ������ ������ �ް� ����� �ð�
    private bool isAttacked = false;

    // ü���� ������ �� �� �ൿ���� �����ϰ� ���� ����
    public event Action OnDamage;
    public event Action OnHeal;
    public event Action OnDeath;
    public event Action OnInvincibilityEnd;

    public float CurrentHealth { get; private set; }

    // get�� ������ ��ó�� ������Ƽ�� ����ϴ� ��
    // ������ ���ϼ� & ���ռ� ����
    public float MaxHealth => statsHandler.CurrentStat.maxHealth;

    private void Awake()
    {
        statsHandler = GetComponent<CharacterStatHandler>();
    }

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        if (isAttacked && timeSinceLastChange < healthChangeDelay)
        {
            timeSinceLastChange += Time.deltaTime;
            if (timeSinceLastChange >= healthChangeDelay)
            {
                OnInvincibilityEnd?.Invoke();
                isAttacked = false;
            }
        }
    }

    public bool ChangeHealth(float change)
    {
        // ���� �ð����� ü���� ���� ����
        if (timeSinceLastChange < healthChangeDelay)
        {
            return false;
        }

        timeSinceLastChange = 0f;
        CurrentHealth += change;
        // [�ּڰ��� 0, �ִ��� MaxHealth�� �ϴ� ����]
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        // CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth;
        // CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth; �� ���ƿ�!

        // �÷��̾� ü�� �ܼ� ������� Ȯ��
        // TODO : UI���� Ȯ���ϵ��� ����
        if (gameObject.name == "Character")
            Debug.Log(CurrentHealth);

        // �÷��̾� ü���� 0 ���ϸ� ���� ���� ȣ��
        if (gameObject.CompareTag("Player") && CurrentHealth <= 0f)
        {
            GameManager.Instance.GameOver();
            return true;
        }
        // �� ü���� 0 ���ϸ� ��� �̺�Ʈ ȣ�� 
        if (CurrentHealth <= 0f && gameObject.CompareTag("Enemy"))
        {
            CallDeath();
            return true;
        }

        if (change >= 0)
        {
            OnHeal?.Invoke();
        }
        else
        {
            OnDamage?.Invoke();
            isAttacked = true;
        }
        return true;
    }

    private void CallDeath()
    {
        OnDeath?.Invoke();
    }
}