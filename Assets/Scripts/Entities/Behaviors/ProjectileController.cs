using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private LayerMask levelCollisionLayer; // 충돌한 레이어(벽)

    private RangedAttackSO attackData;
    private float currentDuration;
    private Vector2 direction;
    private bool isReady;   // 공격 준비가 완료되었는지 체크

    private Rigidbody2D r2bd;
    private SpriteRenderer spriteRenderer;
    private TrailRenderer trailRenderer;

    public bool fxOnDestory = true;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        r2bd = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        if (!isReady) return; // 준비되지 않았으면 동작하지 않음

        currentDuration += Time.deltaTime;  

        // 발사체의 현재 지속시간이 설정된 지속시간보다 크면
        if (currentDuration > attackData.duration)
        {
            DestroyProjectile(transform.position, false);   // 발사체 없애기
        }

        r2bd.velocity = direction * attackData.speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // levelCollisionLayer에 포함되는 레이어인지 확인
        if (IsLayerMatched(levelCollisionLayer.value, collision.gameObject.layer))
        {
            // 벽에서는 충돌한 지점으로부터 약간 앞 쪽에서 발사체를 파괴
            Vector2 destroyPosition = collision.ClosestPoint(transform.position) - direction * .2f;
            DestroyProjectile(destroyPosition, fxOnDestory);
        }
        // _attackData.target에 포함되는 레이어인지 확인
        else if (IsLayerMatched(attackData.target.value, collision.gameObject.layer))
        {
            // 충돌한 오브젝트에서 HealthSystem 컴포넌트를 가져오기
            HealthSystem healthSystem = collision.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                // 충돌한 오브젝트의 체력 감소시키기
                bool isAttackApplied = healthSystem.ChangeHealth(-attackData.power);

                // 넉백이 활성화된 경우, 넉백 적용
                if (isAttackApplied && attackData.isOnKnockback)
                {
                    ApplyKnockback(collision);
                }
            }
            // 충돌한 지점에서 발사체를 파괴
            DestroyProjectile(collision.ClosestPoint(transform.position), fxOnDestory);
        }
    }

    // 레이어가 일치하는지 확인하는 메소드
    private bool IsLayerMatched(int layerMask, int objectLayer)
    {
        return layerMask == (layerMask | (1 << objectLayer));
    }
    // 넉백을 적용하는 메소드
    private void ApplyKnockback(Collider2D collision)
    {
        TopDownMovement movement = collision.GetComponent<TopDownMovement>();
        if (movement != null)
        {
            movement.ApplyKnockback(transform, attackData.knockbackPower, attackData.knockbackTime);
        }
    }
    public void InitializeAttack(Vector2 direction, RangedAttackSO attackData)
    {
        this.attackData = attackData;
        this.direction = direction;

        UpdateProjectileSprite();
        trailRenderer.Clear();
        currentDuration = 0;
        spriteRenderer.color = attackData.projectileColor;

        transform.right = this.direction;

        isReady = true;
    }

    private void UpdateProjectileSprite()
    {
        transform.localScale = Vector3.one * attackData.size;
    }

    private void DestroyProjectile(Vector3 position, bool createFx)
    {
        if (createFx)
        {
            if (createFx && attackData != null && attackData.particleSystem != null)
            {
                // 파티클 시스템을 인스턴스화하고 위치 설정
                ParticleSystem instantiatedFx = Instantiate(attackData.particleSystem, position, Quaternion.identity);
                instantiatedFx.Play(); // 파티클 재생
                Destroy(instantiatedFx.gameObject, instantiatedFx.main.duration); // 파티클이 재생된 후 삭제
            }
            gameObject.SetActive(false); // 투사체 비활성화
        }
        gameObject.SetActive(false);
    }
}