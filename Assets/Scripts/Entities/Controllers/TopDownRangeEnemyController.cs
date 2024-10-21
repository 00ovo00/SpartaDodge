using UnityEngine;

public class TopDownRangeEnemyController : TopDownEnemyController
{
    [SerializeField] private float followRange = 15f;   // 추적 범위
    [SerializeField] private float shootRange = 10f;    // 공격 범위
    private int layerMaskLevel;     // 벽 충돌 레이어
    private int layerMaskTarget;    // 타겟 충돌 레이어

    protected override void Start()
    {
        base.Start();
        layerMaskLevel = LayerMask.NameToLayer("Level");
        layerMaskTarget = stats.CurrentStat.attackSO.target;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        float distanceToTarget = DistanceToTarget();
        Vector2 directionToTarget = DirectionToTarget();

        UpdateEnemyState(distanceToTarget, directionToTarget);
    }

    private void UpdateEnemyState(float distance, Vector2 direction)
    {
        IsAttacking = false; // 기본적으로 공격 상태를 false로 설정

        // 추적범위 내에 있으면 근처에 있는지 확인
        if (distance <= followRange)
        {
            CheckIfNear(distance, direction);
        }
    }

    private void CheckIfNear(float distance, Vector2 direction)
    {
        // 타겟이 공격 범위 내에 있으면 공격
        if (distance <= shootRange)
        {
            TryShootAtTarget(direction);
        }
        // 타겟이 공격 범위 외에 있고 추적 범위 내에 있으면 추격
        else
        {
            CallMoveEvent(direction);
        }
    }

    private void TryShootAtTarget(Vector2 direction)
    {
        // 몬스터 위치에서 direction 방향으로 레이 발사
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, shootRange, GetLayerMaskForRaycast());

        // 벽에 맞은게 아니라 실제 플레이어에 맞았는지 확인
        if (IsTargetHit(hit))
        {
            PerformAttackAction(direction);
        }
        else
        {
            CallMoveEvent(direction);// 타겟을 맞추지 못하면 추격
        }
    }

    private int GetLayerMaskForRaycast()
    {
        // "Level" 레이어와 타겟 레이어 모두를 포함하는 LayerMask를 반환
        return (1 << layerMaskLevel) | layerMaskTarget;
    }

    private bool IsTargetHit(RaycastHit2D hit)
    {
        // RaycastHit2D 결과를 바탕으로 실제 타겟을 명중했는지 확인
        return hit.collider != null && layerMaskTarget == (layerMaskTarget | (1 << hit.collider.gameObject.layer));
    }

    private void PerformAttackAction(Vector2 direction)
    {
        // 타겟을 정확히 명중했을 경우의 행동을 정의
        CallLookEvent(direction);
        CallMoveEvent(Vector2.zero); // 공격 중에는 이동 중지
        IsAttacking = true;
    }
}
