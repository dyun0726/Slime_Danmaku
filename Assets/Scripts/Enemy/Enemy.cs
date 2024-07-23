using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyManager enemyManager;
    public float health = 20;
    public int exp = 15; // 죽였을때 주는 경험치
    public int gold = 15;
    public float damage = 10; // 몸박 데미지
    public float armor = 5; // 방어력

    //즉사 관련 변수
    public float luckyshot; // 즉사 확률 (0에서 100 사이의 값)

    // 도트 데미지 관련 변수
    public int dotCount = 0; // 남은 도트 카운트 수
    public float dotDamge;

    // 공격 감소 디버프 관련 변수
    public bool isAtkReduced = false;
    public float atkReduction = 0;
    protected float atkReductionTimer;

    // 스턴 설정 변수
    public bool isStuned = false;
    protected float stunTimer;

    protected SpriteRenderer spriteRenderer;
    protected Animator animator;

    
    public GameObject PotionPrefab;
    public float potionDropChance = 80f;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        enemyManager = FindObjectOfType<EnemyManager>();
    }

    private void Update()
    {
        if (!GameManager.Instance.isLive)
        {  // live 체크 함수
            return;
        }

        if (isAtkReduced)
        {
            atkReductionTimer -= Time.deltaTime;
            if (atkReductionTimer < 0)
            {
                isAtkReduced = false;
                atkReduction = 0;
            }
        }

        if (isStuned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer < 0)
            {
                isStuned = false;
            }
        }

        spriteRenderer.flipX = PlayerManager.Instance.GetPlayerLoc().x < transform.position.x;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 10)
        { // 플레이어와 충돌하면
            Vector2 dir = (other.transform.position - transform.position).normalized;
            float newDamage = isAtkReduced ? damage * (1 - atkReduction) : damage;
            PlayerManager.Instance.TakeDamage(newDamage, dir);
        }
    }

    public void TakeDamage(float damage, float armorPt, float armorPtPercent)
    {
        float calArmor = (armor - armorPt) * (1f - armorPtPercent / 100f);
        calArmor = Mathf.Max(calArmor, 0); // calArmor가 0보다 작지 않도록 설정
        float finalDamage = damage - calArmor;
        finalDamage = Mathf.Max(finalDamage, 0); // finalDamage가 0보다 작지 않도록 설정

        health -= finalDamage;

        if (health <= 0)
        {
            Die();
        }
        else
        {
            Debug.Log("test");
            animator.SetTrigger("Hurt");
        }
    }

    public void SetDotDamage(float amount)
    {
        dotDamge = amount;
        if (dotCount > 0)
        { // 이미 독데미지를 받고 있다면 count를 초기화
            dotCount = 3;
        }
        else if (dotCount == 0)
        {
            dotCount = 3;
            StartCoroutine(TakeDotDamage());
        }
    }

    IEnumerator TakeDotDamage()
    {
        while (dotCount > 0)
        {
            yield return new WaitForSeconds(1f);
            dotCount--;
            TakeDamage(dotDamge, 0f, 1f);
            Debug.Log("enemy health: " + health);
        }
    }

    public void SetAttackReduce(float time, float amount)
    {
        isAtkReduced = true;
        atkReductionTimer = time;
        atkReduction = amount;
    }

    public void SetStun(float time)
    {
        isStuned = true;
        stunTimer = time;
    }

    public void Die()
    {
        enemyManager.EnemyDefeated();
        DropPotion(); 
        Destroy(gameObject);
        PlayerManager.Instance.IncreaseExp(exp);
        PlayerManager.Instance.AddGold(gold);
    }

    // 하트 아이템 드롭 함수
    private void DropPotion()
    {
        float dropChance = Random.Range(0f, 100f);
        float totalDropChance = potionDropChance * (100f + PlayerManager.Instance.dropbonus) / 100f ;
        if (dropChance < totalDropChance)
        {
            Vector3 dropPosition = transform.position + new Vector3(0, 1f, 0);
            Instantiate(PotionPrefab, transform.position, Quaternion.identity);
        }
    }
}
