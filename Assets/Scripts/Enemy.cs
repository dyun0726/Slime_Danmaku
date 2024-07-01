using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float health = 20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // damage만큼의 체력이 깎임
    public void TakeDamage(float damage){
        health -= damage;
        if (health <= 0){
            Destroy(gameObject);
        }
    }

    
}
