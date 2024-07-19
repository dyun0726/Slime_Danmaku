using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth;
    public float AGI;
    public float INT;
    public float SPEED;

    // CharacterInfo를 기반으로 스탯 설정
    public void SetCharacterInfo(CharacterInfo characterInfo)
    {
        maxHealth = characterInfo.maxHealth;
        AGI = characterInfo.agility;
        INT = characterInfo.baseMagic;
        SPEED = characterInfo.moveSpeed;
    }
}
