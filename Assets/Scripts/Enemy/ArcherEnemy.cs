using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherEnemy : Enemy
{
    private Rigidbody2D rb;
    private BulletSpawner[] bulletSpawner;
    private float nextShootTime = 0f;
    private float waitSeconds = 0.6f;
    public float shootCooldown = 3f;
    public float detectionRange = 15f;

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
                    RandomFire();
                    nextShootTime = Time.time + shootCooldown;
                }
            }
        }
    }

    // 랜덤으로 탄막 스포너를 선택해서 탄막 발사
    private void RandomFire(){
        int index = Random.Range(0, bulletSpawner.Length);
        animator.SetTrigger("Attack");
        StartCoroutine(DelayShoot(index));
    }

    // x초 뒤에 탄막 발사되게 하기
    IEnumerator DelayShoot(int index){
        yield return new WaitForSeconds(waitSeconds);
        bulletSpawner[index].ShootFireBall();
    }

}
