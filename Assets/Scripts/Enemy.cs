using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float health = 20;
    public int exp = 15; // 죽였을때 주는 경험치
    public float damage = 10; // 몸박 데미지

    // damage만큼의 체력이 깎임
    public void TakeDamage(float damage){
        health -= damage;
        if (health <= 0){
            Destroy(gameObject);
            PlayerManager.Instance.IncreaseExp(exp);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == 10) { // 플레이어와 충돌하면
            Vector2 dir = (other.transform.position - transform.position).normalized;
            PlayerManager.Instance.TakeDamage(damage, dir);
        }
    }

    
}
