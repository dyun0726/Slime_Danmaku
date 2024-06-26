using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float moveSpeed = 3f; // μΊλ¦­?°μ ?΄λ ?λ
    public float jumpForce = 8f; // ?ν ??
    public LayerMask groundLayer; // Ground ?μ΄??λ§μ€??
    public Transform groundCheck; // ??μ²΄ν¬ ?μΉ
    public float groundCheckRadius = 0.2f; // ??μ²΄ν¬ λ°κ²½

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
        
        if (Input.GetButtonDown("Fire1")) // Fire1 ?λ ₯(κΈ°λ³Έ?μΌλ‘?μ’ν΄λ¦??λ Ctrl ??
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
        float moveX = Input.GetAxisRaw("Horizontal"); // μ’μ° ?λ ₯??λ°μ (A, D ?λ ?μ΄????
        moveDirection = new Vector2(moveX, 0).normalized; // ?΄λ λ°©ν₯ ?€μ 
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y); // ?λλ₯?μ§μ  ?€μ ?μ¬ κ΄???μ΄ ?΄λ
        if (moveDirection.x != 0){
            spriteRenderer.flipX = moveDirection.x < 0;
        }
        // animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce); // y μΆ??λλ₯??ν ?μΌλ‘??€μ 
    }
    
    void Attack()
    {
        animator.SetTrigger("attack"); // attack ?Έλ¦¬κ±??€μ 
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);;
    }

}
