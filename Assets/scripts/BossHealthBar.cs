using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Slider healthBar; // 체력바 Slider를 연결할 변수
    private BossSlimeEnemy boss;

    private void Start()
    {
        boss = FindObjectOfType<BossSlimeEnemy>(); // BossSlimeEnemy 스크립트를 찾음
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
                healthBar.gameObject.SetActive(false);
            }
        }
    }
}
