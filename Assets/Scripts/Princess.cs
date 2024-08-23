using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Princess : MonoBehaviour
{
    public GameObject endingCanvas;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == 10)
        {
            endingCanvas.SetActive(true);
        }
    }
}
