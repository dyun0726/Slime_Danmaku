using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GrassUpgrade", menuName = "Scriptable Object/Upgrade/Grass")]
public class GrassUpgradeData : BaseUpgradeData
{
    public enum GrassUpgradeType { jumpstack, speed, jump, jumptime }
    public GrassUpgradeType grassUpgradeType;

    public override void ApplyUpgrade(int level)
    {
        switch (grassUpgradeType)
        {
            case GrassUpgradeType.jumpstack:
                PlayerManager.Instance.IncreaseJumpStack(level);
                break;
            case GrassUpgradeType.speed:
                PlayerManager.Instance.IncreaseMoveSpeed(damages[level]);
                break;
            case GrassUpgradeType.jump:
                PlayerManager.Instance.IncreaseJumpForce(damages[level]);
                break;
            case GrassUpgradeType.jumptime:
                Debug.Log("Not Implemented");
                break;
                }
    }
}
