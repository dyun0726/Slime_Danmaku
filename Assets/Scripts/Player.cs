using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    // 
    private bool facingRight = true; // 캐릭터가 오른쪽을 향하는지 여부
    public LayerMask groundLayer; // Ground 레이어 마스크
    public Transform groundCheck; // 땅 체크 위치
    public float groundCheckHeight = 0.2f; // 땅 체크 반경

    // 캐릭터 스탯 저장
    public float strength;
    public float agility; // 민첩성 (10초에 x번)
    public float magic; // 마력
    public float castingSpeed; // 마법 시전 속도 (10초에 x번)

    public float moveSpeed;
    public float jumpForce;
    public float knockbackSpeed ;


    // 필요한 컴포넌트
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveDirection;
    private BoxCollider2D boxCollider2D;
    private LangedController langedController;

    private Vector2 groundCheckBox;

    [SerializeField]
    private bool isGrounded;

    private bool isDamaged;
    private float cannotMoveTime = 0.2f;
    private float cannotMoveTimer;

    // RangeAttack variables
    private bool canRangeAttack = true;
    // public float RangeAttackTime = 1f;
    private float RangeAttackTimer;

    // MeleeAttack variables

    private bool canMeleeAttack = true;
    // public float MeleeAttackTime = 1f;
    private float MeleeAttackTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        groundCheckBox = new Vector2(boxCollider2D.size.x - 0.05f, groundCheckHeight);
        langedController = transform.GetChild(1).GetComponent<LangedController>();

        if (PlayerManager.Instance != null){
            PlayerManager.Instance.RegisterPlayer(gameObject);
            PlayerManager.Instance.UpdateStats();
            Debug.Log("Player stats have been loaded");
        }

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
                RangeAttackTimer = 10f / castingSpeed;
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
                MeleeAttackTimer = 10f / agility;
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

    void GetInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); // 좌우 입력을 받음 (A, D 또는 화살표 키)
        moveDirection = new Vector2(moveX, 0).normalized; // 이동 방향 설정
    }

    private void Filp(){
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y); // 속도를 직접 설정하여 관성 없이 이동
        
        if (moveDirection.x < 0 && facingRight){
            Filp();
        } else if (moveDirection.x > 0 && !facingRight){
            Filp();
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce); // y 축 속도를 점프 힘으로 설정
    }

    void RangeAttack()
    {
        langedController.shootBullet(facingRight);
    }

    void MeleeAttack(){
        animator.SetTrigger("Melee");
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, groundCheckBox, 0f, groundLayer);;
    }

    // isGrounded에서 사용하는 박스를 에디터에서 시각적으로 보여줌
    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireCube(groundCheck.position, groundCheckBox);
    // }

    // dir 방향으로 넉백시키는 함수
    public void Knockback(Vector2 dir){
        isDamaged = true;
        cannotMoveTimer = cannotMoveTime;
        rb.velocity = dir * knockbackSpeed;
        animator.SetTrigger("Damaged");

    }

}