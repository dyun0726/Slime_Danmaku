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
                //PlayerManager.Instance.IncreaseBaseMagic(damages[level]);
                Debug.Log("Not Implemented");
                break;
            case GrassUpgradeType.speed:
                Debug.Log("Not Implemented");
                break;
            case GrassUpgradeType.jump:
                Debug.Log("Not Implemented");
                break;
            case GrassUpgradeType.jumptime:
                Debug.Log("Not Implemented");
                break;

        }
    }
}
