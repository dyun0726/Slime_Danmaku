using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Trap : MonoBehaviour
{
    public float damage = 20f;
    public float damageInterval = 1f;
    public float activeDuration = 0.9f; // 트랩이 활성화되는 시간
    public float inactiveDuration = 2f; // 트랩이 비활성화되는 시간
    public bool isFilck = false;

    private Collider2D trapCollider;
    private TilemapRenderer tilemapRenderer;
    private SpriteRenderer spriteRenderer;
    private Tilemap tilemap;
    private Collider2D playerCollider;
    private bool isTrapActive = true;

    private void Start()
    {
        trapCollider = GetComponent<Collider2D>();
        tilemapRenderer = GetComponent<TilemapRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        tilemap = GetComponent<Tilemap>();

        // TilemapRenderer 또는 SpriteRenderer가 있는 경우에만 코루틴 시작
        if (tilemapRenderer != null || spriteRenderer != null)
        {
            if (isFilck)
            {
                StartCoroutine(TrapCycle());
            }
            else
            {
                isTrapActive = true;
            }
            
        }
        else
        {
            Debug.LogWarning("TilemapRenderer 또는 SpriteRenderer가 없습니다. TrapCycle이 비활성화됩니다.");
        }
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

            if (tilemapRenderer != null)
            {
                tilemapRenderer.enabled = true;
                tilemap.color = Color.white; // 타일맵이 활성화될 때 색상을 원래대로
            }

            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = true;
                spriteRenderer.color = Color.white; // Sprite가 활성화될 때 색상을 원래대로
            }

            yield return new WaitForSeconds(activeDuration);

            // 트랩 비활성화
            isTrapActive = false;
            trapCollider.enabled = false;

            if (tilemapRenderer != null)
            {
                tilemapRenderer.enabled = false;
                tilemap.color = new Color(1, 1, 1, 0); // 타일맵을 투명하게 만들어 비활성화 효과
            }

            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
                spriteRenderer.color = new Color(1, 1, 1, 0); // Sprite를 투명하게 만들어 비활성화 효과
            }

            yield return new WaitForSeconds(inactiveDuration);
        }
    }
}
