using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float moveSpeed = 3f; // ìºë¦­?°ì˜ ?´ë™ ?ë„
    public float jumpForce = 8f; // ?í”„ ??
    public LayerMask groundLayer; // Ground ?ˆì´??ë§ˆìŠ¤??
    public Transform groundCheck; // ??ì²´í¬ ?„ì¹˜
    public float groundCheckRadius = 0.2f; // ??ì²´í¬ ë°˜ê²½

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
        
        if (Input.GetButtonDown("Fire1")) // Fire1 ?…ë ¥(ê¸°ë³¸?ìœ¼ë¡?ì¢Œí´ë¦??ëŠ” Ctrl ??
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
        float moveX = Input.GetAxisRaw("Horizontal"); // ì¢Œìš° ?…ë ¥??ë°›ìŒ (A, D ?ëŠ” ?”ì‚´????
        moveDirection = new Vector2(moveX, 0).normalized; // ?´ë™ ë°©í–¥ ?¤ì •
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y); // ?ë„ë¥?ì§ì ‘ ?¤ì •?˜ì—¬ ê´€???†ì´ ?´ë™
        if (moveDirection.x != 0){
            spriteRenderer.flipX = moveDirection.x < 0;
        }
        // animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce); // y ì¶??ë„ë¥??í”„ ?˜ìœ¼ë¡??¤ì •
    }
    
    void Attack()
    {
        animator.SetTrigger("attack"); // attack ?¸ë¦¬ê±??¤ì •
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);;
    }

}
