using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar4 : MonoBehaviour
{
    public Slider healthBar; // 체력바 Slider를 연결할 변수
    private NecromancerEnemy boss;
    public Portal portal; // 포탈 참조

    private void Start()
    {
        boss = FindObjectOfType<NecromancerEnemy>(); // BossSlimeEnemy 스크립트를 찾음
        healthBar.maxValue = boss.maxhealth;
        healthBar.value = boss.curhealth;
    }

    private void Update()
    {
        if (boss != null)
        {
            healthBar.value = boss.curhealth;

            // 보스가 죽었으면 체력바를 비활성화
            if (boss.IsDead)
            {
                portal.ActivatePortal();
                healthBar.gameObject.SetActive(false);
            }
        }
    }
}