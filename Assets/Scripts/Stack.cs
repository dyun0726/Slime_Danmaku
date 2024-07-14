using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stack : MonoBehaviour
{
    private TextMeshProUGUI[] stacks;
    private int[] levels;
    private void Awake() {
        stacks = GetComponentsInChildren<TextMeshProUGUI>();
        levels = new int[stacks.Length];
    }

    public void StackUp(int index) {
        if (index >= stacks.Length){
            Debug.Log("Invalid index access in stack UI");
            return;
        }
        levels[index] += 1;
        stacks[index].text = levels[index].ToString();
    }
}
