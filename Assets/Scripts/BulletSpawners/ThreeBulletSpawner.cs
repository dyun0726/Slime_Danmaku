using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeBulletSpawner : BulletSpawner
{
    public float angle = 20f;
    public override void ShootFireBall(){
        Vector2 playerDir = GetPlayerDirection();
        GameObject topBulletGO = PoolManager.instance.GetGO(GetBulletName(enemyType));
        GameObject midBulletGO = PoolManager.instance.GetGO(GetBulletName(enemyType));
        GameObject bottomBulletGO = PoolManager.instance.GetGO(GetBulletName(enemyType));


        Bullet midBullet = midBulletGO.GetComponent<Bullet>();
        midBullet.Dir =  playerDir;
        midBullet.Speed = speed;

        Bullet topBullet = topBulletGO.GetComponent<Bullet>();
        topBullet.Dir = Utility.RotateVector2(playerDir, angle);
        topBullet.Speed = speed;

        Bullet bottomBullet = bottomBulletGO.GetComponent<Bullet>();
        bottomBullet.Dir = Utility.RotateVector2(playerDir, -angle);
        bottomBullet.Speed = speed;


        topBulletGO.transform.position = transform.position;
        midBulletGO.transform.position = transform.position;
        bottomBulletGO.transform.position = transform.position;
    }
}
