using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUpgradeData : ScriptableObject
{
    public enum UpgradeType {Fire, Water, Grass, Metal, Ground, Light, Dark}

    [Header("# Base Info")]
    public string upgradeName;
    public string description;
    public Sprite icon;

    [Header("# Level Data")]
    public float[] damages;

    [Header("# Type Data")]
    public UpgradeType upgradeType;
    
    public abstract void ApplyUpgrade(int level);

}
