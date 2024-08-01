// using Cainos.LucidEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;
    public static Player Instance
    {
        get{
            if (instance == null){
                instance = FindObjectOfType<Player>();
                if (instance == null){
                    Debug.LogError("Player is not in scene");
                }
            }
            return instance;
        }
    }

    private bool facingRight = true; // 캐릭터가 오른쪽을 향하는지 여부
    public LayerMask groundLayer; // Ground 레이어 마스크
    public Transform groundCheck; // 땅 체크 위치
    public float groundCheckHeight = 0.2f; // 땅 체크 반경

    // 캐릭터 스탯 저장
    public float agility; // 민첩성 (10초에 x번)
    public float castingSpeed; // 마법 시전 속도 (10초에 x번)
    public float moveSpeed;
    public float jumpForce;
    public float knockbackSpeed;
    public float jumpstack; // 최대 점프 횟수
    public float stance;
    private float gravity = 3f;
    public float gravityMultiplier = 0f;


    // 필요한 컴포넌트
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveDirection;
    private BoxCollider2D boxCollider2D;
    private LangedController langedController;
    private SpriteRenderer spriteRenderer;

    private Vector2 groundCheckBox;

    [SerializeField]
    public bool isGrounded;
    private bool isJumping;

    private bool isDamaged;
    private float cannotMoveTime = 0.2f;
    private float cannotMoveTimer;

    // RangeAttack variables
    private bool canRangeAttack = true;
    private float RangeAttackTimer;

    // MeleeAttack variables
    private bool canMeleeAttack = true;
    private float MeleeAttackTimer;

    // Jump 관련 변수
    private int currentJumpCount; // 현재 점프 횟수

    private void Awake() {
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this){
            Destroy(gameObject);
        }

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        groundCheckBox = new Vector2(boxCollider2D.size.x - 0.05f, groundCheckHeight);
        langedController = transform.GetChild(1).GetComponent<LangedController>();
    }

    void Start()
    {
        if (PlayerManager.Instance != null)
        {
            // PlayerManager.Instance.RegisterPlayer(gameObject);
            PlayerManager.Instance.UpdateStats();
            Debug.Log("Player stats have been loaded");
        }
    }

    void Update()
    {
        if (!GameManager.Instance.isLive){  // live 체크 함수
            return;
        }
            
        if (!isDamaged)
        {
            GetInputs();
            if (isGrounded)
            {
                animator.SetBool("IsSky", false);
                currentJumpCount = 0; // 땅에 닿으면 점프 횟수 초기화
                if (Input.GetButtonDown("Jump"))
                {
                    isJumping = true;
                }
            }
            else
            {
                animator.SetBool("IsSky", true);
                if (Input.GetButtonDown("Jump") && currentJumpCount < jumpstack)
                {
                    isJumping = true;
                }
            }
        }
        else
        {
            cannotMoveTimer -= Time.deltaTime;
            if (cannotMoveTimer <= 0)
            {
                isDamaged = false;
            }
        }

        if (canRangeAttack)
        {
            if (Input.GetButtonDown("Fire1")) // Fire1 입력(기본적으로 좌클릭 또는 Ctrl 키)
            {
                RangeAttack();
                canRangeAttack = false;
                RangeAttackTimer = 10f / castingSpeed;
            }
        }
        else
        {
            RangeAttackTimer -= Time.deltaTime;
            if (RangeAttackTimer <= 0)
            {
                canRangeAttack = true;
            }
        }

        if (canMeleeAttack)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                MeleeAttack();
                canMeleeAttack = false;
                MeleeAttackTimer = 10f / agility;
            }
        }
        else
        {
            MeleeAttackTimer -= Time.deltaTime;
            if (MeleeAttackTimer <= 0)
            {
                canMeleeAttack = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.isLive){  // live 체크 함수
            return;
        }
        
        if (!isDamaged)
        {
            Move();
            isGrounded = IsGrounded();
        }

        if (isJumping){
            Jump();
            isJumping = false;
        }

        // 캐릭터가 떨어지는 중이면
        if (rb.velocity.y < 0)
        { 
            rb.gravityScale = gravity * (1f - gravityMultiplier/100f);
        }
        else
        {
            rb.gravityScale = 1f;
        }
    }

    void GetInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); // 좌우 입력을 받음 (A, D 또는 화살표 키)
        moveDirection = new Vector2(moveX, 0).normalized; // 이동 방향 설정
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y); // 속도를 직접 설정하여 관성 없이 이동

        if (moveDirection.x < 0 && facingRight)
        {
            Flip();
        }
        else if (moveDirection.x > 0 && !facingRight)
        {
            Flip();
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce); // y 축 속도를 점프 힘으로 설정
        currentJumpCount++; // 점프 횟수 증가
    }

    void RangeAttack()
    {
        langedController.shootBullet(facingRight);
    }

    void MeleeAttack()
    {
        animator.SetTrigger("Melee");
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, groundCheckBox, 0f, groundLayer);
    }

    public void Knockback(Vector2 dir)
    {
        isDamaged = true;
        cannotMoveTimer = cannotMoveTime;

        // PlayerManager에서 stance 값 가져오기
        // stance 값에 따라 넉백 속도를 감소시킴
        float knockbackMultiplier = 1f - (stance / 100f);
        rb.velocity = dir * knockbackSpeed * knockbackMultiplier;

        animator.SetTrigger("Damaged");
    }

    public Vector3 GetPlayerLoc(){
        return transform.position;
    }

    public void ActivatePlayer(){
        gameObject.SetActive(true);
    }

    public void DeactivatePlayer(){
        gameObject.SetActive(false);
    }

    public void SetSpriteAnimatorController(CharacterInfo characterInfo){
        spriteRenderer.sprite = characterInfo.sprite;
        animator.runtimeAnimatorController = characterInfo.animatorController;
    }
}
