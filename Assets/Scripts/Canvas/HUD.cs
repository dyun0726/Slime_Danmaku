using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Gold, HealthText, HealthSlider, ShieldSlider }
    public InfoType type;

    private TextMeshProUGUI myText;
    private Slider mySlider;
    // public Slider shieldSlider; 

    private void Awake()
    {
        myText = GetComponent<TextMeshProUGUI>();
        mySlider = GetComponent<Slider>();

        // if (shieldSlider == null)
        // {
        //     Transform shieldSliderTransform = transform.Find("ShieldSlider");
        //     if (shieldSliderTransform != null)
        //     {
        //         shieldSlider = shieldSliderTransform.GetComponent<Slider>();
        //     }
        //     else
        //     {
        //         Debug.LogError("ShieldSlider not found. Please assign it in the inspector.");
        //     }
        // }
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                UpdateExp();
                break;

            case InfoType.Level:
                UpdateLevel();
                break;

            case InfoType.Gold:
                UpdateGold();
                break;

            case InfoType.HealthText:
                UpdateHealthText();
                break;

            case InfoType.HealthSlider:
                UpdateHealthSlider();
                break;

            case InfoType.ShieldSlider:
                UpdateShieldSlider();
                break;
        }
    }

    private void UpdateExp()
    {
        float curExp = PlayerManager.Instance.exp;
        float maxExp = PlayerManager.Instance.levelUpExp;
        mySlider.value = curExp / maxExp;
    }

    private void UpdateLevel()
    {
        myText.text = "LV. " + PlayerManager.Instance.level;
    }

    private void UpdateGold()
    {
        myText.text = "Gold: " + PlayerManager.Instance.gold + "(+" + PlayerManager.Instance.goldbonus + "%)";
    }

    private void UpdateHealthText()
    {
        myText.text = PlayerManager.Instance.currentHealth.ToString("F1") + "/" + PlayerManager.Instance.maxHealth + "(+" + PlayerManager.Instance.shield.ToString("F1") + ")";
    }

    private void UpdateHealthSlider()
    {
        float curHP = PlayerManager.Instance.currentHealth;
        float maxHP = PlayerManager.Instance.maxHealth;
        // float shield = PlayerManager.Instance.shield;

        mySlider.maxValue = maxHP ;
        mySlider.value = curHP ;

        // shieldSlider.maxValue = maxHP; 
        // shieldSlider.value = shield;

        /*  전체 바의 값을 체력+실드로 하려면 아래껄로
           mySlider.maxValue = maxHP + shield;
           mySlider.value = curHP + shield;

           shieldSlider.maxValue = maxHP + shield;
           shieldSlider.value = shield;
       */
    }

    private void UpdateShieldSlider(){
        float maxHP = PlayerManager.Instance.maxHealth;
        float shield = PlayerManager.Instance.shield;

        mySlider.maxValue = maxHP;
        // 쉴드가 maxHP 초과시 꽉차도록
        mySlider.value = (shield <= maxHP) ? shield : maxHP;

    }
}
