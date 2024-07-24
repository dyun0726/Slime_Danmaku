using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    // public float shootInterval = 4f;
    public enum EnemyType{Slime, SlimeBoss, Archer, Monk, SlimeBossBomb, Hashashin}
    public EnemyType enemyType;
    public float speed = 2f;
    protected Enemy enemy; // 부모 오브젝트
    protected virtual void Awake() {
        enemy = GetComponentInParent<Enemy>();
    }

    // Start is called before the first frame update
    public virtual void ShootFireBall(){
        GameObject bulletGO = PoolManager.instance.GetGO(GetBulletName(enemyType));
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bool isRight = (PlayerManager.Instance.GetPlayerLoc() - transform.position).x > 0;
        bullet.Damage = enemy.GetDamage();
        bullet.Dir =  isRight ? Vector2.right : Vector2.left;
        bullet.Speed = speed;
        bullet.StartPos = transform.position;
        bulletGO.transform.position = transform.position;
        bulletGO.transform.rotation = Quaternion.Euler( isRight ? Vector3.zero : new Vector3(0, 0, 180));
    }

    protected string GetBulletName(EnemyType enemyType){
        switch (enemyType)
        {
            case EnemyType.Slime:
                return "Fireball_1";
            case EnemyType.SlimeBoss:
                return "Boss_Fireball_1";
            case EnemyType.Archer:
                return "Arrow_1";
            case EnemyType.Monk:
                return "Energy_Blast_1";
            case EnemyType.SlimeBossBomb:
                return "Boss_Bomb_Fireball_1";
            case EnemyType.Hashashin:
                return "Sword_Large_1";
            
            default:
                Debug.Log("Set EnemyType in BulletSpawner");
                return null;

        }
    }

    protected Vector2 GetPlayerDirection(){
        Vector3 locDiff = PlayerManager.Instance.GetPlayerLoc() - transform.position;
        return ((Vector2)locDiff).normalized;
    }

    protected float GetRotationAngle(Vector2 dir){
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }
}
