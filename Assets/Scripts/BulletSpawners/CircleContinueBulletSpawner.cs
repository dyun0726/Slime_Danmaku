using System.Collections;
using UnityEngine;

public class CircleContinueBulletSpawner : BulletSpawner
{
    public int angle = 13;
    private int currentAngle = 0;
    public override void ShootFireBall(){

        StartCoroutine(Fire());
    }

    private IEnumerator Fire()
    {
        for (int i = 0; i < 48; i++)
        {
            currentAngle = (currentAngle + angle) % 360;
            GameObject bulletGO = PoolManager.instance.GetGO(GetBulletName(enemyType));
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            bullet.Damage = enemy.GetDamage();
            bullet.Dir = Utility.RotateVector2(Vector2.right, currentAngle);
            bullet.Speed = speed;
            bullet.StartPos = transform.position;
            bulletGO.transform.position = transform.position;
            bulletGO.transform.rotation = Quaternion.Euler(new Vector3(0, 0, currentAngle));
            yield return new WaitForSeconds(0.05f);
        }
    }
}
