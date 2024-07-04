using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LangedController : MonoBehaviour
{

    public void shootBullet(bool facingRight, float magic){
        GameObject playerBulletGO = PoolManager.instance.GetGO("PlayerBullet");
        Bullet bullet = playerBulletGO.GetComponent<Bullet>();
        bullet.Dir = facingRight ? Vector2.right : Vector2.left;
        bullet.Speed = 4f;
        bullet.Damage = magic;
        playerBulletGO.transform.position = transform.position;

    }
}
