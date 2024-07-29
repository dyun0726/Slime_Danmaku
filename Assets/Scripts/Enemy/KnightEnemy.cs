using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightEnemy : Enemy
{
    private BulletSpawner bulletSpawner;

    // 땅 탐지 관련 변수
    private float detectionDistance = 1.0f; // Raycast로 탐지할 거리
    private float raySpacing = 0.25f; // 광선 사이의 간격
    public LayerMask groundLayer; // 땅 레이어 마스크
    private float upScale = 0.1f; // 보정을 위한 변수

    // 행동 관련 변수
    private float detectionRange = 10f;
    private float meleeRange = 3f;
    private float nextAttackTime = 0f;
    private float shootCooldown = 5f;
    private float meleeCooldown = 5f;
    private bool canMove = false;
    private bool animationPlaying = false;
    private Vector2 dir = Vector2.right;
    public float speed = 2f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        bulletSpawner = GetComponentInChildren<BulletSpawner>(); 
    }

    // Update is called once per frame
    void Update()
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
        } else if (PlayerManager.Instance.GetPlayerLoc().x > transform.position.x) {
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
            // 탐지 범위 내이면 이동
            float distanceToPlayer = Vector2.Distance(transform.position, PlayerManager.Instance.GetPlayerLoc());
            canMove = distanceToPlayer < detectionRange;

            float xDistance= Mathf.Abs(transform.position.x - PlayerManager.Instance.GetPlayerLoc().x);
            // 탐지 범위 내이고 공격 쿨타임이 지나면
            if (canMove && Time.time > nextAttackTime){
                // x좌표 차이에 따라 근거리 공격
                if (xDistance < meleeRange)
                {
                    animator.SetTrigger("Melee");
                    nextAttackTime = Time.time + meleeCooldown;
                }
                // 원거리 공격
                else if (xDistance < detectionRange)
                {
                    animator.SetTrigger("Range");
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

        if (canMove && !animationPlaying && IsGroundAhead()){
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

    // 탄막 발사 함수
    private void FireBullet(){
        bulletSpawner.ShootFireBall();
    }
}
