using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float health = 20;
    public int exp = 15; // 죽였을때 주는 경험치
    public float damage = 10; // 몸박 데미지
    public float armor = 5; // 방어력

    // 치명타 데미지 관련 변수
    public float crirate;
    public float criticalDamage;

    //즉사 관련 변수
    public float luckyshot; // 즉사 확률 (0에서 100 사이의 값)
    public bool isBoss; // 보스 여부 (보스는 즉사하지 않음)

    // 도트 데미지 관련 변수
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

        crirate = PlayerManager.Instance.crirate;
        criticalDamage = PlayerManager.Instance.criticalDamage;

        if (!GameManager.Instance.isLive){  // live 체크 함수
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
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == 10) { // 플레이어와 충돌하면
            Vector2 dir = (other.transform.position - transform.position).normalized;
            PlayerManager.Instance.TakeDamage(damage, dir);
        }
    }

    // damage만큼의 체력이 깎임, armorPt: 방어력 관통 수치, armorPtPercent: 방어력 관통 퍼센트
    public void TakeDamage(float damage, float armorPt, float armorPtPercent){
        // 방어력 계산식: 관통 수치 뺀 후 퍼센트 적용
        float calArmor = (armor - armorPt) * (1f - armorPtPercent/100f);
        calArmor = Mathf.Max(calArmor, 0); // calArmor가 0보다 작지 않도록 설정
        float finalDamage = damage - calArmor;
        Debug.Log(damage);
        finalDamage = Mathf.Max(finalDamage, 0); // finalDamage가 0보다 작지 않도록 설정


        // 체력 감소
        health -= finalDamage;

        // 즉사 또는 체력 감소 후 사망 여부 확인
        if (health <= 0)
        {
            Die();
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
            TakeDamage(dotDamge, 0f, 1f);
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
    
    // 사망 함수
    public void Die(){
        Destroy(gameObject);
        PlayerManager.Instance.IncreaseExp(exp);
    }


    
}
