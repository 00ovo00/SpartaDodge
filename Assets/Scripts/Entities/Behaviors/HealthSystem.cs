using System;
using UnityEngine;
using System.Collections;
using System.Reflection;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float healthChangeDelay = 0.5f;    // 무적 시간
    private Animator animator;
    private TopDownAnimationController animationController;
    private CharacterStatHandler statsHandler;
    private float timeSinceLastChange = float.MaxValue; // 마지막 공격을 받고 경과한 시간
    private bool isAttacked = false;
    private bool isInvincible = false;

    private BoxCollider2D boxcollider;
    private Rigidbody2D rb;

    public event Action<float, float> OnHealthChanged;

    // 체력이 변했을 때 할 행동들을 정의하고 적용 가능
    public event Action OnDamage;
    public event Action OnHeal;
    public event Action OnDeath;
    public event Action<float> OnInvincibilityStart;
    public event Action OnInvincibilityEnd;
    public event Action OnGameOver;

    public float CurrentHealth { get; set; }

    // get만 구현된 것처럼 프로퍼티를 사용하는 것
    // 데이터 유일성 & 정합성 유지
    public float MaxHealth => statsHandler.CurrentStat.maxHealth;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();    
        boxcollider = GetComponent<BoxCollider2D>();
        statsHandler = GetComponent<CharacterStatHandler>();
        animationController = GetComponent<TopDownAnimationController>();

        OnGameOver += GameManager.Instance.GameOver;
    }

    private void Start()
    {
        CurrentHealth = MaxHealth;
        if (gameObject.CompareTag("Player"))
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    private void Update()
    {
        // 공격받은 상태이고 무적 시간이면
        if (isAttacked && timeSinceLastChange < healthChangeDelay)
        {
            timeSinceLastChange += Time.deltaTime;
            // 무적 시간이 지나면
            if (timeSinceLastChange >= healthChangeDelay)
            {
                OnInvincibilityEnd?.Invoke();   // 무적 상태 종료 이벤트 호출
                isAttacked = false; // 피격 가능한 상태로 변경
            }
        }
    }

    public void Heal(float healAmount)
    {
        CurrentHealth += healAmount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth); 
        OnHeal?.Invoke(); 

        Debug.Log($"hp가 {healAmount}만큼 회복되었습니다. 현재 hp: {CurrentHealth}");
    }

    public bool ChangeHealth(float change)
    {
        // 무적 상태거나 무적 시간이 끝나지 않았다면 체력 변동 무효화
        if (isInvincible || timeSinceLastChange < healthChangeDelay)
            return false;

        timeSinceLastChange = 0f;
        CurrentHealth += change;
        // [최솟값을 0, 최댓값을 MaxHealth로 하는 구문]
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        // CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth;
        // CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth; 와 같아요!

        // 플레이어 체력 UI 갱신
        if (gameObject.CompareTag("Player"))
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

        // 플레이어 체력이 0 이하면 게임 오버 호출
        if (gameObject.CompareTag("Player") && CurrentHealth <= 0f)
        {
            Debug.Log("플레이어 체력0");
            OnGameOver?.Invoke(); 
            return true;
        }
        // 적 체력이 0 이하면 사망 이벤트 호출 
        if (CurrentHealth <= 0f && gameObject.CompareTag("Enemy"))
       {
            
            CallDeath();
            return true;
        }
        // 변동값이 양수면 힐 이벤트 호출
        if (change >= 0)
        {
            OnHeal?.Invoke();
            Debug.Log("데미지입음");
        }
        // 변동값이 음수면 데미지 이벤트 호출
        else
        {
            OnDamage?.Invoke();
            isAttacked = true;  // 피격 상태로 변경
        }
        return true;
    }

    private void CallDeath()
    {
        float score = statsHandler.CurrentStat.attackSO.speed * statsHandler.CurrentStat.attackSO.power;
        DataManager.Instance.IncrementScore(score * 0.5f);
        StartCoroutine(DeathSequence());
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
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("Death") ? "죽음" : "아님");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        OnDeath?.Invoke();
    }


    public void Invincibility(float duration)
    {
        if (!isInvincible)
        {
            isInvincible = true;
            OnInvincibilityStart?.Invoke(duration);

            Invoke(nameof(DisableInvincibility), duration);
        }
    }
    private void DisableInvincibility()
    {
        isInvincible = false;
        OnInvincibilityEnd?.Invoke();
    }
}