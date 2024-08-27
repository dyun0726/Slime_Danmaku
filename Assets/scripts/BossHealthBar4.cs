using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar4 : MonoBehaviour
{
    private Slider healthBar; // 체력바 Slider를 연결할 변수
    private NecromancerEnemy boss;
    public Princess princess; // 공주 참조

    private void Start()
    {
        boss = FindObjectOfType<NecromancerEnemy>(); // BossSlimeEnemy 스크립트를 찾음
        healthBar = GetComponent<Slider>();
        healthBar.maxValue = boss.health;
    }

    private void Update()
    {
        if (boss != null)
        {
            healthBar.value = boss.health;

            // 보스가 죽었으면 체력바를 비활성화
            if (boss.IsDead)
            {
                princess.gameObject.SetActive(true);
                healthBar.gameObject.SetActive(false);
            }
        }
    }
}