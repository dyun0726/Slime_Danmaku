using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LangedController : MonoBehaviour
{

    public void shootBullet(bool facingRight){
        GameObject playerBulletGO = PoolManager.instance.GetGO("PlayerBullet");
        PlayerBullet bullet = playerBulletGO.GetComponent<PlayerBullet>();
        bullet.Dir = facingRight ? Vector2.right : Vector2.left;
        bullet.Speed = PlayerManager.Instance.bulletSpeed;
        bullet.Range = PlayerManager.Instance.bulletRange;
        bullet.PassCount = PlayerManager.Instance.bulletPass;
        bullet.LifeSteel = PlayerManager.Instance.lifeSteel;
        bullet.StartPos = new Vector2(transform.position.x, transform.position.y);
        bullet.Damage = PlayerManager.Instance.magic;
        // 후에 치명타 확률 구현 장소 - 치명타 데미지 까지 연계
        bullet.CriRate = PlayerManager.Instance.crirate;
        bullet.CriDamage = PlayerManager.Instance.criticalDamage;
        bullet.LuckyShot = PlayerManager.Instance.luckyshot;
        bullet.DotDamage = PlayerManager.Instance.dotDamge;
        bullet.AtkReduction = PlayerManager.Instance.atkReduction;
        bullet.ArmorPt = PlayerManager.Instance.armorPt;
        bullet.ArmorPtPercent = PlayerManager.Instance.armorPtPercent;
        playerBulletGO.transform.position = transform.position;
        if (!facingRight){
            playerBulletGO.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        } else {
            playerBulletGO.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
        
    }
}
