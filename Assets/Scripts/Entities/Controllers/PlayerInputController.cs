using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerInputController : TopDownController
{
    private Camera _camera;

    public event Action OnPlayerAttack;

    protected override void Awake()
    {
        base.Awake();   // 부모의 Awake도 실행
        _camera = Camera.main;  // 메인 카메라 가져오기
        GameManager.Instance.playerHealthSystem = this.GetComponent<HealthSystem>();
    }

    // OnXXX 메소드는 사용자 입력 데이터를 정규화 등의 전처리 마치고
    // 관리 주체인 TopDownController로 전달
    public void OnMove(InputValue value)
    {
        Vector2 moveInput = value.Get<Vector2>().normalized;    // 움직임 입력을 정규화
        CallMoveEvent(moveInput);   // 정규화한 입력 정보 이벤트 메소드로 전달
    }

    public void OnLook(InputValue value)
    {
        Vector2 newAim = value.Get<Vector2>();  // 새로운 마우스 벡터 정보 가져오기
        Vector2 worldPos = _camera.ScreenToWorldPoint(newAim);  // 스크린의 마우스 위치를 월드 좌표로 변환
        newAim = (worldPos - (Vector2)transform.position).normalized;   // 월드 좌표의 마우스 벡터 정규화

        if (newAim.magnitude >= .9f)
        // Vector 값을 실수로 변환
        {
            CallLookEvent(newAim);
        }
    }

    public void OnFire(InputValue value)
    {
        // 마우스 좌클릭 이벤트 발생하면 공격중인 상태로 변경
        IsAttacking = value.isPressed;
        
    }

    public void CallAttack()
    {
        OnPlayerAttack?.Invoke();
    }
}