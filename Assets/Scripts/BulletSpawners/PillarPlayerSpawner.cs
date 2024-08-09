using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarPlayerSpawner : BulletSpawner
{
    private float scale = 1.5f;
    private float downScale = 0.3f;
    public override void ShootFireBall(){
        Vector3 playerLoc = Player.Instance.GetPlayerLoc();
        for (int i = -1; i < 2; i++){
            GameObject bulletGO = PoolManager.instance.GetGO(GetBulletName(enemyType));
            PillarBullet bullet = bulletGO.GetComponent<PillarBullet>();
            bullet.Damage = enemy.GetDamage();
            bullet.StartPos = transform.position;
            bullet.isFirst = true;
            bullet.left = false;
            bullet.right = false;
            bullet.count = 5;
            bullet.waitingTime = 1f;
            bulletGO.transform.position = playerLoc + scale * i * Vector3.right + downScale * Vector3.down;
        

            bullet.Ready();
        }
    }
}
