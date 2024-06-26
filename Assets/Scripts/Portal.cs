using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string nextSceneName; // �̵��� ���� ���� �̸�

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
                      
              GameManager.Instance.LoadNextScene();
            Debug.Log("Player entered the portal!");
        }
    }
}
