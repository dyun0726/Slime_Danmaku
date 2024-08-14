using UnityEngine;
using UnityEngine.UI;

public class Signpost : MonoBehaviour
{
    public string tutorialMessage; // 표지판의 튜토리얼 메시지
    public float displayRange = 5f; // 플레이어와의 거리 범위
    public GameObject messageUI; // 튜토리얼 메시지를 표시할 UI 오브젝트 (텍스트)

    private Transform player; // 플레이어의 트랜스폼

    private void Start()
    {
        // 플레이어를 찾음 (플레이어 태그를 설정했다고 가정)
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // 초기에는 메시지 UI를 비활성화
        if (messageUI != null)
        {
            messageUI.SetActive(false);
        }
    }

    private void Update()
    {
        // 플레이어와의 거리 계산
        float distance = Vector3.Distance(player.position, transform.position);

        // 플레이어가 일정 범위 내에 들어오면 메시지 UI를 활성화
        if (distance <= displayRange)
        {
            if (messageUI != null)
            {
                messageUI.SetActive(true);
               // messageUI.GetComponent<Text>().text = tutorialMessage; // 메시지 설정
            }
        }
        else
        {
            // 범위를 벗어나면 메시지 UI를 비활성화
            if (messageUI != null)
            {
                messageUI.SetActive(false);
            }
        }
    }
}
