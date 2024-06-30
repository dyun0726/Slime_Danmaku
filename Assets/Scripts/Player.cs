using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 3f; // 캐릭터의 이동 속도
    public float jumpForce = 8f; // 점프 힘
    public LayerMask groundLayer; // Ground 레이어 마스크
    public Transform groundCheck; // 땅 체크 위치
    public float groundCheckHeight = 0.2f; // 땅 체크 반경
    public float knockbackSpeed = 10f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveDirection;
    private BoxCollider2D boxCollider2D;
    private LangedController langedController;

    private Vector2 groundCheckBox;

    [SerializeField]
    private bool isGrounded;
    private SpriteRenderer spriteRenderer;

    private bool isDamaged;
    private float cannotMoveTime = 0.2f;
    private float cannotMoveTimer;

    // RangeAttack variables
    private bool canRangeAttack = true;
    public float RangeAttackTime = 2f;
    private float RangeAttackTimer;

    // MeleeAttack variables

    private bool canMeleeAttack = true;
    public float MeleeAttackTime = 1f;
    private float MeleeAttackTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        groundCheckBox = new Vector2(boxCollider2D.size.x - 0.05f, groundCheckHeight);
        langedController = transform.GetChild(1).GetComponent<LangedController>();

        Debug.Log(langedController);
    }

    void Update()
    {
        if (!isDamaged){
            GetInputs();
            if (isGrounded)
            {
                animator.SetBool("IsSky", false);
                if (Input.GetButtonDown("Jump"))
                {
                    Jump();
                }
            }
            else
            {
                animator.SetBool("IsSky", true);
            }
        }
        else {
            cannotMoveTimer -= Time.deltaTime;
            if (cannotMoveTimer <= 0){
                isDamaged = false;
            }
        }

        if (canRangeAttack){
            if (Input.GetButtonDown("Fire1")) // Fire1 입력(기본적으로 좌클릭 또는 Ctrl 키)
            {
                RangeAttack();
                canRangeAttack = false;
                RangeAttackTimer = RangeAttackTime;
            }
        } else {
            RangeAttackTimer -= Time.deltaTime;
            if (RangeAttackTimer <= 0){
                canRangeAttack = true;
            }
        }
        

        if (canMeleeAttack){
            if (Input.GetButtonDown("Fire2")){
                MeleeAttack();
                canMeleeAttack = false;
                MeleeAttackTimer = MeleeAttackTime;
            } 
        } else {
            MeleeAttackTimer -= Time.deltaTime;
            if (MeleeAttackTimer <= 0){
                canMeleeAttack = true;
            }
        }

    }

    void FixedUpdate()
    {
        if (!isDamaged){
            Move();
            isGrounded = IsGrounded();
        }
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Enemy")){
            Vector2 dir = (transform.position - other.transform.position).normalized;
            GetDamaged(dir);
        }
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == 7){ // 7번이 Enemy Bullet
            Vector2 dir = (transform.position - other.transform.position).normalized;
            GetDamaged(dir);
            // 풀링 해제
            other.gameObject.GetComponent<Bullet>().ReleaseObject();
        }
    }

    void GetInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); // 좌우 입력을 받음 (A, D 또는 화살표 키)
        moveDirection = new Vector2(moveX, 0).normalized; // 이동 방향 설정
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y); // 속도를 직접 설정하여 관성 없이 이동
        if (moveDirection.x != 0)
        {
            spriteRenderer.flipX = moveDirection.x < 0;
        }
        // animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce); // y 축 속도를 점프 힘으로 설정
    }

    void RangeAttack()
    {
        langedController.shootBullet(spriteRenderer.flipX);
        // animator.SetTrigger("attack"); // attack 트리거 설정
    }

    void MeleeAttack(){
        animator.SetTrigger("Melee");
    }

    bool IsGrounded()
    {
        
        return Physics2D.OverlapBox(groundCheck.position, groundCheckBox, 0f, groundLayer);;
    }

    // isGrounded에서 사용하는 박스를 에디터에서 시각적으로 보여줌
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckBox);
    }

    void GetDamaged(Vector2 dir){
        isDamaged = true;
        cannotMoveTimer = cannotMoveTime;
        rb.velocity = dir * knockbackSpeed;
        animator.SetTrigger("Damaged");
    }

}