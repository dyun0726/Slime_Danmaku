using UnityEngine;

[CreateAssetMenu(fileName = "Info", menuName = "Scriptable Object/CharacterInfo")]
public class CharacterInfo : ScriptableObject
{
    [Header("# Character Info")]
    public string characterName;
    [TextArea]
    public string description;
    public Sprite sprite;
    public RuntimeAnimatorController animatorController;

    [Header("# Stats Info")]
    public float maxHealth;
    public float baseMagic;
    public float castingSpeed;
    public float moveSpeed;
    public float jumpForce;
    public float bulletSpeed;
    public float bulletRange;
    public int bulletPass;
    public float lifeSteel;
    public float dotDamge;
    public float atkReduction;
    public float stunTime;
    public float criticalDamage;
    public float armorPt;
    public float armorPtPercent;
    public float jumpstack;
    public int resurrection;
    public int superstance;
    public float stance;
    public float damagereduce;
    public float expbonus;
    public float dropbonus;
    public float goldbonus;
    public float crirate;
    public float luckyshot;
    public float shield;
    public float gravityMultiplier;
}

