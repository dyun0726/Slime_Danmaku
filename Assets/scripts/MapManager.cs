using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject[] mapPrefabs; // �������� ������ �� ������ �迭

    private void Start()
    {
        GenerateRandomMap();
    }

    void GenerateRandomMap()
    {
        // �������� �� �������� ����
        int randomIndex = Random.Range(0, mapPrefabs.Length);
        GameObject selectedMapPrefab = mapPrefabs[randomIndex];

        // ���õ� �� �������� �ν��Ͻ�ȭ�Ͽ� ����
        GameObject newMap = Instantiate(selectedMapPrefab, Vector3.zero, Quaternion.identity);

        // �߰������� �� ���� �� �ʿ��� ������ ���⿡ �߰��� �� �ֽ��ϴ�.
    }
}
