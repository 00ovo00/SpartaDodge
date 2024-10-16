using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownAimRotation : MonoBehaviour
{
    // 활
    [SerializeField] private SpriteRenderer armRenderer;
    [SerializeField] private Transform armPivot;

    // 캐릭터
    [SerializeField] private SpriteRenderer characterRenderer;

    private TopDownController _controller;  // 관리 주체

    private void Awake()
    {
        _controller = GetComponent<TopDownController>();
    }

    void Start()
    {
        // 마우스의 위치가 들어오는 OnLookEvent에 등록
        // 마우스의 위치를 받아서 팔을 돌리는 데 활용
        _controller.OnLookEvent += OnAim;
    }

    public void OnAim(Vector2 newAimDirection)
    {
        // OnLook
        // 활의 방향을 회전(마우스 방향 따라)
        RotateArm(newAimDirection);
    }

    private void RotateArm(Vector2 direction)
    {
        // Atan2는 직각삼각형이 있다고 할 때 세로가 y, 가로가 x일 때 그 각도를 라디안 [-Pi,Pi]로 나타내는 함수
        // 라디안의 -Pi는 -180도, Pi는 180도 이므로 Mathf.Rad2Deg는 약 57.29임 (180 / 3.14)
        // 오일러 함수에 넣기 위해 라디안 -> 오일러 각으로 변환
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // [1. 캐릭터 뒤집기]
        // 이때 각도는 오른쪽(1,0 방향)이 0도이므로,
        // -90~90도에서는 오른쪽을 바라보고, -90도 미만 90도 초과라면 왼쪽을 바라봄
        characterRenderer.flipX = Mathf.Abs(rotZ) > 90f;
        // 상하대칭 아닌 무기 있으므로 뒤집어주기
        armRenderer.flipY = characterRenderer.flipX;

        // [2. 팔 돌리기]
        // 팔을 돌릴 때는 나온 각도를 그대로 적용하는데, 이때 유니티 내부에서 사용하는 쿼터니언으로 변환.
        // 쿼터니으로 변형하는 방법 두 가지
        // 1) Vector3를 Quaternion으로 변환해서 넣는 방법
        //    Quaternion.Euler(x 회전, y 회전, z 회전) : 오일러 각 기준으로 값을 넣으면 쿼터니언으로 변환됨
        // 2) eulerAngles를 통해 자동으로 변환되게 하는 방법 - rotation이랑 비슷하게 변환.
        //    Transform.eulerAngles을 변경
        armPivot.rotation = Quaternion.Euler(0, 0, rotZ);
        // (2번 방법으로 하면) armPivot.eulerAngles = new Vector3(0, 0, rotZ);
        // 캐릭터가 마우스를 바라보는 각도
    }
}
