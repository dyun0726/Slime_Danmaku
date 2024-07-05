using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LangedController : MonoBehaviour
{

    public void shootBullet(bool facingRight){
        GameObject playerBulletGO = PoolManager.instance.GetGO("PlayerBullet");
        Bullet bullet = playerBulletGO.GetComponent<Bullet>();
        bullet.Dir = facingRight ? Vector2.right : Vector2.left;
        bullet.Speed = PlayerManager.Instance.bulletSpeed;
        bullet.Range = PlayerManager.Instance.bulletRange;
        bullet.PassCount = PlayerManager.Instance.bulletPass;
        bullet.LifeSteel = PlayerManager.Instance.lifeSteel;
        bullet.StartPos = new Vector2(transform.position.x, transform.position.y);
        bullet.Damage = PlayerManager.Instance.magic;
        playerBulletGO.transform.position = transform.position;

    }
}
