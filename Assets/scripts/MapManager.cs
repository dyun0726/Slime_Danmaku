using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject[] mapPrefabs; // 랜덤으로 선택할 맵 프리팹 배열

    private void Start()
    {
        GenerateRandomMap();
    }

    void GenerateRandomMap()
    {
        // 랜덤으로 맵 프리팹을 선택
        int randomIndex = Random.Range(0, mapPrefabs.Length);
        GameObject selectedMapPrefab = mapPrefabs[randomIndex];

        // 선택된 맵 프리팹을 인스턴스화하여 생성
        GameObject newMap = Instantiate(selectedMapPrefab, Vector3.zero, Quaternion.identity);

        // 추가적으로 맵 생성 시 필요한 설정을 여기에 추가할 수 있습니다.
    }
}
