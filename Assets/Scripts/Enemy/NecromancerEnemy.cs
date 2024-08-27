using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerEnemy : Enemy
{
    private BulletSpawner[] bulletSpawners;

    // 강화 오브젝트와 효과 관련 변수
    public Transform attackBoostObject;
    public Transform armorBoostObject;
    public Transform speedBoostObject;  
    public float boostRange = 6f;       
    public float attackMultiplier = 1.5f;
    public float armorMultiplier = 1.5f;
    public float speedMultiplier = 1.5f; 

    private bool isAttackBoosted = false;
    private bool isArmorBoosted = false;
    private bool isSpeedBoosted = false;

    private float originalDamage; 
    private float originalArmor; 
    private float originalSpeed;

    // 기타 기존 변수들...

    public float maxhealth = 100;
    public float curhealth;


    // 행동 관련 변수
    private float detectionRange = 20f;
    private float nextAttackTime = 0f;
    private float atkCooldown = 4f;
    private bool inRange = false;
    private bool animationPlaying = false;
    public float speed = 2f;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        bulletSpawners = GetComponentsInChildren<BulletSpawner>(); 
        detectionDistance = 1.5f;
        upScale = -0.2f;


        originalDamage = damage;
        originalArmor = armor;
        originalSpeed = speed;

        maxhealth = health;
        curhealth = health;

    }

    // Update is called once per frame
    private void Update()
    {
        maxhealth = health;
        curhealth = health;

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

        CheckForBoosts();

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

        if ( inRange && !animationPlaying && !IsGroundAhead()){
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

    private void SetNextAttackTime()
    {
        nextAttackTime = Time.time + atkCooldown;
    }

    // 공격 행동 결정 함수
    private void Action(){
        float randomValue = Random.Range(0f, 1f);
        if (randomValue < 0.4f)
        {
            animator.SetTrigger("AttackCross");
        }
        else if (randomValue < 0.8f)
        {
            animator.SetTrigger("AttackFollow");
        }
        else
        {
            animator.SetTrigger("AttackCircle");
        }
        
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

    protected override bool IsGroundAhead()
    {
        // Raycast를 발사하여 땅과의 충돌 여부를 확인
        Vector2 rayOrigin = (Vector2)transform.position + Vector2.up * upScale;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, dir, detectionDistance, groundLayer);

        if (hit.collider == null)
        {
            Debug.DrawRay(rayOrigin, dir * detectionDistance, Color.red);
        }
        else
        {
            Debug.DrawRay(rayOrigin, dir * detectionDistance, Color.green);
        }

        return hit.collider != null;
    }

    private void CheckForBoosts()
    {
       
        if (attackBoostObject != null)
        {
            float distanceToAttackBoost = Vector2.Distance(transform.position, attackBoostObject.position);
            if (distanceToAttackBoost <= boostRange && !isAttackBoosted)
            {
                isAttackBoosted = true;
                damage *= attackMultiplier;
                Debug.Log("attackboost");
            }
            else if (distanceToAttackBoost > boostRange && isAttackBoosted)
            {
                isAttackBoosted = false;
                damage = originalDamage;
            }
        }

        if (armorBoostObject != null)
        {
            float distanceToArmorBoost = Vector2.Distance(transform.position, armorBoostObject.position);
            if (distanceToArmorBoost <= boostRange && !isArmorBoosted)
            {
                isArmorBoosted = true;
                armor *= armorMultiplier;
                Debug.Log("armorboost");
            }
            else if (distanceToArmorBoost > boostRange && isArmorBoosted)
            {
                isArmorBoosted = false;
                armor = originalArmor;
            }
        }

        if (speedBoostObject != null)
        {
            float distanceToSpeedBoost = Vector2.Distance(transform.position, speedBoostObject.position);
            if (distanceToSpeedBoost <= boostRange && !isSpeedBoosted)
            {
                isSpeedBoosted = true;
                speed *= speedMultiplier;
                Debug.Log("speedboost");
            }
            else if (distanceToSpeedBoost > boostRange && isSpeedBoosted)
            {
                isSpeedBoosted = false;
                speed = originalSpeed;
            }
        }
    }

    private void FireCircle()
    {
        bulletSpawners[0].ShootFireBall();
    }

    private void FireMeteo()
    {
        bulletSpawners[1].ShootFireBall();
    }

    private void FireFollowMeteo()
    {
        bulletSpawners[2].ShootFireBall();
    }



}
