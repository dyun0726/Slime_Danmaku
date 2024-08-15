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

        // 2. 랜덤 3개 업그레이드 뽑기 (full upgrade시 다시 뽑기)
        int[] ran = new int[3];
        do
        {
            ran[0] = Random.Range(0, upgrades.Length);
        } while (CheckFullUpgrade(ran[0]));
        
        do
        {
            ran[1] = Random.Range(0, upgrades.Length);
        } while (ran[1] == ran[0] || CheckFullUpgrade(ran[1]));

        do
        {
            ran[2] = Random.Range(0, upgrades.Length);
        } while (ran[2] == ran[0] || ran[2] == ran[1] || CheckFullUpgrade(ran[2]));

        // 3. 선택지 활성화
        for (int idx = 0; idx < ran.Length; idx++){
            Upgrade ranUpgrade = upgrades[ran[idx]];
            ranUpgrade.gameObject.SetActive(true);
        }
    }

    private bool CheckFullUpgrade(int index)
    {
        Upgrade ranUpgrade = upgrades[index];
        return ranUpgrade.level == ranUpgrade.data.damages.Length;
    }
}
