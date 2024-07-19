using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlimeEnemy : Enemy
{
    // 기존 변수
    public float detectionRange = 20f;
    public float jumpForce = 4f;
    public float jumpCooldown = 4f;
    public float shootCooldown = 4f;
    public float slowJumpForce = 10f; // 천천히 위로 더 높이 점프하는 힘
    public float groundImpactDamage = 10f; // 땅에 닿았을 때 플레이어에게 줄 데미지
    public float slowJumpCooldown = 10f; // 천천히 점프 패턴 쿨다운
    public float groundImpactDelay = 1f;
    public LayerMask groundLayer;
    public Transform groundCheck;

    private Rigidbody2D rb;
    private BulletSpawner[] bulletSpawner;
    private bool isGrounded;
    private bool isFutureGrounded;
    private float nextJumpTime = 0f;
    private float nextShootTime = 0f;
    private float nextSlowJumpTime = 0f; // 천천히 점프 패턴을 위한 변수
    private Vector2 jumpDirection;
    private bool isSlowJumping = false; // 천천히 점프 중인지 여부
    private bool hasLanded = false; // 착지 여부
    // private float groundImpactTime = 0f; 
    private Player player;
    private bool hasDamaged = false; // 데미지 여부 체크
    private CameraShake cameraShake;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        bulletSpawner = GetComponentsInChildren<BulletSpawner>();
        player = FindObjectOfType<Player>(); // Player 객체 찾기
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    private void Update()
    {
        if (!GameManager.Instance.isLive)
        {  // live 체크 함수
            return;
        }

        // 플레이어를 보게 하기
        spriteRenderer.flipX = player.transform.position.x < transform.position.x;

        if (isAtkReduced)
        {
            atkReductionTimer -= Time.deltaTime;
            if (atkReductionTimer < 0)
            {
                isAtkReduced = false;
                atkReduction = 0;
            }
        }

        // 보스는 스턴이 없음
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        // 플레이어가 인식 범위 내에 있을 때
        if (distanceToPlayer < detectionRange)
        {
            // 땅 체크
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

            if (isGrounded)
            {
                if (!hasLanded) // 착지하지 않았을 때만 처리
                {
                    if (Time.time > nextJumpTime)
                    {
                        if (isSlowJumping)
                        {
                            SlowJumpUp();

                            Debug.Log("dland");
                            hasLanded = true;

                        }
                        else
                        {
                            JumpTowardsPlayer();
                            nextJumpTime = Time.time + jumpCooldown;
                            nextShootTime = Mathf.Max(nextShootTime, Time.time + 1f);
                            hasLanded = true; // 착지 상태로 변경
                        }
                    }
                }
                else
                {
                    if (!hasDamaged&& isGrounded) // 착지 후 데미지 처리가 되지 않았다면
                    {
                        Invoke("GroundImpact",2f);
                        Debug.Log("delat");
                        hasDamaged = true; // 데미지 처리 상태로 변경
                    }

                    if (Time.time > nextSlowJumpTime)
                    {
                        SlowJumpUp();
                        nextSlowJumpTime = Time.time + slowJumpCooldown;
                    }
                }
            }
            else
            {
                hasLanded = false; // 착지 상태 초기화
                hasDamaged = false; // 데미지 상태 초기화
            }

            if (Time.time > nextShootTime)
            {
                RandomFire();
                nextShootTime = Time.time + shootCooldown;
            }
        }
    }

    // 천천히 위로 더 높이 점프
    private void SlowJumpUp()
    {
        rb.velocity = new Vector2(0, slowJumpForce);
        isSlowJumping = true;
        nextJumpTime = Time.time + jumpCooldown;
    }
    private void SetHasLanded()
    {
        hasLanded = true;
    }

    // 땅에 닿았을 때 플레이어가 땅에 접촉해 있으면 데미지
    private void GroundImpact()
    {
        if (player.isGrounded) // Player 스크립트의 isGrounded 변수를 사용
        {
            Vector2 dir = (player.transform.position - transform.position).normalized;
            PlayerManager.Instance.TakeDamage(groundImpactDamage, dir);

            if (cameraShake != null)
            {
                cameraShake.Shake(0.5f, 0.1f); // 흔들림 시간, 세기 설정
            }
        }
    }

    // 점프할 방향 계산 함수
    private Vector2 CalculateJumpDirection()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        return new Vector2(direction.x, 1).normalized;
    }

    // 점프시 도착할 곳 계산
    private Vector2 CalculateFuturePosition(Vector2 jumpDirection)
    {
        float timeToApex = jumpForce / Physics2D.gravity.magnitude; // 최고점까지 도달하는 시간
        float totalJumpTime = timeToApex * 2; // 점프 전체 시간
        float horizontalDistance = jumpDirection.x * jumpForce * totalJumpTime; // 수평 이동 거리

        return new Vector2(groundCheck.position.x + horizontalDistance, groundCheck.position.y);
    }

    // 도착할 곳이 땅인지 계산
    private bool CheckFutureGround()
    {
        jumpDirection = CalculateJumpDirection();
        Vector2 futurePosition = CalculateFuturePosition(jumpDirection);
        return Physics2D.OverlapCircle(futurePosition, 0.1f, groundLayer);
    }

    private void JumpTowardsPlayer()
    {
        rb.velocity = jumpDirection * jumpForce;
    }

    // 랜덤으로 탄막 스포너를 선택해서 탄막 발사
    private void RandomFire()
    {
        int index = Random.Range(0, bulletSpawner.Length);
        bulletSpawner[index].ShootFireBall();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.position, 0.1f);

        if (Application.isPlaying)
        {
            Vector2 jumpDirection = CalculateJumpDirection();
            Vector2 futurePosition = CalculateFuturePosition(jumpDirection);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(futurePosition, 0.1f);
        }
    }
}
