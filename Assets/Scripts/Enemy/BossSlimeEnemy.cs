using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlimeEnemy : Enemy
{
    public float detectionRange = 20f;
    public float jumpForce = 4f;
    public float jumpCooldown = 4f;
    public float shootCooldown = 4f;
    public LayerMask groundLayer;
    public Transform groundCheck;

    private Rigidbody2D rb;
    private BulletSpawner[] bulletSpawner;
    private bool isGrounded;
    private bool isFutureGrounded;
    private float nextJumpTime = 0f;
    private float nextShootTime = 0f;
    private Vector2 jumpDirection;


    protected override void Start() {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        bulletSpawner = GetComponentsInChildren<BulletSpawner>(); 
    }

    private void Update() {
        if (!GameManager.Instance.isLive){  // live 체크 함수
            return;
        }

        // 플레이어를 보게 하기
        spriteRenderer.flipX = PlayerManager.Instance.GetPlayerLoc().x < transform.position.x;
        
        if (isAtkReduced){
            atkReductionTimer -= Time.deltaTime;
            if (atkReductionTimer < 0){
                isAtkReduced = false;
                atkReduction = 0;
            }
        }

        // 보스는 스턴이 없음
        float distanceToPlayer = Vector2.Distance(transform.position, PlayerManager.Instance.GetPlayerLoc());
        // 플레이어가 인식 범위 내에 있을 때
        if (distanceToPlayer < detectionRange)
        {
            // 땅 체크
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
            isFutureGrounded = CheckFutureGround();

            // 땅에 있을 때만 점프하고, 예상 도착 위치에 땅이 있을 때만 점프
            if (isGrounded && isFutureGrounded && Time.time > nextJumpTime)
            {
                JumpTowardsPlayer();
                nextJumpTime = Time.time + jumpCooldown;
                // 다음 슈팅 시간 vs 점프 후 0.5초 후 중 더 느린 것을 선택
                nextShootTime = Mathf.Max(nextShootTime, Time.time + 0.5f); 
            }

            if (Time.time > nextShootTime)
            {
                // 랜덤으로 탄막 발사
                RandomFire();
                nextShootTime = Time.time + shootCooldown;
            }
        }
        
    }
    
    // 점프할 방향 계산 함수
    private Vector2 CalculateJumpDirection(){
        Vector2 direction = (PlayerManager.Instance.GetPlayerLoc() - transform.position).normalized;
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

        // 예상 도착 위치에 땅이 있는지 체크
        return Physics2D.OverlapCircle(futurePosition, 0.1f, groundLayer);
    }

    private void JumpTowardsPlayer(){
        // 좌우로 점프
        rb.velocity = jumpDirection * jumpForce;
    }

    // 랜덤으로 탄막 스포너를 선택해서 탄막 발사
    private void RandomFire(){
        int index = Random.Range(0, bulletSpawner.Length);
        bulletSpawner[index].ShootFireBall();
    }

    private void OnDrawGizmosSelected()
    {
        // 플레이어 인식 범위 시각화
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // 땅 체크 위치 시각화
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.position, 0.1f);

        // 예상 도착 위치 시각화
        if (Application.isPlaying)
        {
            Vector2 jumpDirection = CalculateJumpDirection();
            Vector2 futurePosition = CalculateFuturePosition(jumpDirection);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(futurePosition, 0.1f);
        }
    }
}
