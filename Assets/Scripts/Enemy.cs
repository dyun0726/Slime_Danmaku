using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float health = 20;
    public int exp = 15; // 죽였을때 주는 경험치
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // damage만큼의 체력이 깎임
    public void TakeDamage(float damage){
        health -= damage;
        if (health <= 0){
            Destroy(gameObject);
            PlayerManager.Instance.IncreaseExp(exp);
        }
    }

    
}
