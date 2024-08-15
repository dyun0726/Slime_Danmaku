using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomEnemy : Enemy
{
    private BulletSpawner bulletSpawner;

    // 행동 관련 변수
    private float detectionRange = 10f;
    private float nextAttackTime = 0f;
    private float shootCooldown = 3f;
    private bool inRange = false;
    private bool animationPlaying = false;
    public float speed = 2.5f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        bulletSpawner = GetComponentInChildren<BulletSpawner>();
        raySpacing = 0.4f;
        upScale = -0.5f;
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

        // 탐지 범위 내이고 공격 쿨타임이 지나면 공격
        if (inRange && Time.time > nextAttackTime){
            animator.SetTrigger("Attack");
            nextAttackTime = Time.time + shootCooldown;
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
        animator.SetBool("isMoving", true);
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * dir);
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

    // 탄막 발사 함수
    private void FireBullet(){
        bulletSpawner.ShootFireBall();
    }

}
