using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public SceneLoader sceneLoader;
    public float moveSpeed = 3f; // 캐릭터의 이동 속도
    public float jumpForce = 8f; // 점프 힘
    public LayerMask groundLayer; // Ground 레이어 마스크
    public Transform groundCheck; // 땅 체크 위치
    public float groundCheckRadius = 0.2f; // 땅 체크 반경

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveDirection;
    
    [SerializeField]
    private bool isGrounded;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (transform.position.x > 50) // Ư�� ��ġ ��
        {
            sceneLoader.LoadNextScene();
        }

        GetInputs();
        if (isGrounded){
            animator.SetBool("IsSky", false);
            if (Input.GetButtonDown("Jump")){
                Jump();
            }
        } else {
            animator.SetBool("IsSky", true);
        } 
        
        if (Input.GetButtonDown("Fire1")) // Fire1 입력(기본적으로 좌클릭 또는 Ctrl 키)
        {
            Attack();
        }
    }

    void FixedUpdate()
    {
        Move();
        isGrounded = IsGrounded();
    }

    void GetInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); // 좌우 입력을 받음 (A, D 또는 화살표 키)
        moveDirection = new Vector2(moveX, 0).normalized; // 이동 방향 설정
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y); // 속도를 직접 설정하여 관성 없이 이동
        if (moveDirection.x != 0){
            spriteRenderer.flipX = moveDirection.x < 0;
        }
        // animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce); // y 축 속도를 점프 힘으로 설정
    }
    
    void Attack()
    {
        // animator.SetTrigger("attack"); // attack 트리거 설정
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

}
