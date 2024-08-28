using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private WeaponAttack weaponAttack;
    // Start is called before the first frame update
    void Start()
    {
        weaponAttack = GetComponentInParent<WeaponAttack>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other) {
        if (weaponAttack != null){
            if (weaponAttack.canDealDamage){
                if (other.gameObject.layer == 9 || other.gameObject.layer == 12){
                    Enemy enemy = other.GetComponent<Enemy>();

                    if (enemy != null)
                    {
                        bool isCritical = Random.value < (PlayerManager.Instance.crirate / 100f);
                        float nDamage = PlayerManager.Instance.strength;
                        if (isCritical)
                        {
                            Debug.Log("critical!");
                            nDamage *= 1 + PlayerManager.Instance.criticalDamage / 100f;
                        }
                        enemy.TakeDamage(nDamage, PlayerManager.Instance.armorPt, PlayerManager.Instance.armorPtPercent, true);

                        if (PlayerManager.Instance.dotDamge > 0)
                        { // dot damge가 활성화되어있으면
                            enemy.SetDotDamage(PlayerManager.Instance.dotDamge);
                        }

                        if (PlayerManager.Instance.atkReduction > 0)
                        { // 공격 감소 디버프 탄막이면
                            enemy.SetAttackReduce(3f, PlayerManager.Instance.atkReduction);
                        }

                        if (PlayerManager.Instance.lifeSteel > 0)
                        {
                            PlayerManager.Instance.Heal(PlayerManager.Instance.lifeSteel);
                        }
                    } 
                    else
                    {
                        Debug.Log("Fail to find Emeny component");
                    }
                    
                }
            }
        }
    }
}
