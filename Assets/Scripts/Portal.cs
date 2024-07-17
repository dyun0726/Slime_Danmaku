using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string nextSceneName; // 이동할 다음 씬의 이름
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false; // 포탈을 처음엔 보이지 않게 설정
    }

    public void ActivatePortal()
    {
        spriteRenderer.enabled = true; // 포탈을 보이게 설정
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && spriteRenderer.enabled)
        {
            GameManager.Instance.LoadNextScene();
            Debug.Log("Player entered the portal!");
        }
    }
}
