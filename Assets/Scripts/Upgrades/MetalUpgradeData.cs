using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MetalUpgrade", menuName = "Scriptable Object/Upgrade/Metal")]
public class MetalUpgradeData : BaseUpgradeData
{
    public enum MetalUpgradeType {Strength, Agility, ArmorPenetration, ArmorPenetrationPercent}
    public MetalUpgradeType metalUpgradeType;

    public override void ApplyUpgrade(int level)
    {
        switch (metalUpgradeType) {
            case MetalUpgradeType.Strength:
                PlayerManager.Instance.IncreaseStrength(damages[level]);
                break;
            case MetalUpgradeType.Agility:
                PlayerManager.Instance.IncreaseAgility(damages[level]);
                break;
            case MetalUpgradeType.ArmorPenetration:
                PlayerManager.Instance.IncreaseArmorPt(damages[level]);
                break;
            case MetalUpgradeType.ArmorPenetrationPercent:
                PlayerManager.Instance.IncreaseArmorPtPercent(damages[level]);
                break;


        }
    }

}
