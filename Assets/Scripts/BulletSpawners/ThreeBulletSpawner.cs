using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeBulletSpawner : BulletSpawner
{
    public float angle = 20f;
    public override void ShootFireBall(){
        Vector2 playerDir = GetPlayerDirection();
        float rotationAngle = GetRotationAngle(playerDir);

        for (int index = -1; index <= 1; index++){
            GameObject bulletGO = PoolManager.instance.GetGO(GetBulletName(enemyType));
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            // -20, 0, 20도 회전 구하기
            Vector2 dir = Utility.RotateVector2(playerDir, index * angle);
            bullet.Dir =  dir;
            bullet.Speed = speed;
            bullet.StartPos = transform.position;
            bulletGO.transform.position = transform.position;
            bulletGO.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotationAngle + index * angle));
        }
    }
}
