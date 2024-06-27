using TMPro;
using UnityEngine;

public class PlayerGoldManager : MonoBehaviour
{
    private static PlayerGoldManager instance;

    public static PlayerGoldManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerGoldManager>();
                if (instance == null)
                {
                    GameObject managerObject = new GameObject("PlayerGoldManager");
                    instance = managerObject.AddComponent<PlayerGoldManager>();
                    DontDestroyOnLoad(managerObject);
                }
            }
            return instance;
        }
    }

    public int gold = 0; // �÷��̾��� ���
    public TextMeshProUGUI goldText; // HUD�� ǥ�õ� ��� �ؽ�Ʈ

    void Awake()
    {
        // PlayerGoldManager�� ���� ������ �� ���� ��ü ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // goldText�� �������� �ʾ����� �������� ã��
        if (goldText == null)
        {
            goldText = GameObject.FindWithTag("GoldText").GetComponent<TextMeshProUGUI>();
        }

        UpdateGoldText();
    }

    public void AddGold(int amount)
    {
        gold += amount;
        UpdateGoldText();
        CheckForUpgradeOption();
    }

    void UpdateGoldText()
    {
        if (goldText != null)
        {
            goldText.text = "Gold: " + gold.ToString();
        }
        else
        {
            Debug.LogError("Gold Text is not assigned.");
        }
    }

    void CheckForUpgradeOption()
    {
        if (gold >= 100) // ��: 100��� ����� �� ��ȭ ������ ����
        {
            ShowUpgradeOptions();
        }
    }

    void ShowUpgradeOptions()
    {
        // ��ȭ ������ UI ǥ��
        Debug.Log("Upgrade options available!");
    }

    public void SpendGold(int amount)
    {
        gold -= amount;
        UpdateGoldText();
    }

    public void FindGoldTextInNewScene()
    {
        goldText = GameObject.FindWithTag("GoldText").GetComponent<TextMeshProUGUI>();
        UpdateGoldText();
    }
}
