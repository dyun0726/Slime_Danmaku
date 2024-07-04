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

    // 캐릭터 스크립트
    private Player player;

    
    // public TextMeshProUGUI healthText; //HUD로 옮김

    // 경험치 관련 변수
    public int gold = 0; // 플레이어의 골드
    // public TextMeshProUGUI goldText; // HUD로 옮김

    public int exp = 0; // 플레이어의 경험치
    public int levelUpExp = 10; // 레벨업에 필요한 경험치
    public int level = 1;


    // 캐릭터 스탯
    public float maxHealth = 100; // 체력
    public float currentHealth = 50;
    public float strength = 10; // 힘
    public float agility = 10; // 민첩성 (10초에 x번)
    public float magic = 10; // 지능
    public float castingSpeed = 10; // 마법 시전 속도 (10초에 x번)

    public float moveSpeed = 3f; // 이동 속도
    public float jumpForce = 8f;
    public float knockbackSpeed = 10f;


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
        // UpdateHealthText();
        // UpdateGoldText();
    }

    public void RegisterPlayer(GameObject player){
        this.player = player.GetComponent<Player>();
    }


    // HUD로 옮김
    // private void UpdateHealthText(){
    //     if (healthText != null){
    //         healthText.text = "Health: " + currentHealth.ToString();
    //     } else {
    //         Debug.LogError("Health Text is not assigned.");
    //     }
    // }

    public void TakeDamage(float amount, Vector2 dir)
    {
        // 후에 무적시간을 추가해서 로직 처리 필요
        
        currentHealth -= amount;
        player.Knockback(dir);
        Debug.Log("Took damage: " + amount + ", Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        // UpdateHealthText();
    }

    public void Heal(float amount)
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
        // UpdateGoldText();
        CheckForUpgradeOption();
    }

    // void UpdateGoldText()
    // {
    //     if (goldText != null)
    //     {
    //         goldText.text = "Gold: " + gold.ToString();
    //     }
    //     else
    //     {
    //         Debug.LogError("Gold Text is not assigned.");
    //     }
    // }
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
        // UpdateGoldText();
    }

    // public void FindGoldTextInNewScene()
    // {
    //     // goldText = GameObject.FindWithTag("GoldText").GetComponent<TextMeshProUGUI>();
    //     UpdateGoldText();
    // }

    // 스탯 동기화 함수
    public void UpdateStats(){
        player.strength = strength; 
        player.agility = agility;
        player.magic =  magic;
        player.castingSpeed = castingSpeed;
        player.moveSpeed = moveSpeed;
        player.jumpForce = jumpForce;
        player.knockbackSpeed = knockbackSpeed;
    }


    // 스탯 증가 및 세팅 함수
        public void IncreaseStrength(float amount)
    {
        strength += amount;
        Debug.Log("Strength increased: " + strength);
        UpdateStats();
    }

    public void IncreaseAgility(float amount)
    {
        agility += amount;
        Debug.Log("Agility increased: " + agility);
        UpdateStats();
    }

    public void IncreaseMagic(float amount)
    {
        magic += amount;
        Debug.Log("Magic increased: " + magic);
        UpdateStats();
    }

    public void IncreaseCastingSpeed(float amount)
    {
        castingSpeed += amount;
        Debug.Log("Magic increased: " + magic);
        UpdateStats();
    }

    public void IncreaseMoveSpeed(float amount)
    {
        moveSpeed += amount;
        Debug.Log("MoveSpeed increased: " + moveSpeed);
        UpdateStats();
    }
    public void IncreaseJumpForce(float amount)
    {
        jumpForce += amount;
        UpdateStats();
    }

    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;
        currentHealth += amount;
        // UpdateStats();
    }

    public void IncreaseExp(int amount)
    {
        exp += amount;
        // 레벨업에 필요한 경험치에 도달하면
        while (exp >= levelUpExp){
            level += 1;
            exp -= levelUpExp;
            // 후에 강화 함수 추가
            Debug.Log("Level Up: " + level);
        }
    }


    public void SetStrength(float amount)
    {
        strength = amount;
        Debug.Log("Strength increased: " + strength);
    }

    public void SetAgility(float amount)
    {
        agility = amount;
        Debug.Log("Agility increased: " + agility);
    }

    public void SetMagic(float amount)
    {
        magic = amount;
        Debug.Log("Magic increased: " + magic);
    }
    public void SetMoveSpeed(float amount)
    {
        moveSpeed = amount;
        Debug.Log("MoveSpeed increased: " + moveSpeed);
    }
    public void SetJumpForce(float amount)
    {
        jumpForce = amount;
        Debug.Log("JumpForce increased: " + jumpForce);
    }
}   
