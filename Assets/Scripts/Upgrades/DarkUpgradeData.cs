using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DarkUpgrade", menuName = "Scriptable Object/Upgrade/Dark")]
public class DarkUpgradeData : BaseUpgradeData
{
    public enum DarkUpgradeType {DotDamage, AttackReduction, DefenseReduction, Stun}

    public DarkUpgradeType darkUpgradeType;

    public override void ApplyUpgrade(int level)
    {
        switch(darkUpgradeType){
            case DarkUpgradeType.DotDamage:
                PlayerManager.Instance.IncreaseDotDamge(damages[level]);
                break;
            case DarkUpgradeType.AttackReduction:

                break;
            case DarkUpgradeType.DefenseReduction:

                break;
            case DarkUpgradeType.Stun:

                break;

        }
    }
}
