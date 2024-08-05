using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWormEnemy : Enemy
{
    private BulletSpawner bulletSpawner;

    // 땅 탐지 관련 변수
    private float detectionDistance = 1.0f; // Raycast로 탐지할 거리
    private float raySpacing = 0.5f; // 광선 사이의 간격
    public LayerMask groundLayer; // 땅 레이어 마스크
    private float upScale = 0.1f; // 보정을 위한 변수

    // 행동 관련 변수
    private float detectionRange = 10f;
    private float nextAttackTime = 0f;
    private float shootCooldown = 4f;
    private bool canMove = false;
    private bool inRange = false;
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

            if (inRange)
            {
                if (Time.time > nextAttackTime)
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
            animator.SetBool("isMoving", false);
            return;
        }

        if (inRange && canMove && IsGroundAhead()){
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

    private void MoveForward()
    {
        SetReverseDirection();
        // 이동 코드 작성
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * dir);
        // transform.Translate(dir * Time.fixedDeltaTime);
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

    // 정방향으로 방향 세팅
    private void SetDirection(){
        if (Player.Instance.GetPlayerLoc().x < transform.position.x) {
            SetLeft();
        } else if (Player.Instance.GetPlayerLoc().x > transform.position.x) {
            SetRight();
        }
    }

    // 역방향으로 방향 세팅
    private void SetReverseDirection()
    {
        if (Player.Instance.GetPlayerLoc().x < transform.position.x) {
            SetRight();
        } else if (Player.Instance.GetPlayerLoc().x > transform.position.x) {
            SetLeft();
        }
    }


    private void SetLeft(){
        transform.localScale = new Vector3(-1, 1, 1);
        dir = Vector2.left;
    }

    private void SetRight(){
        transform.localScale = Vector3.one;
        dir = Vector2.right;
    }

    private void FireBullet(){
        bulletSpawner.ShootFireBall();
    }
}
