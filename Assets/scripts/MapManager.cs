using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    public GameObject[] mapPrefabs; // 랜덤으로 선택할 맵 프리팹 배열
    public Vector2Int gridSize = new Vector2Int(10, 10); // 격자의 크기

    private HashSet<Vector3Int> usedPositions = new HashSet<Vector3Int>();

    private void Start()
    {
        GenerateRandomMaps(2); // 예시로 2개의 타일맵을 배치합니다.
    }

    void GenerateRandomMaps(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // 랜덤한 위치를 선택
            Vector3Int randomPosition = GetRandomPosition();

            // 기존 타일맵과 충돌을 검사
            if (CheckCollision(randomPosition))
            {
                Debug.Log("Collision detected, retrying...");
                i--; // 충돌이 발생하면 다시 시도합니다.
                continue;
            }

            // 선택된 맵 프리팹을 랜덤한 위치에 인스턴스화하여 생성
            int randomIndex = Random.Range(0, mapPrefabs.Length);
            GameObject selectedMapPrefab = mapPrefabs[randomIndex];
            GameObject newMap = Instantiate(selectedMapPrefab, randomPosition, Quaternion.identity);

            // 기존 타일맵에 사용된 위치 추가
            AddUsedPosition(randomPosition);
        }
    }

    Vector3Int GetRandomPosition()
    {
        // 격자 내에서 랜덤한 위치를 선택합니다.
        int x = Random.Range(-gridSize.x / 2, gridSize.x / 2 + 1);
        int y = Random.Range(-gridSize.y / 2, gridSize.y / 2 + 1);
        return new Vector3Int(x, y, 0);
    }

    bool CheckCollision(Vector3Int position)
    {
        // 기존 타일맵과의 충돌을 검사합니다.
        return usedPositions.Contains(position);
    }

    void AddUsedPosition(Vector3Int position)
    {
        // 사용된 위치를 기록합니다.
        usedPositions.Add(position);
    }
}
