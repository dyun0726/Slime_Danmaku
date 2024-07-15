using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    private void OnTriggerEnter2D(Collider2D other) {
        // Debug.Log("In EnemyBullet:" + other.gameObject.name);
        if (other.gameObject.layer == 10) { // player 일때
            Vector2 dir = (other.transform.position - transform.position).normalized;
            PlayerManager.Instance.TakeDamage(Damage, dir);
            ReleaseObject();
        }
        else if (other.gameObject.layer == 6) { // ground과 부딪힐 때
            ReleaseObject();
        }
    }

}
