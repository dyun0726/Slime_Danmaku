using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private WeaponAttack weaponAttack;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        weaponAttack = GetComponentInParent<WeaponAttack>();
        player = GetComponentInParent<Player>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other) {
        if (weaponAttack != null){
            if (weaponAttack.canDealDamage){
                if (other.gameObject.layer == 9){
                    Enemy enemy = other.GetComponent<Enemy>();

                    if (enemy != null){
                        enemy.TakeDamage(player.strength);
                    } else {
                        Debug.Log("Fail to find Emeny component");
                    }
                    
                }
            }
        }
    }
}
