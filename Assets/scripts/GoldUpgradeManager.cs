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

    private int gold;


    // 강화 단계별 설정값
    private readonly int[] healthUpgradeCosts = { 100, 200, 400, 600, 800 };
    private readonly float[] healthUpgradeValues = { 10f, 10f, 10f, 15f, 15f };
    private readonly int[] magicUpgradeCosts = { 100, 200, 400, 600, 800 };
    private readonly float[] magicUpgradeValues = { 1f, 1f, 1f, 1f, 1f };
    private readonly int[] speedUpgradeCosts = { 100, 200, 400, 600, 800 };
    private readonly float[] speedUpgradeValues = { 0.25f, 0.25f, 0.25f, 0.25f, 0.25f };
    private readonly int[] jumpUpgradeCosts = { 100, 200, 400, 600, 800 };
    private readonly float[] jumpUpgradeValues = { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
    private readonly int[] expUpgradeCosts = { 200, 400, 600, 800, 1000 };
    private readonly float[] expUpgradeValues = { 10f, 10f, 10f, 10f, 10f };
    private readonly int[] goldUpgradeCosts = { 200, 400, 600, 800, 1000 };
    private readonly float[] goldUpgradeValues = { 10f, 10f, 10f, 10f, 10f };

    // 캐릭터 선택 스크립트
    public CharacterSelectManager csManager;

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
    }


  /*  void AddGold()
    {
        gold += 100;
        PlayerPrefs.SetInt("TotalGold", PlayerPrefs.GetInt("TotalGold", 0) + 100);
        SaveData();
        UpdateUI();

    }

    void Reset()
    {
        healthLevel = 0;
        jumpLevel = 0;
        speedLevel = 0;
        magicLevel  = 0;
        expLevel = 0;
        goldLevel = 0;

        healthUpgradeCost = healthUpgradeCosts[0];
        jumpUpgradeCost = jumpUpgradeCosts[0];
        speedUpgradeCost = speedUpgradeCosts[0];
        magicUpgradeCost = magicUpgradeCosts[0];
        expUpgradeCost = expUpgradeCosts[0];
        goldUpgradeCost = goldUpgradeCosts[0];

        SaveData();
        LoadData();
        UpdateUI();
    }
  */
    void UpgradeHealth()
    {
        Debug.Log("lv"+healthLevel + "gold" + gold + "cost " + healthUpgradeCost);
        if (healthLevel < healthUpgradeCosts.Length && gold >= healthUpgradeCost)
        {
            Debug.Log("upgrade");
            gold -= healthUpgradeCost;
            healthLevel++;
            healthUpgradeCost = healthLevel < healthUpgradeCosts.Length ? healthUpgradeCosts[healthLevel] : 0;

            SaveData();
            UpdateUI();
            CheckSpendGold();
        }
    }
    void UpgradeMagic()
    {
        if (magicLevel < magicUpgradeCosts.Length && gold >= magicUpgradeCost)
        {
            Debug.Log("upgrade");
            gold -= magicUpgradeCost;
            magicLevel++;
            magicUpgradeCost = magicLevel < magicUpgradeCosts.Length ? magicUpgradeCosts[magicLevel] : 0;

            SaveData();
            UpdateUI();
            CheckSpendGold();
        }
    }

    void UpgradeSpeed()
    {
        if (speedLevel < speedUpgradeCosts.Length && gold >= speedUpgradeCost)
        {
            Debug.Log("upgrade");
            gold -= speedUpgradeCost;
            speedLevel++;
            speedUpgradeCost = speedLevel < speedUpgradeCosts.Length ? speedUpgradeCosts[speedLevel] : 0;

            SaveData();
            UpdateUI();
            CheckSpendGold();
        }
    }

    void UpgradeJump()
    {
        if (jumpLevel < jumpUpgradeCosts.Length && gold >= jumpUpgradeCost)
        {
            Debug.Log("upgrade");
            gold -= jumpUpgradeCost;
            jumpLevel++;
            jumpUpgradeCost = jumpLevel < jumpUpgradeCosts.Length ? jumpUpgradeCosts[jumpLevel] : 0;

            SaveData();
            UpdateUI();
            CheckSpendGold();
        }
    }

    void UpgradeExp()
    {
        if (expLevel < expUpgradeCosts.Length && gold >= expUpgradeCost)
        {
            Debug.Log("upgrade");
            gold -= expUpgradeCost;
            expLevel++;
            expUpgradeCost = expLevel < expUpgradeCosts.Length ? expUpgradeCosts[expLevel] : 0;

            SaveData();
            UpdateUI();
            CheckSpendGold();
        }
    }

    void UpgradeGold()
    {
        if (goldLevel < goldUpgradeCosts.Length && gold >= goldUpgradeCost)
        {
            Debug.Log("upgrade");
            gold -= goldUpgradeCost;
            goldLevel++;
            goldUpgradeCost = goldLevel < goldUpgradeCosts.Length ? goldUpgradeCosts[goldLevel] : 0;

            SaveData();
            UpdateUI();
            CheckSpendGold();
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
        expValueText.text = "+" + totalExpIncrease + "%";
        expUpgradeCostText.text = expUpgradeCost > 0 ? expUpgradeCost + "" : "Max";
        expUpgradeButton.interactable = expUpgradeCost > 0;

        goldLevelText.text = "Lv" + goldLevel;
        float totalGoldIncrease = GetTotalGoldIncrease();
        goldValueText.text = "+" + totalGoldIncrease + "%";
        goldUpgradeCostText.text = goldUpgradeCost > 0 ? goldUpgradeCost + "" : "Max";
        goldUpgradeButton.interactable = goldUpgradeCost > 0;

        GoldText.text = "" + gold;

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
        PlayerPrefs.SetInt("Gold", gold);
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
        gold = PlayerPrefs.GetInt("Gold", 0);
    }

    private void CheckSpendGold()
    {
        // 상인 해금 확인
        if (PlayerPrefs.GetInt("CharacterUnlocked_7", 0) == 1) return;

        int totalGold = PlayerPrefs.GetInt("TotalGold", 0);
        Debug.Log(totalGold);
        // 누적 소비 골드 일정 이상시 해금
        if (totalGold - gold >= 500){
            PlayerPrefs.SetInt("CharacterUnlocked_7", 1);
            PlayerPrefs.Save();

            csManager.LoadUnlockData();
            csManager.SelectCharacter(0);
        }

    }
}