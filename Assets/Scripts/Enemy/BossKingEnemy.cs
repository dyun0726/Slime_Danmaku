using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKingEnemy : Enemy
{
    private BulletSpawner[] bulletSpawners;

    // 행동 관련 변수
    private float detectionRange = 20f;
    private float nextAttackTime = 0f;
    private float atkCooldown = 4f;
    private bool inRange = false;
    public bool animationPlaying = false;
    private float xRange = 7f;
    public float speed = 3f;


    public GameObject PotionPrefab;
    protected override void Start()
    {
        base.Start();
        bulletSpawners = GetComponentsInChildren<BulletSpawner>(); 
        detectionDistance = 1.0f;
        raySpacing = 0.4f;
        upScale = -0.5f;
    }

    private void Update()
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
        } else if (Player.Instance.GetPlayerLoc().x > transform.position.x) {
            transform.localScale = Vector3.one;
            dir = Vector2.right;
        }

        if (isAtkReduced){
            atkReductionTimer -= Time.deltaTime;
            if (atkReductionTimer < 0){
                isAtkReduced = false;
                atkReduction = 0;
            }
        }

        // 탐지 범위 내이면 이동
        float distanceToPlayer = Vector2.Distance(transform.position, Player.Instance.GetPlayerLoc());
        inRange = distanceToPlayer < detectionRange;

        // 탐지 범위 내이고 공격 쿨타임이 지나면
        if (inRange && Time.time > nextAttackTime){
            Action();
            SetNextAttackTime();
        }
        
    }

    private void FixedUpdate() {
        // 시간이 멈춰있거나 이 오브젝트가 죽은 상태면 return
        if (!GameManager.Instance.isLive || isDead)
        {
            return;
        }

        if ( inRange && !animationPlaying && IsGroundAhead()){
            MoveForward();
        } else {
            animator.SetBool("isMoving", false);
        }
        
    }

    private void MoveForward()
    {
        // 이동 코드 작성
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * dir);
        animator.SetBool("isMoving", true);
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

    private void SetNextAttackTime()
    {
        nextAttackTime = Time.time + atkCooldown;
    }

    // 행동 결정 함수 (텔레포트 및 공격)
    private void Action(){
        float tpValue = Random.Range(0f, 1f);
        if (tpValue < 0.5f)
        {
            animator.SetTrigger("Teleport");
        }
        else
        {
            RandomAttack();
        }
        
    }

    private void RandomAttack()
    {
        float randomValue = Random.Range(0f, 1f);
        if (randomValue < 0.33f)
        {
            animator.SetTrigger("AtkPlayer");
        }
        else if (randomValue < 0.67f)
        {
            animator.SetTrigger("AtkAll");
        }
        else
        {
            animator.SetTrigger("AtkSwing");
        }
        SetNextAttackTime();
        
    }
    // 보스 아래 장판 생성 함수
    private void CreatePillarBoss()
    {
        bulletSpawners[0].ShootFireBall();
    }

    // 플레이어 아래 장판 생성 함수
    private void CreatePillarPlayer()
    {
        bulletSpawners[1].ShootFireBall();
    }

    // 원 탄막 생성 함수
    private void CreateCircleFire()
    {
        bulletSpawners[2].ShootFireBall();
    }

    // 텔레포트 함수
    private void Teleport()
    {
        
        Vector3 newPos = transform.position + Vector3.right * Random.Range(-xRange, xRange);
        // Debug.Log(IsGround(newPos));
        while (!IsGround(newPos))
        {
            newPos = transform.position + Vector3.right * Random.Range(-xRange, xRange);
        }

        transform.position = newPos;
    }

    // 해당 위치가 땅 위인지 확인하는 함수
    private bool IsGround(Vector2 pos)
    {
        // 모든 광선이 땅에 닿아야 땅이 있다고 판정
        Vector2 rayOrigin = pos + Vector2.up * upScale ;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, detectionDistance, groundLayer);

        // Raycast를 발사하여 땅과의 충돌 여부를 확인
        if (hit.collider == null)
        {
            return false;
        }
        return true;
    }

    public override void Die()
    {

        DropPotion(transform.position);
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
