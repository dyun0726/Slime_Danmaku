using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Trap : MonoBehaviour
{
    public float damage = 20f;
    public float damageInterval = 1f;
    public float activeDuration = 0.9f; // 트랩이 활성화되는 시간
    public float inactiveDuration = 2f; // 트랩이 비활성화되는 시간

    private Collider2D trapCollider;
    private TilemapRenderer tilemapRenderer;
    private Tilemap tilemap;
    private Collider2D playerCollider;
    private bool isTrapActive = true;

    private void Start()
    {
        trapCollider = GetComponent<Collider2D>();
        tilemapRenderer = GetComponent<TilemapRenderer>();
        tilemap = GetComponent<Tilemap>();
        StartCoroutine(TrapCycle());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTrapActive) return;

        Debug.Log("hit");
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
            StopCoroutine(DamagePlayer(collision.GetComponent<Player>()));
            playerCollider = null;
        }
    }

    private IEnumerator DamagePlayer(Player player)
    {
        while (true)
        {
            if (isTrapActive)
            {
                Vector2 dir = (player.transform.position - transform.position).normalized;
                PlayerManager.Instance.TakeDamage(damage, dir);
            }

            yield return new WaitForSeconds(damageInterval);

            if (playerCollider == null || !trapCollider.IsTouching(playerCollider))
            {
                yield break; // 코루틴 종료
            }
        }
    }

    private IEnumerator TrapCycle()
    {
        while (true)
        {
            // 트랩 활성화
            isTrapActive = true;
            trapCollider.enabled = true;
            tilemapRenderer.enabled = true;
            tilemap.color = Color.white; // 타일맵이 활성화될 때 색상을 원래대로
            yield return new WaitForSeconds(activeDuration);

            // 트랩 비활성화
            isTrapActive = false;
            trapCollider.enabled = false;
            tilemapRenderer.enabled = false;
            tilemap.color = new Color(1, 1, 1, 0); // 타일맵을 투명하게 만들어 비활성화 효과
            yield return new WaitForSeconds(inactiveDuration);
        }
    }
}
