using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    Rigidbody2D r2bd;

    [SerializeField]
    private float speed;

    void Start()
    {
        // Rigidbody2D 캐싱
        r2bd = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Vertical축과 Horizontal축의 입력을 받아오기
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        // 정규화
        // vertical, horizontal 모두 1인 경우, direction의 크기는 1보다 크게 될 수 있는데(루트2), 이를 1로 맞춰줌.
        Vector2 direction = new Vector2(horizontal, vertical);
        direction = direction.normalized;

        // rigidbody.velocity는 해당 물체가 1초당 움직이는 거리
        r2bd.velocity = direction * speed;
    }
}
