using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBulletSpawner : BulletSpawner
{
    public override void ShootFireBall(){
        Vector3 locDiff = PlayerManager.Instance.GetPlayerLoc() - transform.position;
        Vector2 v2dir = new Vector2(locDiff.x, locDiff.y).normalized;

        GameObject bulletGO = PoolManager.instance.GetGO("Fireball_1");
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.Dir = v2dir;
        bullet.Speed = 2f;
        bulletGO.transform.position = transform.position;


    }
}
