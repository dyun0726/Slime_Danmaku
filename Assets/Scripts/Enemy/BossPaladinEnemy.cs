using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPaladinEnemy : Enemy
{
    private BulletSpawner[] bulletSpawners;

    // 행동 관련 변수
    private float detectionRange = 15f;
    private float nextAttackTime = 0f;
    private float atkCooldown = 4f;
    private bool inRange = false;
    private bool animationPlaying = false;
    public float speed = 3f;
    public List<GameObject> enemyPrefabs; // 적 프리팹 리스트
    private Transform enemySpanwer;
    public GameObject PotionPrefab;

    protected override void Start()
    {
        base.Start();
        bulletSpawners = GetComponentsInChildren<BulletSpawner>(); 
        enemySpanwer = transform.GetChild(1);
        raySpacing = 0.4f;
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
            RandomAttack();
            nextAttackTime = Time.time + atkCooldown;
        }
        
    }

    private void FixedUpdate() {
        // 시간이 멈춰있거나 이 오브젝트가 죽은 상태면 return
        if (!GameManager.Instance.isLive || isDead)
        {
            return;
        }

        if (inRange && !animationPlaying && IsGroundAhead()){
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

    // 랜덤으로 공격하는 함수
    private void RandomAttack(){
        float randomValue = Random.Range(0f, 1f);

        if (randomValue < 0.45f)
        {
            animator.SetTrigger("Melee");
        }
        else if (randomValue < 0.9f)
        {
            animator.SetTrigger("Range");
        }
        else
        {
            animator.SetTrigger("Summon");
        }

    }

    // 플레이어 아래 장판 생성 함수
    private void CreatePillarPlayer()
    {
        bulletSpawners[1].ShootFireBall();
    }

    // 망치 아래 장판 생성 함수
    private void CreatePillarBoss()
    {
        bulletSpawners[0].ShootFireBall();
    }

    // 잡몹 생성 함수
    public void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, enemyPrefabs.Count);
        GameObject newEnemy = Instantiate(enemyPrefabs[randomIndex], enemySpanwer.position, enemySpanwer.rotation);
        // 스폰된 잡몹은 경험치 적음
        newEnemy.GetComponent<Enemy>().exp = 5;
    }

    public override void Die()
    {
       
        DropPotion(transform.position);

        // BossHealthBar bossHealthBar = FindObjectOfType<BossHealthBar>();
        // if (bossHealthBar != null)
        // {
        //     bossHealthBar.healthBar.gameObject.SetActive(false); // 보스 체력바 비활성화
        // }

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
