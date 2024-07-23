using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkEnemy : Enemy
{

    private Rigidbody2D rb;
    private BulletSpawner[] bulletSpawner;
    private float detectionRange = 10f;
    private float meleeRange = 4f;
    private float nextShootTime = 0f;
    private float shootCooldown = 5f;
    private float meleeCooldown = 8f;
    private bool canMove = false;
    private Vector2 dir = Vector2.right;

    // 땅 탐지 관련 변수
    private float detectionDistance = 1.0f; // Raycast로 탐지할 거리
    private float raySpacing = 0.2f; // 광선 사이의 간격
    public LayerMask groundLayer; // 땅 레이어 마스크
    // public float speed = 2f;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        bulletSpawner = GetComponentsInChildren<BulletSpawner>(); 
    }

    private void Update() {
        if (!GameManager.Instance.isLive){  // live 체크 함수
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
    }

    private void FixedUpdate() {
        if (isStuned){
            return;
        }

        if (canMove && IsGroundAhead()){
            MoveForward();
        } else {
            animator.SetBool("isMoving", false);
        }

    }

    bool IsGroundAhead()
    {
        // 모든 광선이 땅에 닿아야 땅이 있다고 판정
        Vector2 rayOrigin = (Vector2)transform.position + dir * raySpacing;
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
        rb.MovePosition(rb.position + dir * Time.fixedDeltaTime);
        // transform.Translate(dir * Time.fixedDeltaTime);
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
