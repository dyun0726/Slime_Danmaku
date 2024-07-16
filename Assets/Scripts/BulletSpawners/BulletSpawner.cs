using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public float shootInterval = 4f;
    public enum EnemyType{Enemy, Boss}

    public EnemyType enemyType;
    public float speed = 2f;

    // Start is called before the first frame update
    public virtual void ShootFireBall(){
        GameObject bulletGO = PoolManager.instance.GetGO(GetBulletName(enemyType));
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.Dir =  (PlayerManager.Instance.GetPlayerLoc() - transform.position).x > 0 ? Vector2.right : Vector2.left;
        bullet.Speed = speed;
        bulletGO.transform.position = transform.position;
    }

    protected string GetBulletName(EnemyType enemyType){
        switch (enemyType)
        {
            case EnemyType.Enemy:
                return "Fireball_1";
            case EnemyType.Boss:
                return "Boss_Fireball_1";
            default:
                Debug.Log("Set EnemyType in BulletSpawner");
                return null;

        }
    }

    protected Vector2 GetPlayerDirection(){
        Vector3 locDiff = PlayerManager.Instance.GetPlayerLoc() - transform.position;
        return new Vector2(locDiff.x, locDiff.y).normalized;
    }
}
