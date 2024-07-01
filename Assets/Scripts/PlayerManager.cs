using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

public class PlayerManager : MonoBehaviour
{
    // singletone
    private static PlayerManager _instance;

    public static PlayerManager Instance{
        get{
            if (_instance == null){
                _instance = FindObjectOfType<PlayerManager>();
                if (_instance == null){
                    GameObject playerManager = new GameObject("PlayerManager");
                    _instance = playerManager.AddComponent<PlayerManager>();
                    DontDestroyOnLoad(playerManager);
                }
            }
            return _instance;
        }
    }

    // 캐릭터 오브젝트
    private GameObject player;

    // 체력 관련 변수
    public float maxHealth = 100;
    public float currentHealth = 50;
    public TextMeshProUGUI healthText;

    // 경험치 관련 변수
    public int gold = 0; // 플레이어의 골드
    public TextMeshProUGUI goldText; // HUD에 표시될 골드 텍스트


    // 캐릭터 스탯
    public int strength = 10; // 힘
    public int agility = 10; // 민첩성 (10초에 x번)
    public int intelligence = 10; // 지능
    public int castingSpeed = 10; // 마법 시전 속도 (10초에 x번)

    public float moveSpeed = 3f;
    public float JumpForce = 8f;


    private void Awake() {
        if (_instance == null){
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this){
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateHealthText();
        UpdateGoldText();
    }

    public void RegisterPlayer(GameObject player){
        this.player = player;
    }

    private void UpdateHealthText(){
        if (healthText != null){
            healthText.text = "Health: " + currentHealth.ToString();
        } else {
            Debug.LogError("Health Text is not assigned.");
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Took damage: " + amount + ", Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        UpdateHealthText();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log("Healed: " + amount + ", Current health: " + currentHealth);
    }

    void Die()
    {
        Debug.Log("Player died!");
        // 플레이어 사망 처리 코드
    }

    // 골드 관련 함수들
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
        // goldText = GameObject.FindWithTag("GoldText").GetComponent<TextMeshProUGUI>();
        UpdateGoldText();
    }

        public void IncreaseStrength(int amount)
    {
        strength += amount;
        Debug.Log("Strength increased: " + strength);
    }

    public void IncreaseAgility(int amount)
    {
        agility += amount;
        Debug.Log("Agility increased: " + agility);
    }

    public void IncreaseIntelligence(int amount)
    {
        intelligence += amount;
        Debug.Log("Intelligence increased: " + intelligence);
    }
    public void IncreaseMoveSpeed(int amount)
    {
        moveSpeed += amount;
        Debug.Log("MoveSpeed increased: " + moveSpeed);
    }
    public void IncreaseJumpForce(int amount)
    {
        JumpForce += amount;
        Debug.Log("JumpForce increased: " + JumpForce);
    }


    public void SetStrength(int amount)
    {
        strength = amount;
        Debug.Log("Strength increased: " + strength);
    }

    public void SetAgility(int amount)
    {
        agility = amount;
        Debug.Log("Agility increased: " + agility);
    }

    public void SetIntelligence(int amount)
    {
        intelligence = amount;
        Debug.Log("Intelligence increased: " + intelligence);
    }
    public void SetMoveSpeed(int amount)
    {
        moveSpeed = amount;
        Debug.Log("MoveSpeed increased: " + moveSpeed);
    }
    public void SetJumpForce(int amount)
    {
        JumpForce = amount;
        Debug.Log("JumpForce increased: " + JumpForce);
    }
}   
