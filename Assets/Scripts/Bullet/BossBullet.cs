using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : Bullet
{
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("In BossBullet:" + other.gameObject.name);
        if (other.gameObject.layer == 10) { // player 일때
            Vector2 dir = (other.transform.position - transform.position).normalized;
            PlayerManager.Instance.TakeDamage(Damage, dir);
            ReleaseObject();
        }
        
    }
}
