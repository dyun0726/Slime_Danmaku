using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMeteoSpawner : BulletSpawner
{
    public float liveTime = 10f;
    public override void ShootFireBall(){
        Vector2 playerDir = GetPlayerDirection();
        GameObject bulletGO = PoolManager.instance.GetGO(GetBulletName(enemyType));
        FollowBullet bullet = bulletGO.GetComponent<FollowBullet>();
        if (bullet == null)
        {
            Debug.Log("잘못된 탄막 이름 설정");
            return;
        }
        bullet.Damage = enemy.GetDamage();
        bullet.Dir = playerDir;
        bullet.Speed = speed;
        bullet.StartPos = transform.position;
        bullet.deathTime = Time.time + liveTime;
        bulletGO.transform.position = transform.position;
        
        float rotationAngle = GetRotationAngle(playerDir);
        bulletGO.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotationAngle));
    }
}
