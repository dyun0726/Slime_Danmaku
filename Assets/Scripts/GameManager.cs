using System.Collections;
using System.Collections.Generic;
// using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
// using UnityEngine.UI; // UI 관련 네임스페이스 추가

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

    private int world1MoveCount = 0; // World1에서의 씬 이동 횟수를 추적하는 변수
    private int world2MoveCount = 0; // World2에서의 씬 이동 횟수를 추적하는 변수
    private string currentWorld = "World1"; // 현재 진행 중인 월드 이름

    // 게임이 멈춰있는지
    public bool isLive;

    void Awake()
    {
        // 씬 전환 시에도 유지되도록 함
        DontDestroyOnLoad(gameObject);

        // 플레이어 오브젝트를 찾아 참조
        player = GameObject.FindGameObjectWithTag("Player");
        
        
        if (player != null)
        {
            DontDestroyOnLoad(player);
        }
        else
        {
            Debug.LogError("Player not found in the scene!");
        }

    }

    void Update()
    {
        
    }

   

    // 다음 씬으로 이동하는 메서드
    public void LoadNextScene()
    {

        // 체력 회복 예시
        // 주석 풀꺼면 두번째로 Vector2 넣어주어야함
        // PlayerManager.Instance.TakeDamage(20);

        // 스탯 증가 예시
        PlayerManager.Instance.IncreaseStrength(1);

        string nextSceneName = "";
        
        // 현재 진행 중인 월드에 따라 다음 씬 이름 설정
        if (currentWorld == "World1")
        {
            PlayerManager.Instance.AddGold(10);
            if (world1MoveCount < 3) // World1에서는 총 1번의 랜덤 씬 이동
            {
                nextSceneName = GetRandomWorld1SceneName();
            }
            else
            {
                nextSceneName = "BossRoom1"; // World1 종료 후 BossRoom1로 이동
                currentWorld = "BossRoom1"; // 현재 월드 설정 변경
            }
        }
        else if (currentWorld == "BossRoom1")
        {
            PlayerManager.Instance.AddGold(100);
            // BossRoom1 종료 후 World2로 이동
            nextSceneName = "World2_Start";
            currentWorld = "World2";
        }
        else if (currentWorld == "World2")
        {
            PlayerManager.Instance.AddGold(20);
            if (world2MoveCount < 3) // World2에서는 총 5번의 랜덤 씬 이동
            {
                nextSceneName = GetRandomWorld2SceneName();
            }
            else
            {
                nextSceneName = "BossRoom2"; // World2 종료 후 BossRoom2로 이동
                currentWorld = "BossRoom2"; // 현재 월드 설정 변경
            }
        }
        else if (currentWorld == "BossRoom2")
        {
            // BossRoom2 종료 후 게임 종료 등의 처리 가능
            Debug.Log("Game completed!");
            return;
        }

        // 다음 씬으로 이동
        SceneManager.LoadScene(nextSceneName);

        // 씬이 로드된 후에 플레이어를 spawn 위치에 배치
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 해당 씬에서 CameraConfiner 찾기
       // CameraManager.instance.SetConfiner();

        // 씬 이동 횟수 증가
        if (currentWorld == "World1")
        {
            world1MoveCount++;
        }
        else if (currentWorld == "World2")
        {
            world2MoveCount++;
        }
    }

    // World1에서 랜덤하게 씬을 선택하는 메서드
    string GetRandomWorld1SceneName()
    {
       //List<string> world1Scenes = new List<string> { "World1_1", "World1_2", "World1_3", "World1_4", "World1_5" };
        List<string> world1Scenes = new List<string> { "Stage1_1", "Stage1_2", "Stage1_3", "Stage1_4", "Stage1_5", "Stage1_6", "Stage1_7" };
        int randomIndex = Random.Range(0, world1Scenes.Count);
        return world1Scenes[randomIndex];
    }

    // World2에서 랜덤하게 씬을 선택하는 메서드
    string GetRandomWorld2SceneName()
    {
        List<string> world2Scenes = new List<string> { "World2_1", "World2_2", "World2_3" };
        int randomIndex = Random.Range(0, world2Scenes.Count);
        return world2Scenes[randomIndex];
    }

    // 씬이 로드된 후에 호출되는 콜백 메서드
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 로드된 후에 spawn 위치를 찾아서 플레이어를 해당 위치에 배치
        PlacePlayerInSpawn();
        CameraManager.instance.SetConfiner();
        PoolManager.instance.DisableAllObjects();
        // PlayerGoldManager.Instance.FindGoldTextInNewScene();
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
    
    // 시간을 멈추는 함수
    public void Stop(){
        isLive = false;
        Time.timeScale = 0;
    }

    // 재실행 함수
    public void Resume(){
        isLive = true;
        Time.timeScale = 1;
    }
}
