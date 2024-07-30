using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardEnemy : Enemy
{
    private BulletSpawner[] circleBulletSpawner;
    private BulletSpawner[] frontBulletSpawner;
    private float nextShootTime = 0f;
    private float shootCooldown = 4f;
    private float detectionRange = 15f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        circleBulletSpawner = transform.GetChild(0).GetComponents<BulletSpawner>();
        frontBulletSpawner = transform.GetChild(1).GetComponents<BulletSpawner>(); 
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
                    int cnt = Random.Range(0, 2);
                    if (cnt == 0)
                    {
                        animator.SetTrigger("CircleAttack");
                    }
                    else
                    {
                        animator.SetTrigger("FrontAttack");
                    }
                    nextShootTime = Time.time + shootCooldown;
                }
            }
        }
    }

    // 랜덤으로 탄막 스포너를 선택해서 탄막 발사
    private void CircleBulletFire(){
        int index = Random.Range(0, circleBulletSpawner.Length);
        circleBulletSpawner[index].ShootFireBall();
    }

    private void FrontBulletFire(){
        int index = Random.Range(0, frontBulletSpawner.Length);
        frontBulletSpawner[index].ShootFireBall();
    }
}
