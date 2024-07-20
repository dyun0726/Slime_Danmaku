using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkEnemy : Enemy
{

    private Rigidbody2D rb;
    private BulletSpawner bulletSpawner;
    private float detectionRange = 10f;
    private float meleeRange = 4f;
    private float nextShootTime = 0f;
    public float shootCooldown = 3f;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        bulletSpawner = GetComponentInChildren<BulletSpawner>(); 
    }

    private void Update() {
        if (!GameManager.Instance.isLive){  // live 체크 함수
            return;
        }

        // 플레이어를 보게 하기 (스케일 조정)
        Vector3 scale = Vector3.one;
        scale.x = PlayerManager.Instance.GetPlayerLoc().x < transform.position.x ? -1 : 1;
        transform.localScale = scale;
        
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

                    }
                    else
                    {
                        animator.SetTrigger("Attack");
                    }
                    nextShootTime = Time.time + shootCooldown;
                }
            }
        }
    }

    // 탄막 발사 함수
    public void BulletFire(){
        bulletSpawner.ShootFireBall();
    }

}
