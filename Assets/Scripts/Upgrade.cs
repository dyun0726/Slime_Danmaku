using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public BaseUpgradeData data;
    public int level;

    Image icon;
    TextMeshProUGUI textLevel;
    TextMeshProUGUI textName;
    TextMeshProUGUI textDesc;

    private void Awake() {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.icon;

        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        textName.text = data.upgradeName;

    }

    private void OnEnable() {
        textLevel.text = "Lv." + level;
        textDesc.text = string.Format(data.description, data.damages[level]);
    }



    public void OnClick(){
        data.ApplyUpgrade(level);
        level ++;
        if (level == data.damages.Length){
            GetComponent<Button>().interactable = false;
        }
    }
}
