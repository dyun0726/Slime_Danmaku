using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float health = 20;
    public int exp = 15; // 죽였을때 주는 경험치
    public float damage = 10; // 몸박 데미지
    public int dotCount = 0; // 남은 도트 카운트 수
    public float dotDamge; 

    // 공격 감소 디버프 관련 변수
    public bool isAtkReduced = false;
    public float atkReduction = 0;
    private float atkReductionTimer;

    // 스턴 설정 변수
    public bool isStuned = false;
    private float stunTimer;



    private void Update() {
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
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == 10) { // 플레이어와 충돌하면
            Vector2 dir = (other.transform.position - transform.position).normalized;
            PlayerManager.Instance.TakeDamage(damage, dir);
        }
    }

    // damage만큼의 체력이 깎임
    public void TakeDamage(float damage){
        health -= damage;
        if (health <= 0){
            Destroy(gameObject);
            PlayerManager.Instance.IncreaseExp(exp);
        }
    }

    // dotDamage를 주는 함수
    public void SetDotDamage(float amount){
        dotDamge = amount;
        if (dotCount > 0){ // 이미 독데미지를 받고 있다면 count를 초기화
            dotCount = 3;
        } else if (dotCount == 0){
            dotCount = 3;
            StartCoroutine(TakeDotDamage());
        }
        
    }

    // dotCount초 동안 amount만큼 데미지를 줌
    IEnumerator TakeDotDamage(){
        while (dotCount > 0){
            yield return new WaitForSeconds(1f);
            dotCount--;
            TakeDamage(dotDamge);
            Debug.Log("enemy health: " + health);
            
        }
    }

    // 공격력 감소 디버프 함수
    public void SetAttackReduce(float time, float amount){
        isAtkReduced = true;
        atkReductionTimer = time;
        atkReduction = amount;
    }

    // 스턴 설정 함수
    public void SetStun(float time){
        isStuned = true;
        stunTimer = time;

    }


    
}
