using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladekeeperEnemy : Enemy
{
    private Rigidbody2D rb;
    private BulletSpawner bulletSpawner;

    // 땅 탐지 관련 변수
    private float detectionDistance = 1.0f; // Raycast로 탐지할 거리
    private float raySpacing = 0.2f; // 광선 사이의 간격
    public LayerMask groundLayer; // 땅 레이어 마스크

    // 행동 관련 변수
    private float detectionRange = 10f;
    private float meleeRange = 5f;
    private float nextAttackTime = 0f;
    private float shootCooldown = 5f;
    private float meleeCooldown = 5f;
    private bool canMove = false;
    private Vector2 dir = Vector2.right;

    protected override void Start() {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        bulletSpawner = GetComponentInChildren<BulletSpawner>(); 
    }

    // Update is called once per frame
    void Update()
    {   
        // live 체크
        if (!GameManager.Instance.isLive){  
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
        }
        
    }
}
