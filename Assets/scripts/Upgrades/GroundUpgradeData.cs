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
                Debug.Log("Not Implemented");
                break;
            case GroundUpgradeType.stance:
                Debug.Log("Not Implemented");
                break;
            case GroundUpgradeType.resurrection:
                Debug.Log("Not Implemented");
                break;
            case GroundUpgradeType.superstance:
                Debug.Log("Not Implemented");
                break;
            case GroundUpgradeType.shield:
                Debug.Log("Not Implemented");
                break;
            case GroundUpgradeType.damagereduce:
                Debug.Log("Not Implemented");
                break;

        }
    }
}
