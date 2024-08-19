using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoSpawner : BulletSpawner
{
    private float range = 10f;
    private float angle = 270;
    private float minSpeed = 4f;
    private float maxSpeed = 7f;
    private float minScale = 1f;
    private float maxScale = 1.5f;
    public override void ShootFireBall(){
        for (float i = 0; i < 10; i++){
            GameObject bulletGO = PoolManager.instance.GetGO(GetBulletName(enemyType));
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            bullet.Damage = enemy.GetDamage();
            bullet.Dir = Vector2.down;
            bullet.Speed = Random.Range(minSpeed, maxSpeed);
            Vector3 newPos = GetRandomPos();
            bullet.StartPos = newPos;
            bulletGO.transform.position = newPos;
            bulletGO.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            bulletGO.transform.localScale = Vector3.one * Random.Range(minScale, maxScale);
        }
    }

    private Vector3 GetRandomPos()
    {
        Vector3 curPos = transform.position;
        curPos.x += Random.Range(-range, range);
        curPos.y += 7f + Random.Range(-1f, 1f);
        return curPos;
    }
}
