using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Enhance : MonoBehaviour
{
    public EnhanceData data;
    public int level;

    Image icon;
    TextMeshProUGUI textLevel;

    
    private void Awake() {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.ehIcon;

        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        textLevel = texts[0];
    }

    private void LateUpdate() {
        textLevel.text = "Lv. " + level;
    }

    public void OnClick(){
        switch (data.ehType){
            case EnhanceData.EhType.Fire:
                // 마력
                PlayerManager.Instance.IncreaseMagic(data.damages[level]);
                break;
            case EnhanceData.EhType.Water:
                // 시전 속도
                PlayerManager.Instance.IncreaseCastingSpeed(data.damages[level]);
                break;
            case EnhanceData.EhType.Grass:
                // 공속
                PlayerManager.Instance.IncreaseAgility(data.damages[level]);
                break;
            case EnhanceData.EhType.Metal:
                // 힘
                PlayerManager.Instance.IncreaseStrength(data.damages[level]);
                break;
            case EnhanceData.EhType.Ground:
                // 체력
                PlayerManager.Instance.IncreaseMaxHealth(data.damages[level]);
                break;
            case EnhanceData.EhType.Light:
                // 이동속도
                PlayerManager.Instance.IncreaseMoveSpeed(data.damages[level]);
                break;
            case EnhanceData.EhType.Dark:
                break;
        }

        level ++;
        if (level == data.damages.Length){
            GetComponent<Button>().interactable = false;
        }
    }

}
