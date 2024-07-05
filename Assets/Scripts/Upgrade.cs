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

    private void Awake() {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.icon;

        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        textLevel = texts[0];
    }

    private void LateUpdate() {
        textLevel.text = "Lv. " + level;
    }

    public void OnClick(){
        data.ApplyUpgrade(level);
        level ++;
        if (level == data.damages.Length){
            GetComponent<Button>().interactable = false;
        }
    }
}
