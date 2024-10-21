using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownContactEnemyController : TopDownEnemyController
{
    [SerializeField][Range(0f, 100f)] private float followRange;
    [SerializeField] private string targetTag = "Player";
    private bool isCollidingWithTarget;

    [SerializeField] private SpriteRenderer characterRenderer;

    private HealthSystem healthSystem;
    private HealthSystem collidingTargetHealthSystem;
    private TopDownMovement collidingMovement;

    protected override void Start()
    {
        base.Start();

        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDamage += OnDamage;
    }

    private void OnDamage()
    {
        followRange = 100f;
    }
    protected override void FixedUpdate()
    {
        // 단거리적은 플레이어처럼 입력 받아 움직이는 것이 아니므로
        // 어떤 식으로 움직일 지 로직 구현 필요

        base.FixedUpdate();

        if (isCollidingWithTarget)
        {
            ApplyHealthChange();
        }

        Vector2 direction = Vector2.zero;
        if (DistanceToTarget() < followRange)
        {
            direction = DirectionToTarget();
        }

        CallMoveEvent(direction);
        Rotate(direction);
    }

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        characterRenderer.flipX = Mathf.Abs(rotZ) > 90f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject receiver = collision.gameObject;

        if (!receiver.CompareTag(targetTag))
        {
            return;
        }

        collidingTargetHealthSystem = receiver.GetComponent<HealthSystem>();
        if (collidingTargetHealthSystem != null)
        {
            isCollidingWithTarget = true;
        }

        collidingMovement = receiver.GetComponent<TopDownMovement>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag(targetTag))
        {
            return;
        }

        isCollidingWithTarget = false;
    }

    private void ApplyHealthChange()
    {
        AttackSO attackSO = stats.CurrentStat.attackSO;
        bool hasBeenChanged = collidingTargetHealthSystem.ChangeHealth(-attackSO.power);
        Debug.Log("현재 공격력" + attackSO.power);
        if (attackSO.isOnKnockback && collidingMovement != null)
        {
            collidingMovement.ApplyKnockback(transform, attackSO.knockbackPower, attackSO.knockbackTime);
        }
    }
}