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

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == 9){
            Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null){
                    enemy.TakeDamage(Damage);

                    if (dotDamge > 0){ // dot damge가 활성화되어있으면
                        enemy.SetDotDamage(dotDamge);
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
