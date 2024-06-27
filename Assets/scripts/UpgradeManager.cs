using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public GameObject upgradeUI; // 강화 선택지 UI

    public void ShowUpgradeOptions()
    {
        upgradeUI.SetActive(true); // 강화 선택지 UI 표시
    }

    public void UpgradeOption1()
    {
        // 예: 플레이어 공격력 증가
     //   PlayerStats.Instance.IncreaseAttackPower();
        CloseUpgradeOptions();
    }

    public void UpgradeOption2()
    {
        // 예: 플레이어 방어력 증가
       // PlayerStats.Instance.IncreaseDefense();
        CloseUpgradeOptions();
    }

    void CloseUpgradeOptions()
    {
        upgradeUI.SetActive(false); // 강화 선택지 UI 숨김
        PlayerGoldManager.Instance.SpendGold(100); // 골드 소비
    }
}
