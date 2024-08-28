using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionItem : MonoBehaviour
{
    public float healAmount = 20f; // 회복량 설정
    private Rigidbody2D rb;
    private bool isGrounded = false;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (isGrounded)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10) // 플레이어와 충돌 감지
        {
            PlayerManager.Instance.Heal(healAmount); // 플레이어 힐
            Destroy(gameObject); // 하트 아이템 파괴
        }
        else if (other.gameObject.layer == 6)
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.layer == 6)
        {
            isGrounded = false;
            rb.isKinematic = false;
        }
    }
}
