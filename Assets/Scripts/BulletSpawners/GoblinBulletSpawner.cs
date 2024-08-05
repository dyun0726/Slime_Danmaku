using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinBulletSpawner : BulletSpawner
{
    public override void ShootFireBall(){
        Vector2 playerDir = GetPlayerDirection().x > 0 ?  Vector2.one : new Vector2(-1, 1);
        GameObject bulletGO = PoolManager.instance.GetGO(GetBulletName(enemyType));
        GoblinBullet bullet = bulletGO.GetComponent<GoblinBullet>();

        bullet.Damage = enemy.GetDamage();
        bullet.Dir = playerDir;
        bullet.Speed = speed;
        bullet.StartPos = transform.position;
        bulletGO.transform.position = transform.position;
        bullet.ExplodeTime = 3f;
        bullet.isSetted = true;

        bullet.ThrowBomb();
    }
}
