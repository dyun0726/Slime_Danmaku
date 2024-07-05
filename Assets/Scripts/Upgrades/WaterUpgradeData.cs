using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaterUpgrade", menuName = "Scriptable Object/Upgrade/Water")]
public class WaterUpgradeData : BaseUpgradeData
{
    public enum WaterUpgradeType {bulletSpeed, bulletRange, bulletPass, lifeSteel}
    public WaterUpgradeType waterUpgradeType;

    public override void ApplyUpgrade(int level)
    {
        switch (waterUpgradeType){
            case WaterUpgradeType.bulletSpeed:
                PlayerManager.Instance.IncreaseBulletSpeed(damages[level]);
                break;
            case WaterUpgradeType.bulletRange:
                PlayerManager.Instance.IncreaseBulletRange(damages[level]);
                break;
            case WaterUpgradeType.bulletPass:
                PlayerManager.Instance.IncreaseBulletPass((int) damages[level]);
                break;
            case WaterUpgradeType.lifeSteel:
                PlayerManager.Instance.IncreaseLifeSteel(damages[level]);
                break;
        }
    }
}
