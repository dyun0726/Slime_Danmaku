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

    private float stunTime = 0;
    public float StunTime {get {return stunTime;} set {stunTime = value;}}

    private float armorPt = 0;
    public float ArmorPt {get {return armorPt;} set {armorPt = value;}}

    private float armorPtPerecnt = 0;
    public float ArmorPtPercent {get {return armorPtPerecnt;} set {armorPtPerecnt = value;}}


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == 9){
            Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null){
                    enemy.TakeDamage(Damage, armorPt, armorPtPerecnt);

                    if (dotDamge > 0){ // dot damge가 활성화되어있으면
                        enemy.SetDotDamage(dotDamge);
                    }

                    if (atkReduction > 0) { // 공격 감소 디버프 탄막이면
                        enemy.SetAttackReduce(3f, atkReduction);
                    }

                    if (stunTime > 0) {
                        enemy.SetStun(stunTime);
                    }

                    if (lifeSteel > 0){
                        PlayerManager.Instance.Heal(lifeSteel);
                    }

                    

                } else {
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
    }
}
