using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaulerEnemy : Enemy
{
    // 땅 탐지 관련 변수
    private float detectionDistance = 1.0f; // Raycast로 탐지할 거리
    private float raySpacing = 0.25f; // 광선 사이의 간격
    public LayerMask groundLayer; // 땅 레이어 마스크
    private float upScale = 0.1f; // 보정을 위한 변수

    // 행동 관련 변수
    private float detectionRange = 10f;
    private float meleeRange = 2.5f;
    private float spMeleeRange = 5f;
    private float nextAttackTime = 0f;
    private float meleeCooldown = 3f;
    private bool canMove = false;
    private bool animationPlaying = false;
    private int atkCount = 1;
    private Vector2 dir = Vector2.right;
    public float speed = 2f;
    protected override void Start()
    {
        base.Start();
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
            // 탐지 범위 내이면 이동 (위치 조정)
            float distanceToPlayer = Vector2.Distance(transform.position + Vector3.up, PlayerManager.Instance.GetPlayerLoc());
            canMove = distanceToPlayer < detectionRange;

            // 공격 쿨타임이 지나고
            if (Time.time > nextAttackTime){
                // 3타에 공격 범위 안이면
                if (atkCount % 3 == 0) 
                {
                    if (distanceToPlayer < spMeleeRange)
                    {
                        animator.SetTrigger("spMelee");
                        nextAttackTime = Time.time + meleeCooldown;
                        atkCount = 1;
                    }
                }
                // 일반 공격 범위 안이면
                else 
                {
                    if (distanceToPlayer < meleeRange)
                    {
                        animator.SetTrigger("Melee");
                        nextAttackTime = Time.time + meleeCooldown;
                        atkCount += 1;
                    }
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
}
