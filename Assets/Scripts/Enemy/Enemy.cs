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
    // public float luckyshot; // 즉사 확률 (0에서 100 사이의 값)

    // 도트 데미지 관련 변수
    private int dotCount = 0; // 남은 도트 카운트 수
    private float dotDamge;

    // 공격 감소 디버프 관련 변수
    public bool isAtkReduced = false;
    public float atkReduction = 0;
    protected float atkReductionTimer;

    // 스턴 설정 변수
    public bool isStuned = false;
    protected float stunTimer;

    // 살아있는지
    protected bool isDead = false;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected Collider2D coll;
    protected Rigidbody2D rb;
    public float potionDropChance = 80f;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        enemyManager = FindObjectOfType<EnemyManager>();
        isDead = false;
    }

    private void Update()
    {
        // 시간이 멈춰있거나 이 오브젝트가 죽은 상태면 return
        if (!GameManager.Instance.isLive || isDead)
        {
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
            PlayerManager.Instance.TakeDamage(GetDamage(), dir);
        }
    }

    public void TakeDamage(float damage, float armorPt, float armorPtPercent, bool animatorPlay)
    {
        float calArmor = (armor - armorPt) * (1f - armorPtPercent / 100f);
        calArmor = Mathf.Max(calArmor, 0); // calArmor가 0보다 작지 않도록 설정
        float finalDamage = damage - calArmor;
        finalDamage = Mathf.Max(finalDamage, 0); // finalDamage가 0보다 작지 않도록 설정
        health -= finalDamage;

        if (health <= 0)
        {
            // 죽는 애니메이션 구현되어 있다면 실행 Die 함수는 애니메이션서 실행
            if (HasParameter("Dead"))
            {
                isDead = true;
                coll.enabled = false;
                rb.simulated = false;
                spriteRenderer.sortingOrder = 0;
                animator.SetTrigger("Dead");
            }
            // 구현 안되어 있으면 바로 die 실행
            else 
            {
                Die();
            }
            
        }
        else
        {
            // Debug.Log("test");
            if (animatorPlay)
            {
                animator.SetTrigger("Hurt");
            }
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
            TakeDamage(dotDamge, 0f, 1f, false);
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
        Vector3 dropPosition = transform.position + new Vector3(0, 0, 0);
        enemyManager.DropPotion(dropPosition); 
        Destroy(gameObject);
        PlayerManager.Instance.IncreaseExp(exp);
        PlayerManager.Instance.AddGold(gold);
    }

  

    // 데미지 반환 함수 (공격 감소가 있으면 적용된)
    public float GetDamage(){
        return isAtkReduced ? damage * (1 - atkReduction / 100f) : damage;
    }

    // 애니메이션에 name 이름을 가진 파라미터 존재하는지 확인하는 함수
    private bool HasParameter(string name)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == name)
            {
                return true;
            }
        }
        return false;
    }
}
