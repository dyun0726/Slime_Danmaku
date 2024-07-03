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
            break;
            case EnhanceData.EhType.Water:
            break;
            case EnhanceData.EhType.Grass:
            break;
            case EnhanceData.EhType.Metal:
            break;
            case EnhanceData.EhType.Ground:
            break;
            case EnhanceData.EhType.Light:
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
