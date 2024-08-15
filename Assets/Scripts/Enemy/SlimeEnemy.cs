using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemy : Enemy
{
    public float detectionRange = 10f;
    public float jumpForce = 4f;
    public float jumpCooldown = 4f;
    public float shootCooldown = 4f;
    private bool doJump = false;
    
    private BulletSpawner bulletSpawner;
    private float nextJumpTime = 0f;
    private float nextShootTime = 0f;
    private Vector2 jumpDirection;
    private Transform groundCheck;

    protected override void Start() {
        base.Start();
        bulletSpawner = GetComponentInChildren<BulletSpawner>(); 
        groundCheck = transform.GetChild(1);
        detectionDistance = 0.5f;
    }

    private void Update() {
        // 시간이 멈춰있거나 이 오브젝트가 죽은 상태면 return
        if (!GameManager.Instance.isLive || isDead)
        {
            return;
        }

        // 플레이어를 보게 하기
        spriteRenderer.flipX = Player.Instance.GetPlayerLoc().x < transform.position.x;
        
        if (isAtkReduced){
            atkReductionTimer -= Time.deltaTime;
            if (atkReductionTimer < 0){
                isAtkReduced = false;
                atkReduction = 0;
            }
        }

        float distanceToPlayer = Vector2.Distance(transform.position, Player.Instance.GetPlayerLoc());
        // 플레이어가 인식 범위 내에 있을 때
        if (distanceToPlayer < detectionRange)
        {
            // 땅에 있을 때만 점프하고, 예상 도착 위치에 땅이 있을 때만 점프
            if (Time.time > nextJumpTime && IsGround() && CheckFutureGround())
            {
                doJump = true;
                // JumpTowardsPlayer();
                nextJumpTime = Time.time + jumpCooldown;
                // 다음 슈팅 시간 vs 점프 후 0.5초 후 중 더 느린 것을 선택
                nextShootTime = Mathf.Max(nextShootTime, Time.time + 0.5f); 
            }

            if (Time.time > nextShootTime)
            {
                bulletSpawner.ShootFireBall();
                nextShootTime = Time.time + shootCooldown;
            }
        }
    }

    private void FixedUpdate() {
        if (doJump) 
        {
            rb.velocity = jumpDirection * jumpForce;
            doJump = false;
        }
    }
    
    // 점프할 방향 계산 함수
    private Vector2 CalculateJumpDirection(){
        Vector2 direction = (Player.Instance.GetPlayerLoc() - transform.position).normalized;
        return new Vector2(direction.x, 1).normalized;
    }

    // 점프시 도착할 곳 계산
    private Vector2 CalculateFuturePosition(Vector2 jumpDirection)
    {
        // 현재 위치에서 점프 방향으로 점프 힘을 곱하여 예상 도착 위치 계산
        float timeToApex = jumpForce / Physics2D.gravity.magnitude; // 최고점까지 도달하는 시간
        float totalJumpTime = timeToApex * 2; // 점프 전체 시간
        float horizontalDistance = jumpDirection.x * jumpForce * totalJumpTime; // 수평 이동 거리

        return new Vector2(groundCheck.position.x + horizontalDistance, groundCheck.position.y);
    }

    // 도착할 곳이 땅인지 계산
    private bool CheckFutureGround()
    {
        // 점프할 방향 계산
        jumpDirection = CalculateJumpDirection();

        // 예상 도착 위치 계산
        Vector2 futurePosition = CalculateFuturePosition(jumpDirection);

        // Raycast를 발사하여 땅과의 충돌 여부를 확인
        RaycastHit2D hit = Physics2D.Raycast(futurePosition, Vector2.down, detectionDistance, groundLayer);
        if (hit.collider == null)
        {
            Debug.DrawRay(futurePosition, Vector2.down * detectionDistance, Color.red);
            return false;
        }
        return true;
    }

    private bool IsGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, detectionDistance, groundLayer);
        // Raycast를 발사하여 땅과의 충돌 여부를 확인
        if (hit.collider == null)
        {
            Debug.DrawRay(groundCheck.position, Vector2.down * detectionDistance, Color.red);
            return false;
        }
        return true;
    }
}
