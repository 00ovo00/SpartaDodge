using System.Runtime.CompilerServices;
using UnityEngine;

public class TopDownShooting : MonoBehaviour
{
    private TopDownController controller;   // 관리 주체

    [SerializeField] private Transform projectileSpawnPosition; // 화살 생성 지점
    private Vector2 aimDirection = Vector2.right;   // 조준 방향
    private PlayerInputController playerInputController;
    private void Awake()
    {
        playerInputController= GetComponent<PlayerInputController>();
        controller = GetComponent<TopDownController>();
    }

    void Start()
    {
        controller.OnAttackEvent += OnShoot;

        /* 현재 OnLookEvent에 등록된 이벤트 */
        // 1. TopDownAimRotation.OnAim(Vec2)
        // 2. TopDownShooting.OnAim(Vec2)
        // 하나의 델리게이트에 여러 개 함수 등록(multicast delegate)
        controller.OnLookEvent += OnAim;
    }

    private void OnAim(Vector2 newAimDirection)
    {
        // 화살이 날아가는 방향 설정
        aimDirection = newAimDirection;
    }

    private void OnShoot(AttackSO attackSO)
    {
        // 원거리 공격만 다룸
        RangedAttackSO rangedAttackSO = attackSO as RangedAttackSO;
        if (rangedAttackSO == null) return;

        float projectilesAngleSpace = rangedAttackSO.multipleProjectilesAngel;  // 발사각
        int numberOfProjectilesPerShot = rangedAttackSO.numberofProjectilesPerShot; // 한번에 나가는 화살 개수

        // 중간부터 펼쳐지는게 아니라 minangle부터 커지면서 쏘는 것으로 설계 
        // 아래에서 위방향으로 순차적으로 커짐
        float minAngle = -(numberOfProjectilesPerShot / 2f) * projectilesAngleSpace + 0.5f * rangedAttackSO.multipleProjectilesAngel;


        for (int i = 0; i < numberOfProjectilesPerShot; i++)
        {
            float angle = minAngle + projectilesAngleSpace * i;
            // 랜덤으로 변하는 randomSpread를 추가
            float randomSpread = Random.Range(-rangedAttackSO.spread, rangedAttackSO.spread);
            angle += randomSpread;
            CreateProjectile(rangedAttackSO, angle);
        }
    }

    private void CreateProjectile(RangedAttackSO rangedAttackSO, float angle)
    {
        Debug.Log(rangedAttackSO);
        // 오브젝트 풀을 활용한 생성
        GameObject obj = SpawnManager.Instance.objectPool.SpawnFromPool(rangedAttackSO.bulletNameTag);
       
        // 발사체 기본 세팅
        Debug.Log("사격 위치"+ projectileSpawnPosition.position);
        obj.transform.position = projectileSpawnPosition.position;
        ProjectileController attackController = obj.GetComponent<ProjectileController>();
        attackController.InitializeAttack(RotateVector2(aimDirection, angle), rangedAttackSO);

        if (rangedAttackSO.bulletNameTag == "Arrow")
        {
            playerInputController.CallAttack();
        }
    }

    private static Vector2 RotateVector2(Vector2 v, float degree)
    {
        // 벡터 회전하기 : 쿼터니언 * 벡터 순
        return Quaternion.Euler(0, 0, degree) * v;
    }
}