using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionItem : MonoBehaviour
{
    public float healAmount = 20f; // 회복량 설정

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) // 플레이어와 충돌 감지
        {
            PlayerManager.Instance.Heal(healAmount); // 플레이어 힐
            Destroy(gameObject); // 하트 아이템 파괴
        }
    }
}
