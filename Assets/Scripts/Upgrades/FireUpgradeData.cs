using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireUpgrade", menuName = "Scriptable Object/Upgrade/Fire")]
public class FireUpgradeData : BaseUpgradeData
{
    public enum FireUpgradeType {baseAttack, percentAttack, casting}
    public FireUpgradeType fireUpgradeType;

    public override void ApplyUpgrade(int level)
    {
        switch (fireUpgradeType)
        {
            case FireUpgradeType.baseAttack:
                PlayerManager.Instance.IncreaseBaseMagic(damages[level]);
                break;
            case FireUpgradeType.percentAttack:
                PlayerManager.Instance.IncreaseMagicPercent(damages[level]);
                break;
            case FireUpgradeType.casting:
                PlayerManager.Instance.IncreaseCastingSpeed(damages[level]);
                break;

        }
    }
}
