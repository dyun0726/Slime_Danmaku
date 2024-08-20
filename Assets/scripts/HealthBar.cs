using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider; // 체력바 슬라이더

    // 체력바를 초기화하는 메서드
    public void Initialize(float maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    // 체력바를 업데이트하는 메서드
    public void SetHealth(float health)
    {
        slider.value = health;
    }
}
