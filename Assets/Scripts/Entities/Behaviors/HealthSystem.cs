using System;
using UnityEngine;
using System.Collections;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float healthChangeDelay = 0.5f;    // ���� �ð�
    private Animator animator;
    private CharacterStatHandler statsHandler;
    private float timeSinceLastChange = float.MaxValue; // ������ ������ �ް� ����� �ð�
    private bool isAttacked = false;
    private BoxCollider2D boxcollider;
    private Rigidbody2D rb;

    private PlayerInfoUI playerInfoUI;

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
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();    
        boxcollider = GetComponent<BoxCollider2D>();
        statsHandler = GetComponent<CharacterStatHandler>();
        playerInfoUI = FindObjectOfType<PlayerInfoUI>();
    }

    private void Start()
    {
        CurrentHealth = MaxHealth;
        if (gameObject.CompareTag("Player"))
            playerInfoUI.UpdateHealth(CurrentHealth, MaxHealth);
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

    public void Heal(float healAmount)
    {
        CurrentHealth += healAmount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth); 
        OnHeal?.Invoke(); 

        Debug.Log($"hp�� {healAmount}��ŭ ȸ���Ǿ����ϴ�. ���� hp: {CurrentHealth}");
    }

    public bool ChangeHealth(float change)
    {
        // ���� �ð����� ü���� ���� ����
        if (timeSinceLastChange < healthChangeDelay)
        {
            Debug.Log("�����ð�");
            return false;
        }

        timeSinceLastChange = 0f;
        CurrentHealth += change;
        // [�ּڰ��� 0, �ִ��� MaxHealth�� �ϴ� ����]
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        // CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth;
        // CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth; �� ���ƿ�!

        // �÷��̾� ü�� UI ����
        if (gameObject.CompareTag("Player"))
            playerInfoUI.UpdateHealth(CurrentHealth, MaxHealth);

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
            CurrentHealth = MaxHealth;
            Debug.Log(CurrentHealth);
            return true;
        }

        if (change >= 0)
        {
            OnHeal?.Invoke();
            Debug.Log("����������");
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
        StartCoroutine(DeathSequence());
        DataManager.Instance.IncrementScore(MaxHealth);
    }

    private void OnEnable()
    {
        timeSinceLastChange = float.MaxValue;
        rb.isKinematic = false;
    }

    IEnumerator DeathSequence()
    {
        animator.SetTrigger("IsDie");
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Death"));
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("Death") ? "����" : "�ƴ�");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        OnDeath?.Invoke();
    }

}