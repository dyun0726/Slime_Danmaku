using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBulletSpawner : BulletSpawner
{
    public override void ShootFireBall(){
        Vector2 playerDir = GetPlayerDirection();
        GameObject bulletGO = PoolManager.instance.GetGO(GetBulletName(enemyType));
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.Dir = playerDir;
        bullet.Speed = speed;
        bulletGO.transform.position = transform.position;


    }
}
