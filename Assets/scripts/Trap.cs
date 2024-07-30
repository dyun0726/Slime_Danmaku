using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public float damage = 20f; 
    public float damageInterval = 1f;

    private Collider2D playerCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            playerCollider = collision;
            StartCoroutine(DamagePlayer(player));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == playerCollider)
        {
            StopAllCoroutines();
            playerCollider = null;
        }
    }

    private IEnumerator DamagePlayer(Player player)
    {
        while (true)
        {
          
            Vector2 dir = (player.transform.position - transform.position).normalized;
            PlayerManager.Instance.TakeDamage(damage, dir);

            // 0.5초 대기
            yield return new WaitForSeconds(damageInterval);

            // 플레이어가 레이저빔 범위를 벗어났는지 확인
            if (playerCollider == null || !GetComponent<Collider2D>().IsTouching(playerCollider))
            {
                yield break; // 코루틴 종료
            }
        }
    }
}