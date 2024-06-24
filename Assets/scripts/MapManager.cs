using UnityEngine;
using UnityEngine.Tilemaps;

using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    public Tilemap[] mapPrefabs; // �������� ������ �� ������ �迭
    public Vector2Int gridSize = new Vector2Int(10, 10); // ������ ũ��

    private HashSet<Vector3Int> usedPositions = new HashSet<Vector3Int>();

    private void Start()
    {
        GenerateRandomMaps(2); // ���÷� 5���� Ÿ�ϸ��� ��ġ�մϴ�.
    }

    void GenerateRandomMaps(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // ������ ��ġ�� ����
            Vector3Int randomPosition = GetRandomPosition();

            // ���� Ÿ�ϸʰ� �浹�� �˻�
            if (CheckCollision(randomPosition))
            {
                Debug.Log("Collision detected, retrying...");
                i--; // �浹�� �߻��ϸ� �ٽ� �õ��մϴ�.
                continue;
            }

            // ���õ� �� �������� ������ ��ġ�� �ν��Ͻ�ȭ�Ͽ� ����
            int randomIndex = Random.Range(0, mapPrefabs.Length);
            Tilemap selectedMapPrefab = mapPrefabs[randomIndex];
            Tilemap newMap = Instantiate(selectedMapPrefab, randomPosition, Quaternion.identity);

            // ���� Ÿ�ϸʿ� ���� ��ġ �߰�
            AddUsedPosition(randomPosition);
        }
    }

    Vector3Int GetRandomPosition()
    {
        // ���� ������ ������ ��ġ�� �����մϴ�.
        int x = Random.Range(-gridSize.x / 2, gridSize.x / 2 + 1);
        int y = Random.Range(-gridSize.y / 2, gridSize.y / 2 + 1);
        return new Vector3Int(x, y, 0);
    }

    bool CheckCollision(Vector3Int position)
    {
        // ���� Ÿ�ϸʰ��� �浹�� �˻��մϴ�.
        foreach (var usedPos in usedPositions)
        {
            if (usedPos == position)
            {
                return true; // �浹 �߻�
            }
        }
        return false; // �浹 ����
    }

    void AddUsedPosition(Vector3Int position)
    {
        // ���� ��ġ�� ����մϴ�.
        usedPositions.Add(position);
    }
}