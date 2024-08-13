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
                if (other.gameObject.layer == 9){
                    Enemy enemy = other.GetComponent<Enemy>();

                    if (enemy != null){
                        enemy.TakeDamage(PlayerManager.Instance.strength, PlayerManager.Instance.armorPt, PlayerManager.Instance.armorPtPercent, true);
                    } else {
                        Debug.Log("Fail to find Emeny component");
                    }
                    
                }
            }
        }
    }
}
