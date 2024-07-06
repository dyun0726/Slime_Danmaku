using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DarkUpgrade", menuName = "Scriptable Object/Upgrade/Dark")]
public class DarkUpgradeData : BaseUpgradeData
{
    public enum DarkUpgradeType {DotDamage, AttackReduction, Stun, CriticalDamage}

    public DarkUpgradeType darkUpgradeType;

    public override void ApplyUpgrade(int level)
    {
        switch(darkUpgradeType){
            case DarkUpgradeType.DotDamage:
                PlayerManager.Instance.IncreaseDotDamge(damages[level]);
                break;
            case DarkUpgradeType.AttackReduction:
                PlayerManager.Instance.SetAtkReduction(damages[level]);
                break;
            case DarkUpgradeType.Stun:
                PlayerManager.Instance.SetStunTime(damages[level]);
                break;
            case DarkUpgradeType.CriticalDamage:
                PlayerManager.Instance.IncreaseCriticalDamage(damages[level]);
                break;

        }
    }
}
