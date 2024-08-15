using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private LevelUp uiLevelUp;

    // 경험치 관련 변수 (초기화 필요)
    public int gold = 0; // 플레이어의 골드
    public int exp = 0; // 플레이어의 경험치
    public int levelUpExp = 10; // 레벨업에 필요한 경험치
    public int level = 1;

    // 캐릭터 스탯
    public float maxHealth = 100f; // 최대 체력
    public float currentHealth = 100f; // 현재 체력
    public float strength = 10f; // 힘
    public float agility = 10f; // 민첩성 (10초에 x번)
    public float baseMagic = 10f; // 마력
    public float magicPercent = 0f; // 마력 퍼센트 계산
    public float magic; // 총 계산 마력
    public float castingSpeed = 10; // 마법 시전 속도 (10초에 x번)
    public float moveSpeed = 3f; // 이동 속도
    public float jumpForce = 8f;
    public float knockbackSpeed = 10f;
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
    public float gravityMultiplier = 0f; // 중력 감소, (0 ~ 100%)
    
    // 타입 강화 횟수 변수 - (불 물 풀 땅 강철 빛 어둠)
    public bool[] typeStacks = new bool[7];
    public int meleeStack = 0;

    // 죽음 시 비활성화할 오브젝트 리스트
    public List<GameObject> objectsToRemove;

    private void Awake() {
        if (_instance == null){
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this){
            Destroy(gameObject);
        }
       

        //테스트용 수치들
        // shield = 20f;
        //damagereduce = 20f;
        //resurrection = 0;
        //expbonus = 50f;
        //goldbonus = 50f;
        //crirate = 50f;
    }

    // Start is called before the first frame update
    void Start()
    {
        calculateMagic();
        if (uiLevelUp == null) {
            uiLevelUp = FindObjectOfType<LevelUp>();
        }
    }

    public void TakeDamage(float amount, Vector2 dir)
    {
        if (Player.Instance.isInvincible){
            Debug.Log("Damaged canceled");
            return;
        }

        // 0.5초 동안 무적 설정
        Player.Instance.SetInvincible();
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
        Player.Instance.Knockback(dir);
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
        SaveData();

        if (DeathCanvas.Instance != null)
        {
            Debug.Log("DeathCanvas active!");
            DeathCanvas.Instance.gameObject.SetActive(true);
        }

        if (Player.Instance != null)
        {
            Player.Instance.DeactivatePlayer();
        }

        // 있던 탄막 모두 제거
        PoolManager.instance.DisableAllObjects();

        // 죽었을시 게임 시간 멈추기
        GameManager.Instance.Stop();
        foreach (GameObject obj in objectsToRemove)
        {
            obj.SetActive(false);
        }

    }

    // 데이터 저장 함수
    public void SaveData()
    {
        SaveGold();
        SaveDeathCount();
        SaveKillCount();
        SaveWorldClear();
        SaveMelee();
        PlayerPrefs.Save();
    }

    // 골드 데이터 저장 함수
    private void SaveGold()
    {
        // 얻은 골드 저장
        int currentGold = PlayerPrefs.GetInt("Gold", 0) + gold;
        PlayerPrefs.SetInt("Gold", currentGold);

        // 누적 골드 저장
        int totalGold = PlayerPrefs.GetInt("TotalGold", 0) + gold;
        PlayerPrefs.SetInt("TotalGold", totalGold);

    }

    // 죽음 데이터 저장 함수
    private void SaveDeathCount()
    {
        // 총 죽음 수 저장
        int totalDeath = PlayerPrefs.GetInt("Death", 0) + 1;
        PlayerPrefs.SetInt("Death", totalDeath);

        // 그린 슬라임(1) 해금 확인
        if (PlayerPrefs.GetInt("CharacterUnlocked_1", 0) == 0 && totalDeath >= 1)
        {
            PlayerPrefs.SetInt("CharacterUnlocked_1", 1);
        }

        // 다이아 슬라임(3) 해금 확인
        if (PlayerPrefs.GetInt("CharacterUnlocked_3", 0) == 0 && totalDeath >= 10)
        {
            PlayerPrefs.SetInt("CharacterUnlocked_3", 1);
        }
    }

    // 죽인 적 개수 저장 함수
    private void SaveKillCount()
    {
        // 총 킬 수 저장
        int totalKilled = PlayerPrefs.GetInt("Killed", 0) + GameManager.Instance.killed;
        PlayerPrefs.GetInt("Killed", totalKilled);

        // 레드 슬라임(2) 해금 확인
        if (PlayerPrefs.GetInt("CharacterUnlocked_2", 0) == 0 && totalKilled >= 10)
        {
            PlayerPrefs.SetInt("CharacterUnlocked_2", 1);
        }
    }

    // 월드 클리어 저장 함수
    private void SaveWorldClear()
    {
        // 월드 1 클리어
        if (GameManager.Instance.clearStage1)
        {
            PlayerPrefs.SetInt("Stage1", PlayerPrefs.GetInt("Stage1", 0) + 1);
        }
        
        // 월드 2 클리어
        if (GameManager.Instance.clearStage2)
        {
            int stage2 = PlayerPrefs.GetInt("Stage2", 0) + 1;
            PlayerPrefs.SetInt("Stage2", stage2);
            if (PlayerPrefs.GetInt("CharacterUnlocked_4", 0) == 0) 
            {
                PlayerPrefs.SetInt("CharacterUnlocked_4", 1);
            }
        }

        // 월드 3 클리어
        if (GameManager.Instance.clearStage3)
        {
            int stage3 = PlayerPrefs.GetInt("Stage3", 0) + 1;
            PlayerPrefs.SetInt("Stage3", stage3);
            if (PlayerPrefs.GetInt("CharacterUnlocked_5", 0) == 0) 
            {
                PlayerPrefs.SetInt("CharacterUnlocked_5", 1);
            }
        }

        // 월드 4 클리어 (게임 클리어)
        if (GameManager.Instance.gameClear)
        {
            int clear = PlayerPrefs.GetInt("Clear", 0) + 1;
            PlayerPrefs.SetInt("Clear", clear);
            if (PlayerPrefs.GetInt("CharacterUnlocked_6", 0) == 0) 
            {
                PlayerPrefs.SetInt("CharacterUnlocked_6", 1);
            }
        }
    }

    private void SaveMelee()
    {
        // 총 근접 공격 수 저장
        int totalMelee = PlayerPrefs.GetInt("Melee", 0) + meleeStack;
        PlayerPrefs.GetInt("Melee", totalMelee);

        // 레드 슬라임(2) 해금 확인
        if (PlayerPrefs.GetInt("WeaponUnlocked_2", 0) == 0 && totalMelee >= 100)
        {
            PlayerPrefs.SetInt("WeaponUnlocked_2", 1);
            PlayerPrefs.SetInt("WeaponUnlocked_3", 1);
        }
    }

    // 골드 추가 함수
    public void AddGold(int amount)
    {
        int goldToAdd = Mathf.FloorToInt(amount * (1 + goldbonus / 100f));
        gold += goldToAdd;
    }

    // 스탯 동기화 함수
    public void UpdateStats(){
        Player.Instance.agility = agility;
        Player.Instance.castingSpeed = castingSpeed;
        Player.Instance.moveSpeed = moveSpeed;
        Player.Instance.jumpForce = jumpForce;
        Player.Instance.knockbackSpeed = knockbackSpeed;
        Player.Instance.jumpstack = jumpstack;
        Player.Instance.stance = stance;
        Player.Instance.gravityMultiplier = gravityMultiplier;
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
    }

    public void IncreaseExp(int amount)
    {
        int expToAdd = Mathf.FloorToInt(amount * (1 + expbonus / 100f));
        exp += expToAdd;
        Debug.Log("Gained experience: " + expToAdd + ", Total experience: " + exp);
        StartCoroutine(LevelUpRoutine());
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

    public void IncreaseMagicPercent(float amount)
    {
        magicPercent += amount;
        Debug.Log("Magic percent is set to: " + magicPercent);
        calculateMagic();
    }

    public void calculateMagic(){
        magic = baseMagic * (1 + (magicPercent / 100f));
    }

    // 도현 추가 --------------------
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
    // 의섭 ---------------------------------- 
    public void IncreaseJumpStack(float amount)
    {
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

    public void SetGravityMultiplier(float amount){
        gravityMultiplier = amount;
        UpdateStats();
    }

    public void ApplyStack(int index)
    {
        switch (index)
        {
            // 불 속성: 최종 데미지 20%
            case 0:
                IncreaseMagicPercent(20);
                break;
            // 물 속성: 탄막 속도 (2) 및 사거리 증가 (5)
            case 1:
                IncreaseBulletSpeed(2);
                IncreaseBulletRange(5);
                break;
            // 풀 속성: 이동 속도 (1.5) 및 점프력 증가 (1.5)
            case 2:
                IncreaseMoveSpeed(1.5f);
                IncreaseJumpForce(1.5f);
                break;
            // 땅 속성: 최대 체력 50 증가
            case 3:
                IncreaseMaxHealth(50);
                currentHealth += 50;
                break;
            // 강철 속성: 방어력 관통 20% 추가
            case 4:
                IncreaseArmorPtPercent(20);
                break;
            // 빛 속성: 골드 획득 및 경험치 획득 50% 증가
            case 5:
                IncreaseGoldBonus(50);
                IncreaseExpBonus(50);
                break;
            // 어둠 속성: 크리티컬 데미지 30% 증가
            case 6:
                IncreaseCriticalDamage(30);
                break;
        }
        calculateMagic();
        UpdateStats();
    }

    public void SetPlayerAllStats(CharacterInfo characterInfo, WeaponInfo weaponInfo){
        maxHealth = characterInfo.maxHealth + PlayerPrefs.GetFloat("HpBonus", 0); ;
        currentHealth = characterInfo.maxHealth + PlayerPrefs.GetFloat("HpBonus", 0); // 최대 체력으로 초기화
        baseMagic = characterInfo.baseMagic + PlayerPrefs.GetFloat("MagicBonus", 0);
        castingSpeed = characterInfo.castingSpeed;
        moveSpeed = characterInfo.moveSpeed + PlayerPrefs.GetFloat("SpeedBonus", 0);
        jumpForce = characterInfo.jumpForce + PlayerPrefs.GetFloat("JumpBonus", 0);
        bulletSpeed = characterInfo.bulletSpeed;
        bulletRange = characterInfo.bulletRange;
        bulletPass = characterInfo.bulletPass;
        lifeSteel = characterInfo.lifeSteel;
        dotDamge = characterInfo.dotDamge;
        atkReduction = characterInfo.atkReduction;
        stunTime = characterInfo.stunTime;
        criticalDamage = characterInfo.criticalDamage;
        armorPt = characterInfo.armorPt;
        armorPtPercent = characterInfo.armorPtPercent;
        jumpstack = characterInfo.jumpstack;
        resurrection = characterInfo.resurrection;
        superstance = characterInfo.superstance;
        stance = characterInfo.stance;
        damagereduce = characterInfo.damagereduce;
        expbonus = characterInfo.expbonus + PlayerPrefs.GetFloat("ExpBonus", 0);
        dropbonus = characterInfo.dropbonus;
        goldbonus = characterInfo.goldbonus + PlayerPrefs.GetFloat("GoldBonus", 0);
        crirate = characterInfo.crirate;
        luckyshot = characterInfo.luckyshot;
        shield = characterInfo.shield;
        gravityMultiplier = characterInfo.gravityMultiplier;

        strength = weaponInfo.strength;
        agility = weaponInfo.agility;

        calculateMagic();
        UpdateStats();
    }

    public void FindCanvas()
    {
        uiLevelUp = FindObjectOfType<LevelUp>();
    }

    // 초기화
    public void Init()
    {
        gold = 0;
        exp = 0;
        levelUpExp = 10;
        level = 1;
    }

}
