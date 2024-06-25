using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private GameObject player; // 플레이어 오브젝트 참조

    // 싱글톤 인스턴스 반환
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject managerObject = new GameObject("GameManager");
                    instance = managerObject.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        // 씬 전환 시에도 유지되도록 함
        DontDestroyOnLoad(gameObject);

        // 플레이어 오브젝트를 찾아 참조
        player = GameObject.FindGameObjectWithTag("Player");

        // 플레이어 오브젝트에 DontDestroyOnLoad 설정
        if (player != null)
        {
            DontDestroyOnLoad(player);
        }
        else
        {
            Debug.LogError("Player not found in the scene!");
        }
    }

    // 다음 씬으로 이동하는 메서드
    public void LoadNextRandomScene()
    {
        // 사용 가능한 씬 리스트에서 랜덤하게 씬을 선택
        string nextSceneName = GetRandomSceneName();

        // 다음 씬으로 이동
        SceneManager.LoadScene(nextSceneName);

        // 씬이 로드된 후에 플레이어를 spawn 위치에 배치
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 사용 가능한 씬 리스트에서 랜덤하게 씬을 선택하는 메서드
    string GetRandomSceneName()
    {
        if (availableScenes.Count == 0)
        {
            Debug.LogWarning("No scenes available to load.");
            return null;
        }

        // 랜덤한 인덱스를 선택
        int randomIndex = Random.Range(0, availableScenes.Count);
        return availableScenes[randomIndex];
    }

    // 씬이 로드된 후에 호출되는 콜백 메서드
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 로드된 후에 spawn 위치를 찾아서 플레이어를 해당 위치에 배치
        PlacePlayerInSpawn();
    }

    // spawn 위치에 플레이어를 배치하는 메서드
    void PlacePlayerInSpawn()
    {
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("Spawn");
        if (spawnPoint != null && player != null)
        {
            player.transform.position = spawnPoint.transform.position;
        }
        else
        {
            Debug.LogWarning("Spawn point or player not found in the scene.");
        }
    }

    // Inspector에서 설정할 수 있는 사용 가능한 씬 리스트
    public List<string> availableScenes = new List<string>();
}
