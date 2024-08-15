using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkEnemy : Enemy
{

    private BulletSpawner[] bulletSpawner;
    private float detectionRange = 10f;
    private float meleeRange = 4f;
    private float nextShootTime = 0f;
    private float shootCooldown = 5f;
    private float meleeCooldown = 8f;
    private bool canMove = false;
    public float speed = 2f;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        bulletSpawner = GetComponentsInChildren<BulletSpawner>(); 
        upScale = 0f;
        raySpacing = 0.2f;

    }

    private void Update() {
        // 시간이 멈춰있거나 이 오브젝트가 죽은 상태면 return
        if (!GameManager.Instance.isLive || isDead)
        {
            return;
        }

        // 플레이어를 보게 하기 (스케일 조정)
        if (Player.Instance.GetPlayerLoc().x < transform.position.x) {
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

        float distanceToPlayer = Vector2.Distance(transform.position, Player.Instance.GetPlayerLoc());
        // 플레이어가 인식 범위 내에 있을 때
        if (distanceToPlayer < detectionRange)
        {
            if (Time.time > nextShootTime)
            {
                // 근접 공격 범위면
                if (distanceToPlayer < meleeRange)
                {
                    animator.SetTrigger("Melee");
                    nextShootTime = Time.time + meleeCooldown;
                }
                else // 근접 공격 범위보다 멀면
                {
                    // 원거리 공격 후
                    animator.SetTrigger("Attack");
                    nextShootTime = Time.time + shootCooldown;
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

        if (canMove && IsGroundAhead()){
            MoveForward();
        } else {
            animator.SetBool("isMoving", false);
        }

    }

    void MoveForward()
    {
        // 이동 코드 작성
        rb.MovePosition(rb.position + dir * Time.fixedDeltaTime);
        animator.SetBool("isMoving", true);
    }

    // follow 탄막 발사 함수
    public void FireFollowBullet(){
        bulletSpawner[0].ShootFireBall();
    }

    // 일반 탄막 발사 함수
    public void FireNormalBullet(){
        bulletSpawner[1].ShootFireBall();
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

}
