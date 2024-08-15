using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlyingEyeEnemy : Enemy
{
    private BulletSpawner[] bulletSpawner;
    private bool inRange = false;
    private float nextShootTime = 0f;
    private float nextDirChangeTime = 0f;
    private float shootCooldown = 4f;
    private float dirChangeCooldown = 3f;
    private float detectionRange = 15f;
    private float speed = 2f;


    protected override void Start()
    {
        base.Start();
        bulletSpawner = GetComponentsInChildren<BulletSpawner>(); 
        dir = Vector2.up;
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

        float distanceToPlayer = Vector2.Distance(transform.position, Player.Instance.GetPlayerLoc());
        inRange = distanceToPlayer < detectionRange;

        // 플레이어가 인식 범위 내에 있을 때
        if (inRange && Time.time > nextShootTime)
        {
            animator.SetTrigger("Attack");
            nextShootTime = Time.time + shootCooldown;
        }
    }

    private void FixedUpdate() {
        // 시간이 멈춰있거나 이 오브젝트가 죽은 상태면 return
        if (!GameManager.Instance.isLive || isDead)
        {
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

    protected override bool IsGroundAhead()
    {
        // 발사한 광선이 땅에 닿는지 판정
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, dir, detectionDistance, groundLayer);

        if (hit.collider == null)
        {
            Debug.DrawRay((Vector2)transform.position, dir * detectionDistance, Color.green);
            return false;
        }
        else
        {
            Debug.DrawRay((Vector2)transform.position, dir * detectionDistance, Color.red);
            return true;
        }
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
    }


    // 랜덤으로 탄막 스포너를 선택해서 탄막 발사
    private void BulletFire(){
        int index = Random.Range(0, bulletSpawner.Length);
        bulletSpawner[index].ShootFireBall();
    }
}
