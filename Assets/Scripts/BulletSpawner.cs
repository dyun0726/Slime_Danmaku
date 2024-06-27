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

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator ShootCoroutine(){
        while (true) {
            ShootFireBall();
            yield return new WaitForSeconds(shootInterval);
        }
    }
    private void ShootFireBall(){
        GameObject bullet = PoolManager.instance.GetGO("Fireball_1");
        bullet.transform.position = transform.position;

    }
}
