using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using Unity.VisualScripting;

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
    public LevelUp uiLevelUp;

    
    // public TextMeshProUGUI healthText; //HUD로 옮김

    // 경험치 관련 변수
    public int gold = 0; // 플레이어의 골드
    // public TextMeshProUGUI goldText; // HUD로 옮김

    public int exp = 0; // 플레이어의 경험치
    public int levelUpExp = 10; // 레벨업에 필요한 경험치
    public int level = 1;


    // 캐릭터 스탯
    public float maxHealth = 100f; // 체력
    public float currentHealth = 50f;
    public float strength = 10f; // 힘
    public float agility = 10f; // 민첩성 (10초에 x번)
    public float baseMagic = 10f; // 마력
    public float magicPercent = 0f; // 마력 퍼센트 계산
    public float magic; // 총 계산 마력

    public float castingSpeed = 10; // 마법 시전 속도 (10초에 x번)

    public float moveSpeed = 3f; // 이동 속도
    public float jumpForce = 8f;
    public float knockbackSpeed = 10f;
    // 변수 추가 --------------------
    // 도현
    public float bulletSpeed = 4f; // 원거리 공격의 발사 속도
    public float bulletRange = 10f; // 원거리 공격의 사거리
    public int bulletPass = 0; // 원거리 공격 관통 가능 수
    public float lifeSteel = 0f; // 원거리 공격 생명력 흡수
    public float dotDamge = 0f; // 도트 데미지
    public float atkReduction = 0f; // 적 데미지 감소 디버프 (10 = 10% 감소)
    public float stunTime = 0f; // 적 스턴 디버프 시간
    public float criticalDamage = 100f; // 크리티컬 데미지 (100 = 100% 추가 데미지)
    public float armorPt = 0; // 방관 수치
    public float armorPtPercent = 0; //방관 퍼센트 (0 ~ 100 %)
    public float jumpstack = 0f;// 점프횟수 ex)2단점프, 3단점프
    public int resurrection = 0; // 남은 부활 횟수, 사망 시 절반의 체력으로 부활 
    public int superstance = 0;//슈퍼스탠스 여부, 1이면 슈퍼스탠스
    public float stance = 0f;//스탠스 수치, 0~100
    public float damagereduce = 0f;//뎀감 수치, 0~100
    public float expbonus = 0f;//경험치 보너스
    public float dropbonus = 0f;//드랍율 보너스
    public float goldbonus = 0f;//골드획득 보너스

    public float crirate = 0f;//치명타율, 0~100
    public float luckyshot = 0f;//일반몹 즉사율, 0~100
    public float shield = 0f;//스테이지마다 생성되는 실드의 양

    public GameObject deathPopup; // 팝업 창
    public Button confirmButton; // 확인 버튼
    public List<GameObject> objectsToRemove;

    // ------------------------------

    public float gravityMultiplier = 0f; // 중력 감소, (0 ~ 100%)

    // 타입 강화 횟수 변수
    public float fire_stack = 0;
    //골드획득텍스트프리팹
    public GameObject goldTextPrefab;
    public Canvas canvas;



    private void Awake() {
        if (_instance == null){
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this){
            Destroy(gameObject);
        }
       

        //테스트용 수치들
        shield = 20f;
        //damagereduce = 20f;
        //resurrection = 0;
        //expbonus = 50f;
        //goldbonus = 50f;
        //crirate = 50f;

        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        if (deathPopup != null)
        {
            deathPopup.SetActive(false);
            confirmButton.onClick.AddListener(OnConfirmButtonClick);
        }
        
    }
    
   void Update()
    {
        //    if (player = null)
        //    {
        //        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }


    private void OnConfirmButtonClick()
    {
        SceneManager.LoadScene("MainMenu"); 
    }

    // Start is called before the first frame update
    void Start()
    {
        calculateMagic();
        // UpdateHealthText();
        // UpdateGoldText();
    }


    // PlayerManager.cs
    

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
        float realDamage = amount * (1f - (damagereduce / 100f));

        // 실드가 있을 경우 실드를 먼저 소모
        if (shield > 0)
        {
            if (shield >= realDamage)
            {
                shield -= realDamage;
                realDamage = 0;
            }
            else
            {
                realDamage -= shield;
                shield = 0;
            }
        }

        // 남은 데미지를 체력에서 소모
        currentHealth -= realDamage;
        player.Knockback(dir);
        Debug.Log("Took damage: " + amount + ", Real damage: " + realDamage + ", Current health: " + currentHealth + ", Current shield: " + shield);

        if (currentHealth <= 0)
        {
            if (resurrection > 0)
            {
                resurrection--;
                currentHealth = maxHealth / 2;
                Debug.Log("Resurrected! Current health: " + currentHealth + ", Remaining resurrections: " + resurrection);
            }
            else
            {
                currentHealth = 0;
                Die();
            }
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
       
        if (deathPopup != null)
        {
            deathPopup.SetActive(true);
        }

       
        if (player != null)
        {
            //Destroy(player.gameObject);
            player.gameObject.SetActive(false);
        }
       
       
        Camera mainCamera = Camera.main;
        foreach (GameObject obj in objectsToRemove)
        {
            //Destroy(obj); //여기에 파괴할 거 다 넣을 수 있음
            obj.SetActive(false);

        }

    }

    // 골드 관련 함수들
    public void AddGold(int amount)
    {
        int goldToAdd = Mathf.FloorToInt(amount * (1 + goldbonus / 100f));
        gold += goldToAdd;
        ShowGoldText(goldToAdd);
        // UpdateGoldText();
        CheckForUpgradeOption();
    }

    private void ShowGoldText(int amount)
    {
        // 골드 텍스트 프리팹이 할당되지 않았을 경우 예외 처리
        if (goldTextPrefab == null)
        {
            Debug.LogError("goldTextPrefab is not assigned in the Inspector");
            return;
        }
        if (canvas == null)
        {
            Debug.LogError("Canvas is not assigned in the Inspector");
            return;
        }

        // 플레이어의 머리 위에 골드 텍스트 생성
        Vector3 spawnPosition = player.transform.position + new Vector3(0, 2f, 0); // y축으로 2 유닛 위쪽에 표시
        GameObject goldText = Instantiate(goldTextPrefab, spawnPosition, Quaternion.identity, transform);

        // 골드 텍스트 오브젝트에 TextMeshProUGUI 컴포넌트가 붙어 있는지 확인
        TextMeshProUGUI textComponent = goldText.GetComponent<TextMeshProUGUI>();
        if (textComponent == null)
        {
            Debug.LogError("The instantiated goldTextPrefab does not have a TextMeshProUGUI component");
            Destroy(goldText);
            return;
        }

        textComponent.text = "+" + amount + "G";

        // 일정 시간 후 텍스트 제거
        StartCoroutine(FadeAndDestroy(goldText, 100f)); // 1초 후 제거
    }

    private IEnumerator FadeAndDestroy(GameObject textObject, float duration)
    {
        TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();
        if (text == null)
        {
            Debug.LogError("The textObject does not have a TextMeshProUGUI component");
            yield break;
        }

        Color originalColor = text.color;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            text.color = Color.Lerp(originalColor, Color.clear, timer / duration);
            yield return null;
        }

        Destroy(textObject);
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
        player.agility = agility;
        player.castingSpeed = castingSpeed;
        player.moveSpeed = moveSpeed;
        player.jumpForce = jumpForce;
        player.knockbackSpeed = knockbackSpeed;
        player.jumpstack = jumpstack;
        player.stance = stance;
        player.gravityMultiplier = gravityMultiplier;
    }


    // 스탯 증가 및 세팅 함수
        public void IncreaseStrength(float amount)
    {
        strength += amount;
        Debug.Log("Strength increased: " + strength);
    }

    public void IncreaseAgility(float amount)
    {
        agility += amount;
        Debug.Log("Agility increased: " + agility);
        UpdateStats();
    }

    public void IncreaseBaseMagic(float amount)
    {
        baseMagic += amount;
        Debug.Log("Base Magic increased: " + magic);
        calculateMagic();
    }

    public void IncreaseCastingSpeed(float amount)
    {
        castingSpeed += amount;
        Debug.Log("Casting increased: " + castingSpeed);
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
        int expToAdd = Mathf.FloorToInt(amount * (1 + expbonus / 100f));
        exp += expToAdd;
        Debug.Log("Gained experience: " + expToAdd + ", Total experience: " + exp);
        StartCoroutine(LevelUpRoutine());
        
        // 이전 코드
        // while (exp >= levelUpExp){ 
        //     level += 1;
        //     exp -= levelUpExp;
        //     uiLevelUp.Show(); // 강화 창 호출
        //     Debug.Log("Level Up: " + level);
    }

    private IEnumerator LevelUpRoutine(){
        while (exp >= levelUpExp){ // 레벨업에 필요한 경험치에 도달하면
            level += 1;
            exp -= levelUpExp;
            uiLevelUp.Show();

            // 강화 창이 닫힐 때까지 기다립니다.
            yield return new WaitUntil(() => !uiLevelUp.IsVisible());
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
        UpdateStats();
    }

    public void SetBaseMagic(float amount)
    {
        baseMagic = amount;
        Debug.Log("Base Magic increased: " + magic);
        calculateMagic();
    }
    public void SetMoveSpeed(float amount)
    {
        moveSpeed = amount;
        Debug.Log("MoveSpeed increased: " + moveSpeed);
        UpdateStats();
    }
    public void SetJumpForce(float amount)
    {
        jumpForce = amount;
        Debug.Log("JumpForce increased: " + jumpForce);
        UpdateStats();
    }

    public void SetMagicPercent(float amount)
    {
        magicPercent = amount;
        Debug.Log("Magic percent is set to: " + magicPercent);
        calculateMagic();
    }

    public void calculateMagic(){
        magic = baseMagic + baseMagic * (magicPercent / 100f);
    }

    // 새함수 추가 --------------------
    // 도현

    public void IncreaseBulletSpeed(float amount){
        bulletSpeed += amount;
    }

    public void IncreaseBulletRange(float amount){
        bulletRange += amount;
    }

    public void IncreaseBulletPass(int amount){
        bulletPass += amount;
    }

    public void IncreaseLifeSteel(float amount){
        lifeSteel += amount;
    }

    public void IncreaseDotDamge(float amount){
        dotDamge += amount;
    }

    public void SetAtkReduction(float amount){
        atkReduction = amount;
    }

    public void SetStunTime(float amount){
        stunTime = amount;
    }

    public void IncreaseCriticalDamage(float amount){
        criticalDamage += amount;
    }

    public void IncreaseArmorPt(float amount){
        armorPt += amount;
    }

    public void IncreaseArmorPtPercent(float amount){
        armorPtPercent += amount;
    }


    //----------------------------------
    // 의섭


    public void IncreaseJumpStack(float amount)
    {
        Debug.Log("jumpstack="+jumpstack);
        jumpstack += amount;
        UpdateStats();
    }

    public void IncreaseResurrection(int amount)
    {
        resurrection += amount;
    }

    public void SetSuperStance()
    {
        stance = 100f;
        UpdateStats();
    }

    public void IncreaseStance(float amount)
    {
        stance += amount;
        UpdateStats();
    }
    public void IncreaseDamageReduce(float amount)
    {
        damagereduce += amount;
    }
    public void IncreaseGoldBonus(float amount)
    {
       goldbonus += amount;
    }
    public void IncreaseExpBonus(float amount)
    {
        expbonus += amount;
    }
    public void IncreaseDropBonus(float amount)
    {
        dropbonus += amount;
    }
    public void IncreaseCriRate(float amount)
    {
        crirate += amount;
    }
    public void IncreaseLuckyShot(float amount)
    {
        luckyshot += amount;
    }

    public void IncreaseShield(float amount)
    {
        shield += amount;
    }
    public float GetStance()
    {
        return stance;
    }
    public float GetJumpstack()
    {
        return jumpstack;
    }

    //------------------------------------

    public void SetGravityMultiplier(float amount){
        gravityMultiplier = amount;
        UpdateStats();
    }

    public Vector3 GetPlayerLoc(){
        return player.transform.position;
    }

}
