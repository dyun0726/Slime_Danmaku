using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeBulletSpawner : BulletSpawner
{
    protected override void ShootFireBall(){
        Vector2 dir = (PlayerManager.Instance.GetPlayerLoc() - transform.position).x > 0 ? Vector2.right : Vector2.left;
        float angle = 20f;
        GameObject topBulletGO = PoolManager.instance.GetGO("Fireball_1");
        GameObject midBulletGO = PoolManager.instance.GetGO("Fireball_1");
        GameObject bottomBulletGO = PoolManager.instance.GetGO("Fireball_1");


        Bullet midBullet = midBulletGO.GetComponent<Bullet>();
        midBullet.Dir =  dir;
        midBullet.Speed = 2f;

        Bullet topBullet = topBulletGO.GetComponent<Bullet>();
        topBullet.Dir = RotateVector2(dir, angle);
        topBullet.Speed = 2f;

        Bullet bottomBullet = bottomBulletGO.GetComponent<Bullet>();
        bottomBullet.Dir = RotateVector2(dir, -angle);
        bottomBullet.Speed = 2f;


        topBulletGO.transform.position = transform.position;
        midBulletGO.transform.position = transform.position;
        bottomBullet.transform.position = transform.position;
    }

    Vector2 RotateVector2(Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);
        
        float x = v.x * cos - v.y * sin;
        float y = v.x * sin + v.y * cos;

        return new Vector2(x, y);
    }
}
