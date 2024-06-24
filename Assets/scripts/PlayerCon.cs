using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCon : MonoBehaviour
{
    public float moveSpeed = 5f; // 플레이어의 이동 속도

    private Rigidbody2D rb; // 플레이어의 Rigidbody2D 컴포넌트

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트 가져오기
    }

    void Update()
    {
        // 플레이어 이동 입력 처리
        float moveHorizontal = Input.GetAxis("Horizontal"); // 좌우 이동 입력
        float moveVertical = Input.GetAxis("Vertical"); // 상하 이동 입력

        // 이동 방향 벡터 계산
        Vector2 moveDirection = new Vector2(moveHorizontal, moveVertical);

        // Rigidbody를 이용한 플레이어 이동
        rb.velocity = moveDirection.normalized * moveSpeed;
    }
}