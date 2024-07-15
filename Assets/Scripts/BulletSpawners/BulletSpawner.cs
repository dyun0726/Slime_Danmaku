using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public float shootInterval = 4f;
    public enum EnemyType{Enemy, Boss}

    public EnemyType enemyType;

    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(ShootCoroutine());
    }

    private IEnumerator ShootCoroutine(){
        while (true) {
            ShootFireBall();
            yield return new WaitForSeconds(shootInterval);
        }
    }
    public virtual void ShootFireBall(){
        GameObject bulletGO = PoolManager.instance.GetGO(GetBulletName(enemyType));
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.Dir =  (PlayerManager.Instance.GetPlayerLoc() - transform.position).x > 0 ? Vector2.right : Vector2.left;
        bullet.Speed = 2f;
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
}
