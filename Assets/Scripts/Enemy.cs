using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float health = 20;
    public int exp = 15; // 죽였을때 주는 경험치
    public float damage = 10; // 몸박 데미지
    public int dotCount = 0; // 남은 도트 카운트 수


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
        if (dotCount > 0){ // 이미 독데미지를 받고 있다면 count를 초기화
            dotCount = 3; 
        } else if (dotCount == 0){
            dotCount = 3;
            StartCoroutine(TakeDotDamage(amount));
        }
        
    }

    // dotCount초 동안 amount만큼 데미지를 줌
    IEnumerator TakeDotDamage(float amount){
        while (dotCount > 0){
            yield return new WaitForSeconds(1f);
            dotCount--;
            TakeDamage(amount);
            Debug.Log("enemy health: " + health);
            
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == 10) { // 플레이어와 충돌하면
            Vector2 dir = (other.transform.position - transform.position).normalized;
            PlayerManager.Instance.TakeDamage(damage, dir);
        }
    }

    
}
