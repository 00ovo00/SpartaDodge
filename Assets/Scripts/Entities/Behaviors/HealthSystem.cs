using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float healthChangeDelay = 0.5f;    // 무적 시간

    private CharacterStatHandler statsHandler;
    private float timeSinceLastChange = float.MaxValue; // 마지막 공격을 받고 경과한 시간
    private bool isAttacked = false;

    // 체력이 변했을 때 할 행동들을 정의하고 적용 가능
    public event Action OnDamage;
    public event Action OnHeal;
    public event Action OnDeath;
    public event Action OnInvincibilityEnd;

    public float CurrentHealth { get; private set; }

    // get만 구현된 것처럼 프로퍼티를 사용하는 것
    // 데이터 유일성 & 정합성 유지
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
        // 무적 시간에는 체력이 닳지 않음
        if (timeSinceLastChange < healthChangeDelay)
        {
            return false;
        }

        timeSinceLastChange = 0f;
        CurrentHealth += change;
        // [최솟값을 0, 최댓값을 MaxHealth로 하는 구문]
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        // CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth;
        // CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth; 와 같아요!

        // 플레이어 체력 콘솔 출력으로 확인
        // TODO : UI에서 확인하도록 수정
        if (gameObject.name == "Character")
            Debug.Log(CurrentHealth);

        // 플레이어 체력이 0 이하면 게임 오버 호출
        if (gameObject.CompareTag("Player") && CurrentHealth <= 0f)
        {
            GameManager.Instance.GameOver();
            return true;
        }
        // 적 체력이 0 이하면 사망 이벤트 호출 
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