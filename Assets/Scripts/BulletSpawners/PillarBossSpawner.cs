using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarBossSpawner : BulletSpawner
{
    public override void ShootFireBall(){
        GameObject bulletGO = PoolManager.instance.GetGO(GetBulletName(enemyType));
        PillarBullet bullet = bulletGO.GetComponent<PillarBullet>();
        bullet.Damage = enemy.GetDamage();
        bullet.StartPos = transform.position;
        bullet.isFirst = true;
        bullet.left = true;
        bullet.right = true;
        bullet.count = 0;
        bulletGO.transform.position = transform.position;
        

        bullet.Ready();
    }
}
