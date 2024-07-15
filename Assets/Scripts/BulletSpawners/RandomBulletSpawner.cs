using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBulletSpawner : BulletSpawner
{
    public override void ShootFireBall(){
        for (int i = 0; i < 3; i++){
            GameObject bulletGO = PoolManager.instance.GetGO(GetBulletName(enemyType));
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            bullet.Dir = Utility.RotateVector2(Vector2.right, Random.Range(0, 180));
            bullet.Speed = 3f;
            bulletGO.transform.position = transform.position;
        }

    }
}
