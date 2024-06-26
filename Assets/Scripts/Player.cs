using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float moveSpeed = 3f; // 캐릭?�의 ?�동 ?�도
    public float jumpForce = 8f; // ?�프 ??
    public LayerMask groundLayer; // Ground ?�이??마스??
    public Transform groundCheck; // ??체크 ?�치
    public float groundCheckRadius = 0.2f; // ??체크 반경

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

        GetInputs();
        if (isGrounded && Input.GetButtonDown("Jump")){
                Jump();
        } 
        
        if (Input.GetButtonDown("Fire1")) // Fire1 ?�력(기본?�으�?좌클�??�는 Ctrl ??
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
        float moveX = Input.GetAxisRaw("Horizontal"); // 좌우 ?�력??받음 (A, D ?�는 ?�살????
        moveDirection = new Vector2(moveX, 0).normalized; // ?�동 방향 ?�정
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y); // ?�도�?직접 ?�정?�여 관???�이 ?�동
        if (moveDirection.x != 0){
            spriteRenderer.flipX = moveDirection.x < 0;
        }
        // animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce); // y �??�도�??�프 ?�으�??�정
    }
    
    void Attack()
    {
        animator.SetTrigger("attack"); // attack ?�리�??�정
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);;
    }

}
