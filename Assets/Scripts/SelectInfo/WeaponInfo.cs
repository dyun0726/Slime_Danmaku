using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Object/WeaponInfo")]
public class WeaponInfo : ScriptableObject
{
    public string weaponName;
    [TextArea]
    public string description;
    public float strength;
    public float agility;
    public GameObject prefab;
}
