using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlimeEnemy : Enemy
{
    // 기존 변수
    public float detectionRange = 20f;
    public float jumpForce = 4f;
    public float shootCooldown = 4f; // 공격 쿨다운
    public float slowJumpForce = 10f; // 천천히 위로 더 높이 점프하는 힘
    public float groundImpactDamage = 10f; // 땅에 닿았을 때 플레이어에게 줄 데미지
    public float groundImpactDelay = 1f;
    public Transform groundCheck;
    public GameObject minionPrefab; // 잡몹 프리팹
    public Transform[] spawnPoints1; // 잡몹 스폰 위치 배열
    public Transform[] spawnPoints2;
    public GameObject rotationEffectPrefab; // 회전 이펙트 프리팹
    public float maxhealth=100;
    public float curhealth;

    private bool hasSpawnedAt66 = false; // 66% 체력 시 스폰 여부
    private bool hasSpawnedAt33 = false; // 33% 체력 시 스폰 여부
    private BulletSpawner[] bulletSpawner;
    private bool isGrounded;
    private bool isFutureGrounded;
    private Vector2 jumpDirection;
    private Player player;
    private ParticleSystem groundImpactEffect; // 충격파 효과
    public GameObject laserBeamPrefab; // 레이저빔 프리팹
    public Transform firePoint; // 레이저빔 발사 위치
    public float laserChargeTime = 2f; // 레이저빔 차지 시간
    public float laserDuration = 1f; // 레이저빔 지속 시간
    private GameObject laserBeam; // 현재 활성화된 레이저빔 인스턴스를 추적하는 변수


    public Portal portal;
    public GameObject PotionPrefab;

    protected override void Start()
    {
        base.Start();
        // BossHealthBar bossHealthBar = FindObjectOfType<BossHealthBar>();
        // if (bossHealthBar != null)
        // {
        //     bossHealthBar.healthBar.gameObject.SetActive(true);
        //     bossHealthBar.healthBar.maxValue = maxhealth;
        //     bossHealthBar.healthBar.value = curhealth;
        // }

        bulletSpawner = GetComponentsInChildren<BulletSpawner>();
        player = FindObjectOfType<Player>(); // Player 객체 찾기
        groundImpactEffect = GetComponentInChildren<ParticleSystem>();
        groundImpactEffect.Stop();
        Vector3 firePointPosition = firePoint.position;
        firePointPosition.x = firePointPosition.x - 15f; // firePoint의 x 좌표를 -20으로 조정
        firePointPosition.y = firePointPosition.y + 0.5f;
        firePoint.position = firePointPosition;
        maxhealth = health;
        curhealth = health;

        StartCoroutine(BossActionCycle());
    }

    private void Update()
    {
        curhealth = health;
        // 시간이 멈춰있거나 이 오브젝트가 죽은 상태면 return
        if (!GameManager.Instance.isLive || isDead)
        {
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

        // 체력 체크 및 잡몹 스폰
        CheckHealthAndSpawnMinions();
    }




    // 보스 행동 주기를 코루틴으로 설정
    private IEnumerator BossActionCycle()
    {
        while (true)
        {
            // 랜덤 공격 두 번 실행
            for (int i = 0; i < 2; i++)
            {
                RandomFire();
                yield return new WaitForSeconds(shootCooldown); // 공격 쿨다운 시간 대기
            }

            // 천천히 점프 동작 실행
            SlowJumpUp();
            
            Invoke("GroundImpact", 2f);
            Debug.Log("delat");
            yield return new WaitForSeconds(shootCooldown); // 점프 후 대기 시간
        }
    }

    // 천천히 위로 더 높이 점프
    private void SlowJumpUp()
    {
        rb.velocity = new Vector2(0, slowJumpForce);
    }


    // 땅에 닿았을 때 플레이어가 땅에 접촉해 있으면 데미지
    private void GroundImpact()
    {
        if (player.isGrounded) // Player 스크립트의 isGrounded 변수를 사용
        {
            Vector2 dir = (player.transform.position - transform.position).normalized;
            PlayerManager.Instance.TakeDamage(groundImpactDamage, dir);

            if (groundImpactEffect != null)
            {
                groundImpactEffect.Play(); // 파티클 시스템 재생
            }
        }
    }

    // 체력 체크 및 잡몹 스폰 함수
    private void CheckHealthAndSpawnMinions()
    {
        if (!hasSpawnedAt66 && curhealth <= maxhealth * 0.66f)
        {
            Debug.Log("spawn");
            SpawnMinions1();
            hasSpawnedAt66 = true;
        }

        if (!hasSpawnedAt33 && curhealth <= maxhealth * 0.33f)
        {
            SpawnMinions2();
            hasSpawnedAt33 = true;
        }
    }

    // 잡몹 스폰 함수
    private void SpawnMinions1()
    {
        foreach (Transform spawnPoint in spawnPoints1)
        {
            Instantiate(minionPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
    private void SpawnMinions2()
    {
        if (spawnPoints2.Length >= 3)
        {
            // 첫 번째 스폰 지점에 소환
            Instantiate(minionPrefab, spawnPoints2[0].position, spawnPoints2[0].rotation);
            // 두 번째 스폰 지점에 소환
            Instantiate(minionPrefab, spawnPoints2[1].position, spawnPoints2[1].rotation);

            Instantiate(minionPrefab, spawnPoints2[2].position, spawnPoints2[2].rotation);
        }
        else
        {
            Debug.LogWarning("SpawnPoints2 배열에 스폰 지점이 2개 이상 필요합니다.");
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
        float rand = Random.Range(0f, 1f);

        if (rand < 0.5f)
        {
            // 50% 확률로 탄막 발사
            int index = Random.Range(0, bulletSpawner.Length);
            bulletSpawner[index].ShootFireBall();
        }
        else
        {
            // 50% 확률로 레이저빔 발사
            StartCoroutine(FireLaserBeam());
        }
    }

    // 레이저빔 발사
    private IEnumerator FireLaserBeam()
    {
        // 레이저 차지 효과 (2초)
        //        yield return new WaitForSeconds(laserChargeTime);

        // 레이저 차지 효과 (2초) 동안 회전 이펙트 실행
        GameObject rotationEffect = Instantiate(rotationEffectPrefab, transform.position, Quaternion.identity);
        rotationEffect.transform.SetParent(transform); // 보스의 위치에 고정

        float elapsed = 0f;
        while (elapsed < laserChargeTime)
        {
            rotationEffect.transform.Rotate(Vector3.forward, 360 * Time.deltaTime); // 1초에 한 바퀴 회전
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(rotationEffect);


        // 레이저빔 생성
        laserBeam = Instantiate(laserBeamPrefab, firePoint.position, firePoint.rotation);

        // 보스의 현재 바라보고 있는 방향에 따라 레이저빔의 방향 설정
        if (spriteRenderer.flipX)
        {
            laserBeam.transform.localScale = new Vector2(-1, 1);
        }
        else
        {
            laserBeam.transform.localScale = new Vector2(1, 1);
        }

        // 레이저빔 크기 조정
        laserBeam.transform.localScale = new Vector2(15, 4); // 원하는 크기로 조정

        // 디버그 로그 추가
        Debug.Log("레이저빔 생성됨: " + laserBeam.transform.position);

        // 레이저빔 지속 시간
        yield return new WaitForSeconds(laserDuration);

        // 레이저빔 제거
        Destroy(laserBeam);
        laserBeam = null;
       yield return new WaitForSeconds(laserChargeTime);
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

    public override void Die()
    {
        if (laserBeam != null)
        {
            Destroy(laserBeam);
            laserBeam = null;
        }

        DropPotion(transform.position);

        if (portal != null)
        {
            portal.GetComponent<Portal>().ActivatePortal();
            BossHealthBar bossHealthBar = FindObjectOfType<BossHealthBar>();
            if (bossHealthBar != null)
            {
                bossHealthBar.healthBar.gameObject.SetActive(false); // 보스 체력바 비활성화
            }
        }

        base.Die(); // 부모 클래스의 Die() 메서드 호출
    }

    private void DropPotion(Vector3 dropPosition)
    {
        float dropChance = Random.Range(0f, 100f);
        float totalDropChance = potionDropChance * (100f + PlayerManager.Instance.dropbonus) / 100f;
        if (dropChance < totalDropChance)
        {
            Instantiate(PotionPrefab, dropPosition, Quaternion.identity);
        }
    }
}
