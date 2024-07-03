using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 강화 데이터
[CreateAssetMenu(fileName = "Enhance", menuName = "Scriptable Object/EnhanceData")]
public class EnhanceData : ScriptableObject
{
    public enum EhType { Fire, Water, Grass, Metal, Ground, Light, Dark}
    [Header("# Main Info")]
    public EhType ehType;
    public int ehId;
    public string ehName;
    public string ehDesc;
    public Sprite ehIcon;

    [Header("# Level Data")]
    public float baseDamage;
    public int baseCount;
    public float[] damages;
    public int[] counts;

    [Header("# Weapon")]
    public GameObject projectile;
}
