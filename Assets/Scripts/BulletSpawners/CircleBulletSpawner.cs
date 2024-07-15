using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBulletSpawner : BulletSpawner
{
    public override void ShootFireBall(){
        for (int a = 0; a < 360; a += 30){
            GameObject bulletGO = PoolManager.instance.GetGO("Fireball_1");
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            bullet.Dir = Utility.RotateVector2(Vector2.left, a);
            bullet.Speed = 3f;
            bulletGO.transform.position = transform.position;
        }

    }
}
