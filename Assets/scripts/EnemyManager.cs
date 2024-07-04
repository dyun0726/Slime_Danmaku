using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> enemyPrefabs; // 적 프리팹 리스트
    public int numberOfEnemiesToSpawn = 2; // 스폰할 적의 수

    private List<Transform> spawnPoints; // 스폰 위치 리스트

    void Awake()
    {
        // 씬 로딩 이벤트 구독
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 현재 씬의 스폰 포인트 초기화 및 적 스폰
       // InitializeSpawnPointsAndSpawnEnemies();
    }

    void OnDestroy()
    {
        // 씬 로딩 이벤트 구독 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 새로운 씬이 로드되었을 때 스폰 포인트 초기화 및 적 스폰
        InitializeSpawnPointsAndSpawnEnemies();
    }

    void InitializeSpawnPointsAndSpawnEnemies()
    {
        // 스폰 포인트를 태그로 찾기
        GameObject[] spawnPointObjects = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");
        spawnPoints = new List<Transform>();

        foreach (GameObject spawnPointObject in spawnPointObjects)
        {
            spawnPoints.Add(spawnPointObject.transform);
        }

        // 적 스폰
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        // 스폰 위치 랜덤하게 선택
        List<Transform> selectedSpawnPoints = new List<Transform>();
        while (selectedSpawnPoints.Count < numberOfEnemiesToSpawn && spawnPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, spawnPoints.Count);
            selectedSpawnPoints.Add(spawnPoints[randomIndex]);
            spawnPoints.RemoveAt(randomIndex); // 선택된 위치는 리스트에서 제거
        }

        // 선택된 위치에 적 스폰
        foreach (Transform spawnPoint in selectedSpawnPoints)
        {
            int randomEnemyIndex = Random.Range(0, enemyPrefabs.Count);
            Instantiate(enemyPrefabs[randomEnemyIndex], spawnPoint.position, spawnPoint.rotation);
        }
    }
}
