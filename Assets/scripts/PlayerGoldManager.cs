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

    public int gold = 0; // 플레이어의 골드
    public TextMeshProUGUI goldText; // HUD에 표시될 골드 텍스트

    void Awake()
    {
        // PlayerGoldManager가 새로 생성될 때 기존 객체 제거
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
        // goldText가 설정되지 않았으면 동적으로 찾음
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
        if (gold >= 100) // 예: 100골드 모았을 때 강화 선택지 제공
        {
            ShowUpgradeOptions();
        }
    }

    void ShowUpgradeOptions()
    {
        // 강화 선택지 UI 표시
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
