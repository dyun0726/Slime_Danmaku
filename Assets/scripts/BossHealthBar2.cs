using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar2 : MonoBehaviour
{
    private Slider healthBar; // 체력바 Slider를 연결할 변수
    private BossPaladinEnemy boss;
    public Portal portal; // 포탈 참조

    private void Start()
    {
        boss = FindObjectOfType<BossPaladinEnemy>(); // BossSlimeEnemy 스크립트를 찾음
        healthBar = GetComponent<Slider>(); // 슬라이더 연결
    }

    private void Update()
    {
        if (boss != null)
        {
            healthBar.value = boss.health;

            // 보스가 죽었으면 체력바를 비활성화
            if (boss.IsDead)
            {
                portal.ActivatePortal();
                healthBar.gameObject.SetActive(false);
            }
        }
    }
}