using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public GameObject upgradeUI; // ��ȭ ������ UI

    public void ShowUpgradeOptions()
    {
        upgradeUI.SetActive(true); // ��ȭ ������ UI ǥ��
    }

    public void UpgradeOption1()
    {
        // ��: �÷��̾� ���ݷ� ����
     //   PlayerStats.Instance.IncreaseAttackPower();
        CloseUpgradeOptions();
    }

    public void UpgradeOption2()
    {
        // ��: �÷��̾� ���� ����
       // PlayerStats.Instance.IncreaseDefense();
        CloseUpgradeOptions();
    }

    void CloseUpgradeOptions()
    {
        upgradeUI.SetActive(false); // ��ȭ ������ UI ����
        PlayerGoldManager.Instance.SpendGold(100); // ��� �Һ�
    }
}
