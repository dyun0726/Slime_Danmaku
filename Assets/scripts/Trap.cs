using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    // Start is called before the first frame update
    public float damage = 999f; // 레이저빔이 주는 데미지
    public float damageInterval = 2f; // 데미지 판정을 주는 간격

    private Collider2D playerCollider; // 현재 충돌 중인 플레이어의 Collider

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 객체가 플레이어인지 확인
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            playerCollider = collision; // 플레이어의 Collider 저장
            StartCoroutine(DamagePlayer(player));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 플레이어가 범위를 벗어났을 때
        if (collision == playerCollider)
        {
            StopAllCoroutines(); // 모든 코루틴 중지
            playerCollider = null;
        }
    }

    private IEnumerator DamagePlayer(Player player)
    {
        while (true)
        {
            // 플레이어에게 데미지를 줌
            Vector2 dir = (player.transform.position - transform.position).normalized;
            PlayerManager.Instance.TakeDamage(damage, dir);

            // 2초 대기
            yield return new WaitForSeconds(damageInterval);

            // 플레이어가 범위를 벗어났는지 확인
            if (playerCollider == null || !GetComponent<Collider2D>().IsTouching(playerCollider))
            {
                yield break; // 코루틴 종료
            }
        }
    }
}
