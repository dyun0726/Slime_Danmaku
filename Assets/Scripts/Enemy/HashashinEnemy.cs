using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashashinEnemy : Enemy
{
    private Rigidbody2D rb;
    private BulletSpawner bulletSpawner;

    // 땅 탐지 관련 변수
    private float detectionDistance = 1.0f; // Raycast로 탐지할 거리
    private float raySpacing = 0.25f; // 광선 사이의 간격
    public LayerMask groundLayer; // 땅 레이어 마스크
    private float upScale = 0.1f; // 보정을 위한 변수

    // 행동 관련 변수
    private float detectionRange = 10f;
    private float meleeRange = 5f;
    private float nextAttackTime = 0f;
    private float shootCooldown = 5f;
    private float meleeCooldown = 5f;
    private bool canMove = false;
    private bool startJump = false;
    private bool isJumping = false;
    private bool isGround;
    private Vector2 dir = Vector2.right;
    public float speed = 1f;
    public float jumpForce = 10f;

    protected override void Start() {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        bulletSpawner = GetComponentInChildren<BulletSpawner>(); 
    }


    void Update()
    {
        // live 체크
        if (!GameManager.Instance.isLive){  
            return;
        }

        // 플레이어를 보게 하기 (스케일 조정)
        if (PlayerManager.Instance.GetPlayerLoc().x < transform.position.x) {
            transform.localScale = new Vector3(-1, 1, 1);
            dir = Vector2.left;
        } else {
            transform.localScale = Vector3.one;
            dir = Vector2.right;
        }


        // 애니메이션 관련 동작
        if (IsGrounded()){
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        } 
        else 
        {
            if (rb.velocity.y >= 0)
            {
                animator.SetBool("isJumping", true);
            }
            else
            {
                animator.SetBool("isFalling", true);
            }
            
        }

        if (isAtkReduced){
            atkReductionTimer -= Time.deltaTime;
            if (atkReductionTimer < 0){
                isAtkReduced = false;
                atkReduction = 0;
            }
        }

        if (isStuned) {
            stunTimer -= Time.deltaTime;
            if (stunTimer < 0){
                isStuned = false;
            }
        }
        else
        {
            float distanceToPlayer = Vector2.Distance(transform.position, PlayerManager.Instance.GetPlayerLoc());

            // 일정 거리 안이면 돌격 및 근거리 공격
            if (distanceToPlayer < meleeRange)
                {
                    canMove = true;
                    
                    // 공격 쿨타임이 지났으면 근거리 공격 (근거리 공격 중에는 이동하지 않도록 변수 추가 필요)
                    if (Time.time > nextAttackTime){
                        animator.SetTrigger("Melee");
                        nextAttackTime = Time.time + meleeCooldown;
                    }
                }

            // 공격 쿨타임이 지나면
            if (Time.time > nextAttackTime){
                
                
                // 탐지 범위 안이면 점프 후 원거리 공격
                if (distanceToPlayer < detectionRange)
                {
                    startJump = true;
                    nextAttackTime = Time.time + shootCooldown;
                }
            }
        }
    }

    private void FixedUpdate() {
        // float distanceToPlayer = Vector2.Distance(transform.position, PlayerManager.Instance.GetPlayerLoc());

        if (startJump)
        {
            rb.velocity = Vector2.up * jumpForce;
            startJump = false;
        }
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(transform.position, 0.1f, groundLayer);
    }
}
