using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoldUpgradeManager : MonoBehaviour
{
    public TextMeshProUGUI healthLevelText;
    public TextMeshProUGUI healthValueText;
    public TextMeshProUGUI healthUpgradeCostText;

    public TextMeshProUGUI magicLevelText;
    public TextMeshProUGUI magicValueText;
    public TextMeshProUGUI magicUpgradeCostText;

    public TextMeshProUGUI GoldText;
    public Button healthUpgradeButton;
    public Button magicUpgradeButton;

    private int healthLevel;
    private int healthUpgradeCost;

    private int magicLevel;
    private int magicUpgradeCost;
    // 강화 단계별 설정값
    private readonly int[] healthUpgradeCosts = { 1, 2, 4, 8, 16 };
    private readonly float[] healthUpgradeValues = { 10f, 20f, 40f, 70f, 110f };
    private readonly int[] magicUpgradeCosts = { 1, 2, 4, 8, 16 };
    private readonly float[] magicUpgradeValues = { 10f, 20f, 40f, 70f, 110f };

    void Start()
    {
        LoadData();
        UpdateUI();
        healthUpgradeButton.onClick.AddListener(UpgradeHealth);
        magicUpgradeButton.onClick.AddListener(UpgradeMagic);
    }

    void UpgradeHealth()
    {
        Debug.Log("lv"+healthLevel + "gold" + PlayerManager.Instance.gold + "cost " + healthUpgradeCost);
        if (healthLevel < healthUpgradeCosts.Length && PlayerManager.Instance.gold >= healthUpgradeCost)
        {
            Debug.Log("upgrade");
            PlayerManager.Instance.gold -= healthUpgradeCost;
            healthLevel++;
            healthUpgradeCost = healthLevel < healthUpgradeCosts.Length ? healthUpgradeCosts[healthLevel] : 0;

            SaveData();
            UpdateUI();
        }
    }
    void UpgradeMagic()
    {
        if (magicLevel < magicUpgradeCosts.Length && PlayerManager.Instance.gold >= magicUpgradeCost)
        {
            Debug.Log("upgrade");
            PlayerManager.Instance.gold -= magicUpgradeCost;
            magicLevel++;
            magicUpgradeCost = magicLevel < magicUpgradeCosts.Length ? magicUpgradeCosts[magicLevel] : 0;

            SaveData();
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        healthLevelText.text = "Lv" + healthLevel;
        float totalHealthIncrease = GetTotalHealthIncrease();
        healthValueText.text = "+" + totalHealthIncrease;
        healthUpgradeCostText.text = healthUpgradeCost > 0 ? healthUpgradeCost + "" : "Max Level Reached";
        healthUpgradeButton.interactable = healthUpgradeCost > 0;

        magicLevelText.text = "Lv" + magicLevel;
        float totalMagicIncrease = GetTotalMagicIncrease();
        magicValueText.text = "+" + totalMagicIncrease;
        magicUpgradeCostText.text = magicUpgradeCost > 0 ? magicUpgradeCost + "" : "Max Level Reached";
        magicUpgradeButton.interactable = magicUpgradeCost > 0;

        GoldText.text = "" + PlayerManager.Instance.gold;
    }

    float GetTotalHealthIncrease()
    {
        float totalHealthIncrease = 0;
        for (int i = 0; i < healthLevel; i++)
        {
            totalHealthIncrease += healthUpgradeValues[i];
        }
        return totalHealthIncrease;
    }

    float GetTotalMagicIncrease()
    {
        float totalmagicIncrease = 0;
        for (int i = 0; i < magicLevel; i++)
        {
            totalmagicIncrease += magicUpgradeValues[i];
        }
        return totalmagicIncrease;
    }

    void SaveData()
    {
        PlayerPrefs.SetInt("HealthLevel", healthLevel);
        PlayerPrefs.SetInt("HealthUpgradeCost", healthUpgradeCost);
        PlayerPrefs.SetInt("MagicLevel", magicLevel);
        PlayerPrefs.SetInt("MagicUpgradeCost", magicUpgradeCost);
        PlayerPrefs.SetInt("Gold", PlayerManager.Instance.gold);
        PlayerPrefs.Save();
    }

    void LoadData()
    {
        healthLevel = PlayerPrefs.GetInt("HealthLevel", 0);
        healthUpgradeCost = PlayerPrefs.GetInt("HealthUpgradeCost", healthUpgradeCosts[0]);
        magicLevel = PlayerPrefs.GetInt("MagicLevel", 0);
        magicUpgradeCost = PlayerPrefs.GetInt("MagicUpgradeCost", magicUpgradeCosts[0]);
        PlayerManager.Instance.gold = PlayerPrefs.GetInt("Gold", PlayerManager.Instance.gold);
    }
}