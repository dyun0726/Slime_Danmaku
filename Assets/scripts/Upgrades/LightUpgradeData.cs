using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LightUpgrade", menuName = "Scriptable Object/Upgrade/Light")]
public class LightUpgradeData : BaseUpgradeData
{
    public enum LightUpgradeType { expbonus, dropbonus, goldbonus, crirate, luckyshot }
    public LightUpgradeType lightUpgradeType;

    public override void ApplyUpgrade(int level)
    {
        switch (lightUpgradeType)
        {
            case LightUpgradeType.expbonus:
                PlayerManager.Instance.IncreaseExpBonus(damages[level]);
                break;
            case LightUpgradeType.dropbonus:
                PlayerManager.Instance.IncreaseDropBonus(damages[level]);
                break;
            case LightUpgradeType.goldbonus:
                PlayerManager.Instance.IncreaseGoldBonus(damages[level]);
                break;
            case LightUpgradeType.crirate:
                PlayerManager.Instance.IncreaseCriRate(damages[level]);
                break;
            case LightUpgradeType.luckyshot:
                PlayerManager.Instance.IncreaseLuckyShot(damages[level]);
                break;

        }
    }
}
