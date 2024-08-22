using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
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

    public void HidePortal()
    {
        spriteRenderer.enabled = false; // 포탈을 보이게 설정
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
