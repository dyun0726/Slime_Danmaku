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
        bullet.StartPos = transform.position;
        bulletGO.transform.position = transform.position;
        

        float rotationAngle = GetRotationAngle(playerDir);
        bulletGO.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotationAngle));
    }
}
