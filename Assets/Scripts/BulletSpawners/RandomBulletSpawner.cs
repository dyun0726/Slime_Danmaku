using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBulletSpawner : BulletSpawner
{
    public int bulletCount = 3;
    public override void ShootFireBall(){
        for (int i = 0; i < bulletCount; i++){
            GameObject bulletGO = PoolManager.instance.GetGO(GetBulletName(enemyType));
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            int angle = Random.Range(0, 180);
            bullet.Dir = Utility.RotateVector2(Vector2.right, angle);
            bullet.Speed = 3f;
            bullet.StartPos = transform.position;
            bulletGO.transform.position = transform.position;
            bulletGO.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

    }
}
