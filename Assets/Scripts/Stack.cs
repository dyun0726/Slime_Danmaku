using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stack : MonoBehaviour
{
    public TextMeshProUGUI[] stackTexts;
    public int[] levels;
    private void Awake() {
        stackTexts = GetComponentsInChildren<TextMeshProUGUI>();
        levels = new int[stackTexts.Length];
    }

    public void StackUp(int index) {
        if (index >= stackTexts.Length){
            Debug.Log("Invalid index access in stack UI");
            return;
        }
        levels[index] += 1;
        stackTexts[index].text = levels[index].ToString();

        if (!PlayerManager.Instance.typeStacks[index] && levels[index] >= 5)
        {
            PlayerManager.Instance.typeStacks[index] = true;
            PlayerManager.Instance.ApplyStack(index);
        }
    }
}
