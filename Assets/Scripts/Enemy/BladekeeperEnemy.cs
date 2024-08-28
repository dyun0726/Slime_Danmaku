using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladekeeperEnemy : Enemy
{
    private BulletSpawner bulletSpawner;

    // 행동 관련 변수
    private float detectionRange = 10f;
    private float meleeRange = 5f;
    private float nextAttackTime = 0f;
    private float shootCooldown = 5f;
    private float meleeCooldown = 5f;
    private bool canMove = false;
    public float speed = 2f;

    protected override void Start() {
        base.Start();
        bulletSpawner = GetComponentInChildren<BulletSpawner>(); 
        raySpacing = 0.25f; // 광선 사이의 간격
        upScale = 0.1f; // 보정을 위한 변수
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

        float distanceToPlayer = Vector2.Distance(transform.position, Player.Instance.GetPlayerLoc());
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

    private void FixedUpdate() {
        // 시간이 멈춰있거나 이 오브젝트가 죽은 상태면 return
        if (!GameManager.Instance.isLive || isDead)
        {
            return;
        }

        // 플레이어와 x 좌표가 0.1 이상 차이 날 경우만 이동
        if (canMove && IsGroundAhead() && Mathf.Abs(Player.Instance.GetPlayerLoc().x - transform.position.x) > 0.1f){
            MoveForward();
        } else {
            animator.SetBool("isMoving", false);
        }
    }

    private void MoveForward()
    {
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * dir);
        animator.SetBool("isMoving", true);
    }

    // 애니메이션 이벤트 핸들러
    private void OnAnimationEnd()
    {
        canMove = true;
    }

    private void OnAnimationStart()
    {
        canMove = false;
    }

    // follow 탄막 발사 함수
    private void FireFollowBullet(){
        bulletSpawner.ShootFireBall();
    }
}
