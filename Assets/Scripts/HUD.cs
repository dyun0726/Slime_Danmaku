using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Time, Health}
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

                break;
            
            case InfoType.Time:

                break;
            
            case InfoType.Health:

                break;
        }
    }

}
