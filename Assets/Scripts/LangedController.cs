using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LangedController : MonoBehaviour
{

    public void shootBullet(bool filpX){
        GameObject playerBulletGO = PoolManager.instance.GetGO("PlayerBullet");
        Bullet bullet = playerBulletGO.GetComponent<Bullet>();
        bullet.Dir = (filpX) ? Vector2.left : Vector2.right;
        bullet.Speed = 4f;
        playerBulletGO.transform.position = transform.position;

    }
}
