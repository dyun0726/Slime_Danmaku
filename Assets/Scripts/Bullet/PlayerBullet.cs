using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    private int passCount = 0;
    public int PassCount {get {return passCount;} set {passCount = value;}}

    private float lifeSteel = 0;
    public float LifeSteel {get {return lifeSteel;} set {lifeSteel = value;}}

    private float dotDamge = 0;
    public float DotDamage {get {return dotDamge;} set {dotDamge = value;}}

    private float atkReduction = 0;
    public float AtkReduction {get {return atkReduction;} set {atkReduction = value;}}

    private float armorPt = 0;
    public float ArmorPt {get {return armorPt;} set {armorPt = value;}}

    private float armorPtPerecnt = 0;
    public float ArmorPtPercent {get {return armorPtPerecnt;} set {armorPtPerecnt = value;}}

    private float criRate = 0;
    public float CriRate {get {return criRate;} set {criRate = value;}}

    private float criDamage = 0;
    public float CriDamage {get {return criDamage;} set {criDamage = value;}}

    private float luckyShot = 0;
    public float LuckyShot {get {return luckyShot;} set {luckyShot = value;}}


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == 9 || other.gameObject.layer == 12){
            bool isBoss = other.gameObject.layer == 12;
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                // luckyshot 여부 결정 (보스가 아닌 경우에만 적용)
                bool isLuckyShot = (!isBoss) && Random.value < (luckyShot / 100f);
                if (isLuckyShot)
                {
                    // 즉사 처리
                    enemy.Die();
                }
                else
                {
                    bool isCritical = Random.value < (criRate / 100f);
                    // 치명타 데미지 계산
                    float nDamage = Damage;
                    if (isCritical)
                    {
                        Debug.Log("critical!");
                        nDamage *= 1 + criDamage / 100f;
                    }
                    
                    Debug.Log(nDamage);
                    enemy.TakeDamage(nDamage, armorPt, armorPtPerecnt, true);

                    if (dotDamge > 0){ // dot damge가 활성화되어있으면
                        enemy.SetDotDamage(dotDamge);
                    }

                    if (atkReduction > 0) { // 공격 감소 디버프 탄막이면
                        enemy.SetAttackReduce(3f, atkReduction);
                    }

                    if (lifeSteel > 0){
                        PlayerManager.Instance.Heal(lifeSteel);
                    }
                }
            } 
            else
            {
                Debug.Log("Fail to find Emeny component");
            }

            if (passCount == 0){
                ReleaseObject();
            } else if (passCount > 0) { // 관통 가능하면
                passCount--;
            } else { // 오류 발생
                Debug.LogError("passCount cannot be negative");
            }
        }
        else if (other.gameObject.layer == 6)
        {
            ReleaseObject();
        }
    }
}
