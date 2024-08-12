using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarPlayerSpawner : BulletSpawner
{
    private float scale = 1.5f;
    private float rayDistance = 20f;
    public LayerMask groundLayer; // Ground 레이어 마스크
    
    public override void ShootFireBall(){
        Vector3 playerLoc = Player.Instance.GetPlayerLoc();
        RaycastHit2D hit = Physics2D.Raycast(playerLoc, Vector2.down, rayDistance, groundLayer);
        if (hit.collider == null)
        {
            Debug.LogWarning("Ground 레이어를 찾을 수 없습니다");
            return;
        }
        Vector3 groundPos = hit.point;

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
            bulletGO.transform.position = groundPos + scale * i * Vector3.right;
        

            bullet.Ready();
        }
    }
}
