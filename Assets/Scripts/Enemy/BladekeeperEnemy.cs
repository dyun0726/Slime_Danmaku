using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladekeeperEnemy : Enemy
{
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
    private Vector2 dir = Vector2.right;
    public float speed = 2f;

    protected override void Start() {
        base.Start();
        bulletSpawner = GetComponentInChildren<BulletSpawner>(); 
    }

    // Update is called once per frame
    private void Update()
    {   
        // 시간이 멈춰있거나 이 오브젝트가 죽은 상태면 return
        if (!GameManager.Instance.isLive || isDead)
        {
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

            // 공격 쿨타임이 지나면
            if (Time.time > nextAttackTime){
                // 일정 거리 안이면 근거리 공격
                if (distanceToPlayer < meleeRange)
                {
                    animator.SetTrigger("Melee");
                    nextAttackTime = Time.time + meleeCooldown;
                }
                // 탐지 범위 안이면 원거리 공격
                else if (distanceToPlayer < detectionRange)
                {
                    animator.SetTrigger("Attack");
                    nextAttackTime = Time.time + shootCooldown;
                }
            }
        }
    }

    private void FixedUpdate() {
        // 시간이 멈춰있거나 이 오브젝트가 죽은 상태면 return
        if (!GameManager.Instance.isLive || isDead)
        {
            return;
        }
        
        if (isStuned){
            return;
        }

        if (canMove && IsGroundAhead()){
            MoveForward();
        } else {
            animator.SetBool("isMoving", false);
        }
    }

    private bool IsGroundAhead()
    {
        // 모든 광선이 땅에 닿아야 땅이 있다고 판정
        Vector2 rayOrigin = (Vector2)transform.position + dir * raySpacing + Vector2.up * upScale;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, detectionDistance, groundLayer);

        // Raycast를 발사하여 땅과의 충돌 여부를 확인
        if (hit.collider == null)
        {
            Debug.DrawRay(rayOrigin, Vector2.down * detectionDistance, Color.red);
            return false;
        }
        else
        {
            Debug.DrawRay(rayOrigin, Vector2.down * detectionDistance, Color.green);
        }

        return true; // 모든 광선이 땅에 닿으면 true 반환
    }

    void MoveForward()
    {
        // 이동 코드 작성
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * dir);
        // transform.Translate(dir * Time.fixedDeltaTime);
        animator.SetBool("isMoving", true);
    }

    // 애니메이션 이벤트 핸들러
    public void OnAnimationEnd()
    {
        canMove = true;
    }

    public void OnAnimationStart()
    {
        canMove = false;
    }

    // follow 탄막 발사 함수
    public void FireFollowBullet(){
        bulletSpawner.ShootFireBall();
    }
}
