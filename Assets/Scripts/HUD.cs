using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Gold, Health}
    public InfoType type;

    private TextMeshProUGUI myText;
    private Slider mySlider;

    private void Awake() {
        myText = GetComponent<TextMeshProUGUI>();
        mySlider = GetComponent<Slider>();
    }


    private void LateUpdate() {
        switch (type){
            case InfoType.Exp:
                float curExp = PlayerManager.Instance.exp;
                float maxExp = PlayerManager.Instance.levelUpExp;
                mySlider.value = curExp / maxExp;
                break;

            case InfoType.Level:
                myText.text = "LV. " + PlayerManager.Instance.level;
                break;
            
            case InfoType.Gold:
                myText.text = "Gold: " + PlayerManager.Instance.gold;
                break;
            
            case InfoType.Health:
                myText.text = "HP: " + PlayerManager.Instance.currentHealth;
                break;
        }
    }

}
