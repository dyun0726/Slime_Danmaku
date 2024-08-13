using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAttack : MonoBehaviour
{
    private Bullet bullet;
    private void Awake() {
        bullet = GetComponentInParent<Bullet>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("meleeAttack: " + other.name);
        if (other.gameObject.layer == 10){
            Vector2 dir = (other.transform.position - transform.position).normalized;
            PlayerManager.Instance.TakeDamage(bullet.Damage, dir);
        }
    }
}
