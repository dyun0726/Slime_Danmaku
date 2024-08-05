using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeEnemy : Enemy
{
    private BulletSpawner[] bulletSpawner;

    // 땅 탐지 관련 변수
    private float detectionDistance = 1f; // Raycast로 탐지할 거리
    public LayerMask groundLayer; // 땅 레이어 마스크

    private bool inRange = false;
    private float nextShootTime = 0f;
    private float nextDirChangeTime = 0f;
    private float shootCooldown = 4f;
    private float dirChangeCooldown = 3f;
    private float detectionRange = 15f;
    private float speed = 2f;
    private Vector2 dir = Vector2.up;


    protected override void Start()
    {
        base.Start();
        bulletSpawner = GetComponentsInChildren<BulletSpawner>(); 
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
        Vector3 scale = Vector3.one;
        scale.x = Player.Instance.GetPlayerLoc().x < transform.position.x ? -1 : 1;
        transform.localScale = scale;

        // 방향 조정
        if (Time.time > nextDirChangeTime) {
            changeDir();
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
            float distanceToPlayer = Vector2.Distance(transform.position, Player.Instance.GetPlayerLoc());
            inRange = distanceToPlayer < detectionRange;

            // 플레이어가 인식 범위 내에 있을 때
            if (inRange && Time.time > nextShootTime)
            {
                animator.SetTrigger("Attack");
                nextShootTime = Time.time + shootCooldown;
                // dir = Random.insideUnitCircle.normalized;
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

        if (!IsGroundAhead())
        {
            MoveForward();
        } 
        else 
        {
            changeDir();
        }
        
    }

    private bool IsGroundAhead()
    {
        // 발사한 광선이 땅에 닿는지 판정
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, dir, detectionDistance, groundLayer);

        // Raycast를 발사하여 땅과의 충돌 여부를 확인
        if (hit.collider == null)
        {
            Debug.DrawRay((Vector2)transform.position, dir * detectionDistance, Color.green);
            return false;
        }
        else
        {
            Debug.DrawRay((Vector2)transform.position, dir * detectionDistance, Color.red);
        }

        return true; // 모든 광선이 땅에 닿으면 true 반환
    }

    private void changeDir()
    {
        dir *= -1;
        nextDirChangeTime = Time.time + dirChangeCooldown;
    }


    private void MoveForward()
    {
        // 이동 코드 작성
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * dir);
        // animator.SetBool("isMoving", true);
    }


    // 랜덤으로 탄막 스포너를 선택해서 탄막 발사
    private void BulletFire(){
        int index = Random.Range(0, bulletSpawner.Length);
        bulletSpawner[index].ShootFireBall();
    }
}
