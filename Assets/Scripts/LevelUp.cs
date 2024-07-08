using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    public bool isVisible = false;
    private Upgrade[] upgrades;

    private void Awake() {
        rect = GetComponent<RectTransform>();
        upgrades = GetComponentsInChildren<Upgrade>(true);
    }

    public void Show(){
        isVisible = true;
        Next();
        rect.localScale = Vector3.one;
        GameManager.Instance.Stop();
    }

    public void Hide(){
        isVisible = false;
        rect.localScale = Vector3.zero;
        GameManager.Instance.Resume();
    }

    public bool IsVisible(){
        return isVisible;
    }

    private void Next(){
        // 1. 모든 아이템 비활성화
        foreach (Upgrade upgrade in upgrades){
            upgrade.gameObject.SetActive(false);
        }

        // 2. 그 중에서 랜덤 3개 활성화
        int[] ran = new int[3];
        while (true){
            ran[0] = Random.Range(0, upgrades.Length);
            ran[1] = Random.Range(0, upgrades.Length);
            ran[2] = Random.Range(0, upgrades.Length);
            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2]){
                break;
            }
        }

        for (int idx = 0; idx < ran.Length; idx++){
            Upgrade ranUpgrade = upgrades[ran[idx]];

            // 3. 만렙 아이템의 경우 다른 아이템 대체
            if (ranUpgrade.level == ranUpgrade.data.damages.Length){
                // 나중에 수정 필요
                ranUpgrade.gameObject.SetActive(true);
            } else {
                ranUpgrade.gameObject.SetActive(true);
            }
        }


        
    }
}
