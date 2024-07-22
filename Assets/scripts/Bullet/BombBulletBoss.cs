using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBulletBoss : BossBullet
{
    public float explodeDistance = 4f; // 폭발 거리
    public string bulletPrefabName = "Fireball_1"; // 오브젝트 풀에서 총알 프리팹을 가져올 때 사용할 이름
    public float spreadAngle = 45f; // 방향 간의 각도

    private bool hasExploded = false;

    void Update()
    {
        // live 체크 함수
        if (!GameManager.Instance.isLive)
        {
            return;
        }

        // 폭발 거리 체크
        if (!hasExploded && getDist() >= explodeDistance)
        {
            Debug.Log(StartPos);
            Explode();
            Debug.Log("비거리:"+ getDist()+", 폭발거리:"+ explodeDistance);
        }


        if (transform.position.x < GameManager.Instance.leftBound || transform.position.x > GameManager.Instance.rightBound ||
            transform.position.y < GameManager.Instance.lowerBound || transform.position.y > GameManager.Instance.upperBound || getDist() > Range)
        {
            ReleaseObject();
        }

        transform.Translate(Speed * Time.deltaTime * Dir, Space.World);

    }

    private void Explode()
    {
        hasExploded = true;

        // 8방향으로 총알 발사
        for (int i = 0; i < 8; i++)
        {
            float angle = i * spreadAngle; // 0도, 45도, 90도, ..., 315도
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            // 오브젝트 풀에서 총알 프리팹 가져오기
            GameObject bulletPrefab = PoolManager.instance.GetGO(bulletPrefabName);
            Bullet bullet = bulletPrefab.GetComponent<Bullet>();

            if (bulletPrefab != null || bullet != null)
            {
                bullet.Dir = direction;
                bullet.Speed = 5f; // 필요에 따라 조정
                bullet.StartPos = transform.position;
                bullet.Range = 10f; // 필요에 따라 조정
                bulletPrefab.transform.position = transform.position;

            }
        }

        hasExploded = false;
        // 총알 본체 삭제
        ReleaseObject();
    }
}
