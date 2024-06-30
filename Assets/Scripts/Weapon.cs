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

                    Destroy(other.gameObject);
                }
            }
        }
    }
}
