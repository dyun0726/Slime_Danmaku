using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinBullet : Bullet
{
    private Rigidbody2D rb;
    private Animator animator;
    private string bulletPrefabName = "Fireball_1";
    private float subSpeed = 5f;
    public int spreadAngle = 30;
    private float explodeTime = 5f;
    public float ExplodeTime {get {return explodeTime;} set {explodeTime = value;}}

    public bool isSetted = false;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        // live 체크 함수
        if (!GameManager.Instance.isLive)
        {  
            return;
        }

        if (isSetted)
        {
            if (explodeTime > 0)
            {
                explodeTime -= Time.deltaTime;

            }
            else
            {
                Debug.Log("explosion!");
                isSetted = false;
                animator.SetTrigger("Explosion");
            }
        }
    }

    public void ThrowBomb()
    {
        rb.AddForce(Dir * Speed, ForceMode2D.Impulse);
    }

    private void Explosion()
    {
        // 8방향으로 총알 발사
        for (int angle = 0; angle < 360; angle += spreadAngle)
        {
            // 오브젝트 풀에서 총알 프리팹 가져오기
            GameObject bulletPrefab = PoolManager.instance.GetGO(bulletPrefabName);
            Bullet bullet = bulletPrefab.GetComponent<Bullet>();

            if (bulletPrefab != null || bullet != null)
            {
                bullet.Damage = Damage;
                bullet.Dir = Utility.RotateVector2(Vector2.right, angle);
                bullet.Speed = subSpeed;
                bullet.StartPos = transform.position;
                bullet.Range = 10f; // 필요에 따라 조정
                bulletPrefab.transform.position = transform.position;

            }
        }
    }
}
