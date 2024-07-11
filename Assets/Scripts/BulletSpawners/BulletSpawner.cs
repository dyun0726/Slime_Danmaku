using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public float shootInterval = 4f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShootCoroutine());
    }

    private IEnumerator ShootCoroutine(){
        while (true) {
            ShootFireBall();
            yield return new WaitForSeconds(shootInterval);
        }
    }
    protected virtual void ShootFireBall(){
        GameObject bulletGO = PoolManager.instance.GetGO("Fireball_1");
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.Dir =  (PlayerManager.Instance.GetPlayerLoc() - transform.position).x > 0 ? Vector2.right : Vector2.left;
        bullet.Speed = 2f;
        bulletGO.transform.position = transform.position;
    }
}
