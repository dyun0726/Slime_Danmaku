using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBulletSpawner : BulletSpawner
{
    public float angle = 30f;
    public override void ShootFireBall(){
        for (float a = 0; a < 360; a += angle){
            GameObject bulletGO = PoolManager.instance.GetGO(GetBulletName(enemyType));
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            bullet.Dir = Utility.RotateVector2(Vector2.right, a);
            bullet.Speed = speed;
            bulletGO.transform.position = transform.position;
            bulletGO.transform.rotation = Quaternion.Euler(new Vector3(0, 0, a));
        }

    }
}
