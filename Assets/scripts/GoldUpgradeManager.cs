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

    public TextMeshProUGUI speedLevelText;
    public TextMeshProUGUI speedValueText;
    public TextMeshProUGUI speedUpgradeCostText;

    public TextMeshProUGUI jumpLevelText;
    public TextMeshProUGUI jumpValueText;
    public TextMeshProUGUI jumpUpgradeCostText;

    public TextMeshProUGUI expLevelText;
    public TextMeshProUGUI expValueText;
    public TextMeshProUGUI expUpgradeCostText;

    public TextMeshProUGUI goldLevelText;
    public TextMeshProUGUI goldValueText;
    public TextMeshProUGUI goldUpgradeCostText;

    public TextMeshProUGUI GoldText;


    public Button healthUpgradeButton;
    public Button magicUpgradeButton;
    public Button speedUpgradeButton;
    public Button goldUpgradeButton;
    public Button expUpgradeButton;
    public Button jumpUpgradeButton;


    public Button AddGoldButton;

    private int healthLevel;
    private int healthUpgradeCost;

    private int magicLevel;
    private int magicUpgradeCost;

    private int speedLevel;
    private int speedUpgradeCost;

    private int jumpLevel;
    private int jumpUpgradeCost;

    private int expLevel;
    private int expUpgradeCost;

    private int goldLevel;
    private int goldUpgradeCost;


    // 강화 단계별 설정값
    private readonly int[] healthUpgradeCosts = { 1, 2, 4, 8, 16 };
    private readonly float[] healthUpgradeValues = { 10f, 20f, 40f, 70f, 110f };
    private readonly int[] magicUpgradeCosts = { 1, 2, 4, 8, 16 };
    private readonly float[] magicUpgradeValues = { 10f, 20f, 40f, 70f, 110f };
    private readonly int[] speedUpgradeCosts = { 1, 2, 4, 8, 16 };
    private readonly float[] speedUpgradeValues = { 1f, 1f, 1f, 1f, 1f };
    private readonly int[] jumpUpgradeCosts = { 1, 2, 4, 8, 16 };
    private readonly float[] jumpUpgradeValues = { 1f, 1f, 1f, 1f, 1f };
    private readonly int[] expUpgradeCosts = { 1, 2, 4, 8, 16 };
    private readonly float[] expUpgradeValues = { 10f, 20f, 30f, 40f, 50f };
    private readonly int[] goldUpgradeCosts = { 1, 2, 4, 8, 16 };
    private readonly float[] goldUpgradeValues = { 10f, 20f, 30f, 40f, 50f };

    void Start()
    {
        LoadData();
        UpdateUI();
        healthUpgradeButton.onClick.AddListener(UpgradeHealth);
        magicUpgradeButton.onClick.AddListener(UpgradeMagic);
        speedUpgradeButton.onClick.AddListener(UpgradeSpeed);
        goldUpgradeButton.onClick.AddListener(UpgradeGold);
        jumpUpgradeButton.onClick.AddListener(UpgradeJump);
        expUpgradeButton.onClick.AddListener(UpgradeExp);

        AddGoldButton.onClick.AddListener(AddGold);
    }


    void AddGold()
    {
        PlayerManager.Instance.gold += 20;
        SaveData();
        UpdateUI();

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

    void UpgradeSpeed()
    {
        if (speedLevel < speedUpgradeCosts.Length && PlayerManager.Instance.gold >= speedUpgradeCost)
        {
            Debug.Log("upgrade");
            PlayerManager.Instance.gold -= speedUpgradeCost;
            speedLevel++;
            speedUpgradeCost = speedLevel < speedUpgradeCosts.Length ? speedUpgradeCosts[speedLevel] : 0;

            SaveData();
            UpdateUI();
        }
    }

    void UpgradeJump()
    {
        if (jumpLevel < jumpUpgradeCosts.Length && PlayerManager.Instance.gold >= jumpUpgradeCost)
        {
            Debug.Log("upgrade");
            PlayerManager.Instance.gold -= jumpUpgradeCost;
            jumpLevel++;
            jumpUpgradeCost = jumpLevel < jumpUpgradeCosts.Length ? jumpUpgradeCosts[jumpLevel] : 0;

            SaveData();
            UpdateUI();
        }
    }

    void UpgradeExp()
    {
        if (expLevel < expUpgradeCosts.Length && PlayerManager.Instance.gold >= expUpgradeCost)
        {
            Debug.Log("upgrade");
            PlayerManager.Instance.gold -= expUpgradeCost;
            expLevel++;
            expUpgradeCost = expLevel < expUpgradeCosts.Length ? expUpgradeCosts[expLevel] : 0;

            SaveData();
            UpdateUI();
        }
    }

    void UpgradeGold()
    {
        if (goldLevel < goldUpgradeCosts.Length && PlayerManager.Instance.gold >= goldUpgradeCost)
        {
            Debug.Log("upgrade");
            PlayerManager.Instance.gold -= goldUpgradeCost;
            goldLevel++;
            goldUpgradeCost = speedLevel < goldUpgradeCosts.Length ? goldUpgradeCosts[speedLevel] : 0;

            SaveData();
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        healthLevelText.text = "Lv" + healthLevel;
        float totalHealthIncrease = GetTotalHealthIncrease();
        healthValueText.text = "+" + totalHealthIncrease;
        healthUpgradeCostText.text = healthUpgradeCost > 0 ? healthUpgradeCost + "" : "Max";
        healthUpgradeButton.interactable = healthUpgradeCost > 0;

        magicLevelText.text = "Lv" + magicLevel;
        float totalMagicIncrease = GetTotalMagicIncrease();
        magicValueText.text = "+" + totalMagicIncrease;
        magicUpgradeCostText.text = magicUpgradeCost > 0 ? magicUpgradeCost + "" : "Max";
        magicUpgradeButton.interactable = magicUpgradeCost > 0;

        speedLevelText.text = "Lv" + speedLevel;
        float totalSpeedIncrease = GetTotalSpeedIncrease();
        speedValueText.text = "+" + totalSpeedIncrease;
        speedUpgradeCostText.text = speedUpgradeCost > 0 ? speedUpgradeCost + "" : "Max";
        speedUpgradeButton.interactable = speedUpgradeCost > 0;

        jumpLevelText.text = "Lv" + jumpLevel;
        float totalJumpIncrease = GetTotalJumpIncrease();
        jumpValueText.text = "+" + totalJumpIncrease;
        jumpUpgradeCostText.text = jumpUpgradeCost > 0 ? jumpUpgradeCost + "" : "Max";
        jumpUpgradeButton.interactable = jumpUpgradeCost > 0;

        expLevelText.text = "Lv" + expLevel;
        float totalExpIncrease = GetTotalExpIncrease();
        expValueText.text = "+" + totalExpIncrease;
        expUpgradeCostText.text = expUpgradeCost > 0 ? expUpgradeCost + "" : "Max";
        expUpgradeButton.interactable = expUpgradeCost > 0;

        goldLevelText.text = "Lv" + goldLevel;
        float totalGoldIncrease = GetTotalGoldIncrease();
        goldValueText.text = "+" + totalGoldIncrease + "%";
        goldUpgradeCostText.text = goldUpgradeCost > 0 ? goldUpgradeCost + "" : "Max";
        goldUpgradeButton.interactable = goldUpgradeCost > 0;

        GoldText.text = "" + PlayerManager.Instance.gold;

        PlayerPrefs.SetFloat("HpBonus", totalHealthIncrease);
        PlayerPrefs.SetFloat("MagicBonus", totalMagicIncrease);
        PlayerPrefs.SetFloat("SpeedBonus", totalSpeedIncrease);
        PlayerPrefs.SetFloat("JumpBonus", totalJumpIncrease);
        PlayerPrefs.SetFloat("ExpBonus", totalExpIncrease);
        PlayerPrefs.SetFloat("GoldBonus", totalGoldIncrease);
        PlayerPrefs.Save();
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

    float GetTotalSpeedIncrease()
    {
        float totalspeedIncrease = 0;
        for (int i = 0; i < speedLevel; i++)
        {
            totalspeedIncrease += speedUpgradeValues[i];
        }
        return totalspeedIncrease;
    }
    float GetTotalJumpIncrease()
    {
        float totaljumpIncrease = 0;
        for (int i = 0; i < jumpLevel; i++)
        {
            totaljumpIncrease += jumpUpgradeValues[i];
        }
        return totaljumpIncrease;
    }
    float GetTotalExpIncrease()
    {
        float totalexpIncrease = 0;
        for (int i = 0; i < expLevel; i++)
        {
            totalexpIncrease += expUpgradeValues[i];
        }
        return totalexpIncrease;
    }

    float GetTotalGoldIncrease()
    {
        float totalgoldIncrease = 0;
        for (int i = 0; i < goldLevel; i++)
        {
            totalgoldIncrease += goldUpgradeValues[i];
        }
        return totalgoldIncrease;
    }

    void SaveData()
    {
        PlayerPrefs.SetInt("HealthLevel", healthLevel);
        PlayerPrefs.SetInt("HealthUpgradeCost", healthUpgradeCost);
        PlayerPrefs.SetInt("MagicLevel", magicLevel);
        PlayerPrefs.SetInt("MagicUpgradeCost", magicUpgradeCost);
        PlayerPrefs.SetInt("SpeedLevel", speedLevel);
        PlayerPrefs.SetInt("SpeedUpgradeCost", speedUpgradeCost);
        PlayerPrefs.SetInt("JumpLevel", jumpLevel);
        PlayerPrefs.SetInt("JumpUpgradeCost", jumpUpgradeCost);
        PlayerPrefs.SetInt("ExpLevel", expLevel);
        PlayerPrefs.SetInt("ExpUpgradeCost", expUpgradeCost);
        PlayerPrefs.SetInt("GoldLevel", goldLevel);
        PlayerPrefs.SetInt("GoldUpgradeCost", goldUpgradeCost);
        PlayerPrefs.SetInt("Gold", PlayerManager.Instance.gold);
        PlayerPrefs.Save();
    }

    void LoadData()
    {
        healthLevel = PlayerPrefs.GetInt("HealthLevel", 0);
        healthUpgradeCost = PlayerPrefs.GetInt("HealthUpgradeCost", healthUpgradeCosts[0]);
        magicLevel = PlayerPrefs.GetInt("MagicLevel", 0);
        magicUpgradeCost = PlayerPrefs.GetInt("MagicUpgradeCost", magicUpgradeCosts[0]);
        speedLevel = PlayerPrefs.GetInt("SpeedLevel", 0);
        speedUpgradeCost = PlayerPrefs.GetInt("SpeedUpgradeCost", speedUpgradeCosts[0]);
        jumpLevel = PlayerPrefs.GetInt("JumpLevel", 0);
        jumpUpgradeCost = PlayerPrefs.GetInt("JumpUpgradeCost", jumpUpgradeCosts[0]);
        expLevel = PlayerPrefs.GetInt("ExpLevel", 0);
        expUpgradeCost = PlayerPrefs.GetInt("ExpUpgradeCost", expUpgradeCosts[0]);
        goldLevel = PlayerPrefs.GetInt("GoldLevel", 0);
        goldUpgradeCost = PlayerPrefs.GetInt("GoldUpgradeCost", goldUpgradeCosts[0]);
        PlayerManager.Instance.gold = PlayerPrefs.GetInt("Gold", PlayerManager.Instance.gold);
    }
}