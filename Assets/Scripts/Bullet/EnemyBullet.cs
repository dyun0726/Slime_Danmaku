using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    private void OnTriggerEnter2D(Collider2D other) {
        // layer 8: Player Bullet, 7: Enemy bullet
        // if (gameObject.layer == 7){ // 적 탄환 일때
            if (other.gameObject.layer == 10) { // player 일때

                Vector2 dir = (other.transform.position - transform.position).normalized;
                PlayerManager.Instance.TakeDamage(Damage, dir);
                ReleaseObject();

            }
        // }
    }
}
