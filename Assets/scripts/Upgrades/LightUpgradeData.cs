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
                Debug.Log("Not Implemented");
                break;
            case LightUpgradeType.dropbonus:
                Debug.Log("Not Implemented");
                break;
            case LightUpgradeType.goldbonus:
                Debug.Log("Not Implemented");
                break;
            case LightUpgradeType.crirate:
                Debug.Log("Not Implemented");
                break;
            case LightUpgradeType.luckyshot:
                Debug.Log("Not Implemented");
                break;

        }
    }
}
