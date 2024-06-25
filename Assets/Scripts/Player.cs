using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
<<<<<<< HEAD
    public float speed = 3f;
    public float jumpForce = 5f;
    public float xInput;
    public bool isGround;
    public SceneLoader sceneLoader;
=======
    public float moveSpeed = 3f; // 캐릭터의 이동 속도
    public float jumpForce = 8f; // 점프 힘
    public LayerMask groundLayer; // Ground 레이어 마스크
    public Transform groundCheck; // 땅 체크 위치
    public float groundCheckRadius = 0.2f; // 땅 체크 반경
>>>>>>> 05def9fababf10ded75168599cd082d04dfa8032

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
<<<<<<< HEAD
        if (transform.position.x > 50) // Ư�� ��ġ ��
        {
            sceneLoader.LoadNextScene();
        }


    }

    private void FixedUpdate() {
        xInput =Input.GetAxis("Horizontal");
        float jumpInput = Input.GetAxisRaw("Jump");

        if (isGround && jumpInput != 0){
            playerRb.velocity = new Vector2(xInput * speed, playerRb.velocity.y + jumpInput * jumpForce);
            anim.SetBool("Jumping", true);
        } else {
            playerRb.velocity = new Vector2(xInput * speed, playerRb.velocity.y);
        }
        
    }

    private void LateUpdate() {
        anim.SetFloat("Speed", Mathf.Abs(xInput));
        if (xInput != 0){
            spriteRenderer.flipX = xInput < 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ground")){
            isGround = true;
            anim.SetBool("Jumping", false);
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ground")){
            isGround = false;
=======
        GetInputs();
        if (isGrounded && Input.GetButtonDown("Jump")){
                Jump();
>>>>>>> 05def9fababf10ded75168599cd082d04dfa8032
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
        animator.SetTrigger("attack"); // attack 트리거 설정
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);;
    }

}
