using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GroundUpgrade", menuName = "Scriptable Object/Upgrade/Ground")]

public class GroundUpgradeData : BaseUpgradeData
{
    public enum GroundUpgradeType { maxhealth, stance,resurrection, superstance, shield, damagereduce}
    public GroundUpgradeType groundUpgradeType;

    public override void ApplyUpgrade(int level)
    {
        switch (groundUpgradeType)
        {
            case GroundUpgradeType.maxhealth:
                PlayerManager.Instance.IncreaseMaxHealth(damages[level]);
                break;
            case GroundUpgradeType.stance:
                PlayerManager.Instance.IncreaseStance(damages[level]);
                break;
            case GroundUpgradeType.resurrection:
                PlayerManager.Instance.IncreaseResurrection(level);
                break;
            case GroundUpgradeType.superstance:
                PlayerManager.Instance.SetSuperStance();
                break;
            case GroundUpgradeType.shield:
                PlayerManager.Instance.IncreaseShield(damages[level]);
                break;
            case GroundUpgradeType.damagereduce:
                PlayerManager.Instance.IncreaseDamageReduce(damages[level]);
                break;

        }
    }
}
