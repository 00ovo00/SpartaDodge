using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class TopDownController : MonoBehaviour
{
    // 관리 주체에서 이벤트 함수 들고 있기
    // event 키워드 입력하면 public 이라도 여기서만 Invoke 가능
    public event Action<Vector2> OnMoveEvent;
    public event Action<Vector2> OnLookEvent;
    // OnAttackEvent는 눌렀을 때 공격 기준정보(AttackSO) 들고 옴
    public event Action<AttackSO> OnAttackEvent;


    // 마지막 공격 시각 저장
    // 마지막 공격 시점과 새로운 공격 이벤트 발생 시점 시간 구하기 위함
    private float timeSinceLastAttack = float.MaxValue;
    protected bool IsAttacking { get; set; }    // 공격 중인 상태인지 체크

    // private set으로 여기서만 값 수정 가능하도록 제한
    // protected로 상속 받은 클래스만 접근하도록 제한
    protected CharacterStatHandler stats {  get; private set; }

    protected virtual void Awake()
    {
        stats = GetComponent<CharacterStatHandler>();
    }

    protected virtual void Update()
    {
        HandleAttackDelay();
    }
    private void HandleAttackDelay()
    {
        // 연속 발사 제한 위해 시간차 조정
        if (timeSinceLastAttack <= stats.CurrentStat.attackSO.delay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }
        if (IsAttacking && timeSinceLastAttack > stats.CurrentStat.attackSO.delay)
        {
            timeSinceLastAttack = 0;
            // 현재 장착된 무기의 attackSO 전달
            CallAttackEvent(stats.CurrentStat.attackSO);
        }
    }

    // 이벤트 발생시 등록된 메소드 호출
    // OnXXX?.Invoke()로 null safe하게 사용
    public void CallMoveEvent(Vector2 direction)
    {
        OnMoveEvent?.Invoke(direction);
    }

    public void CallLookEvent(Vector2 direction)
    {
        OnLookEvent?.Invoke(direction);
    }
    public void CallAttackEvent(AttackSO attackSO)
    {
        OnAttackEvent?.Invoke(attackSO);
    }
}
