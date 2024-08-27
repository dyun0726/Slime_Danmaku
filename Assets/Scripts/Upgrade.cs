using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public BaseUpgradeData data;
    public int level;
    public Stack stackUI;
    private Button button;

    Image icon;
    TextMeshProUGUI textLevel;
    TextMeshProUGUI textName;
    TextMeshProUGUI textDesc;

    

    private void Awake() {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.icon;

        button = GetComponent<Button>();

        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        textName.text = data.upgradeName;

        gameObject.SetActive(false);
    }

    private void OnEnable() {
        textLevel.text = "Lv." + level;
        if (data.damages.Length != 0){
            textDesc.text = string.Format(data.description, data.damages[level]);
        } else {
            textDesc.text = data.description;
        }
        
    }



    public void OnClick(){
        data.ApplyUpgrade(level);
        level ++;
        switch (data.upgradeType){
            case BaseUpgradeData.UpgradeType.Fire:
                stackUI.StackUp(0);
                break;
            case BaseUpgradeData.UpgradeType.Water:
                stackUI.StackUp(1);
                break;
            case BaseUpgradeData.UpgradeType.Grass:
                stackUI.StackUp(2);
                break;
            case BaseUpgradeData.UpgradeType.Ground:
                stackUI.StackUp(3);
                break;
            case BaseUpgradeData.UpgradeType.Metal:
                stackUI.StackUp(4);
                break;
            case BaseUpgradeData.UpgradeType.Light:
                stackUI.StackUp(5);
                break;
            case BaseUpgradeData.UpgradeType.Dark:
                stackUI.StackUp(6);
                break;
        }
        
        if (level == data.damages.Length){
            button.interactable = false;
        }
    }

    public void SetInteractable()
    {
        button.interactable = true;
    }
}
