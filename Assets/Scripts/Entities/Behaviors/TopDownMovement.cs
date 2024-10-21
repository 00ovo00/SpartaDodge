using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    private TopDownController movementController;   // 관리 주체
    private CharacterStatHandler characterStatHandler;
    private Rigidbody2D movementRigidbody;
    // 움직임에 필요한 세 스크립트 중 유일하게 RigidBody 컴포넌트 필요
    // 실제로 이동이 일어나는 실행 주체 역할

    private Vector2 movementDirection = Vector2.zero;
    private Vector2 knockback = Vector2.zero;
    private float knockbackDuration  = 0.0f;

    private void Awake()
    {
        // Awake는 주로 자신의 컴포넌트에서 일어나는 로직 처리
        movementController = GetComponent<TopDownController>();
        movementRigidbody = GetComponent<Rigidbody2D>();
        characterStatHandler = GetComponent<CharacterStatHandler>();
    }

    private void Start()
    {
        // OnMoveEvent에 Move를 호출하라고 등록
        movementController.OnMoveEvent += Move;
    }

    private void FixedUpdate()
    {
        // 물리 업데이트에서 움직임 적용
        ApplyMovement(movementDirection);
        if (knockbackDuration > 0.0f)
        {
            knockbackDuration -= Time.fixedDeltaTime;
        }    
    }

    private void Move(Vector2 direction)
    {
        // 이동방향만 정해두고 실제로 움직이지는 않음.
        // 움직이는 것은 물리 업데이트에서 진행(rigidbody가 물리니까)
        movementDirection = direction;
    }
    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        knockback = -(other.position - transform.position).normalized * power;
    }

    private void ApplyMovement(Vector2 direction)
    {
        // 실제 움직임 적용
        direction = direction * characterStatHandler.CurrentStat.speed;
        if (knockbackDuration > 0.0f)
        {
            direction += knockback;
        }
        movementRigidbody.velocity = direction;
    }
}
