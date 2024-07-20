using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    private Enemy enemy;
    private void Awake() {
        enemy = GetComponentInParent<Enemy>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("meleeAttack: " + other.name);
        if (other.gameObject.layer == 10){
            Vector2 dir = (other.transform.position - transform.position).normalized;
            PlayerManager.Instance.TakeDamage(enemy.damage, dir);
        }
    }
}
