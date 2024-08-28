using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashashinEnemy : Enemy
{
    private BulletSpawner bulletSpawner;

    // 행동 관련 변수
    private float detectionRange = 13f;
    private float meleeRange = 6f;
    private float nextAttackTime = 0f;
    private float shootCooldown = 4f;
    private float meleeCooldown = 4f;
    private bool canMove = false;
    private bool animationPlaying = false;
    private bool startJump = false;
    public float speed = 2f;
    public float jumpForce = 10f;

    protected override void Start() {
        base.Start();
        bulletSpawner = GetComponentInChildren<BulletSpawner>();
        raySpacing = 0.25f;
        upScale = 0.1f;
    }


    void Update()
    {
        // 시간이 멈춰있거나 이 오브젝트가 죽은 상태면 return
        if (!GameManager.Instance.isLive || isDead)
        {
            return;
        }

        // 플레이어를 보게 하기 (스케일 조정)
        if (Player.Instance.GetPlayerLoc().x < transform.position.x) {
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

        float distanceToPlayer = Vector2.Distance(transform.position, Player.Instance.GetPlayerLoc());

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
        else if (distanceToPlayer < detectionRange)
        {
            canMove = false;

            // 공격 쿨타임이 지나면 점프 및 원거리 공격
            if (Time.time > nextAttackTime)
            {
                startJump = true;
                nextAttackTime = Time.time + shootCooldown;

                StartCoroutine(PerformJumpAttack());
            }

        }
    }

    private void FixedUpdate() {
        // 시간이 멈춰있거나 이 오브젝트가 죽은 상태면 return
        if (!GameManager.Instance.isLive || isDead)
        {
            return;
        }

        // canMove가 참이고 땅에 있고 앞에 땅이 있고 애니메이션이 실행 되지 않을 때
        if (canMove && !animationPlaying && IsGrounded() && IsGroundAhead() && Mathf.Abs(Player.Instance.GetPlayerLoc().x - transform.position.x) > 0.25f) 
        {
            MoveForward();
        } 
        else
        {
            animator.SetBool("isMoving", false);
        }

        if (startJump)
        {
            rb.velocity = Vector2.up * jumpForce;
            startJump = false;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(transform.position, 0.1f, groundLayer);
    }
    
    // 이동 함수
    private void MoveForward()
    {
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * dir);
        animator.SetBool("isMoving", true);
    }

    // 랜덤한 시점에 점프 공격을 실행하는 함수
    private IEnumerator PerformJumpAttack(){
        // 0초에서 1초
        float delay = Random.Range(0f, 1.5f);
        yield return new WaitForSeconds(delay);

        animator.SetTrigger("JumpAttack");
    }

    // 탄막 발사 함수
    private void FireBullet(){
        bulletSpawner.ShootFireBall();
    }

    // 애니메이션 이벤트 핸들러
    private void OnAnimationStart()
    {
        animationPlaying = true;
    }
    private void OnAnimationEnd()
    {
        animationPlaying = false;
    }
}
