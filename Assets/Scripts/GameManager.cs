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
    private int world3MoveCount = 0; // World3에서의 씬 이동 횟수를 추적하는 변수
    private int world4MoveCount = 0; // World4에서의 씬 이동 횟수를 추적하는 변수
    private string currentWorld = "World1"; // 현재 진행 중인 월드 이름

    // 게임이 멈춰있는지
    public bool isLive;

    // 맵의 끝 범위 변수
    public float leftBound = -12f;
    public float rightBound = 49f;
    public float upperBound = 10f;
    public float lowerBound = -4.5f;

    // 해금 관련 변수
    public int killed = 0;
    public bool clearStage1 = false;
    public bool clearStage2 = false;
    public bool clearStage3 = false;
    public bool gameClear = false;

    // 적 피격 소리 변수
    private AudioSource audioSource;
    public AudioClip enemyHitSound;

    public void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null){
            Debug.LogError("Player not found in the scene!");
        }
        world1MoveCount = 0;
        world2MoveCount = 0;
        world3MoveCount = 0;
        world4MoveCount = 0;
        currentWorld = "World1";
        killed = 0;
        clearStage1 = false;
        clearStage2 = false;
        clearStage3 = false;
        gameClear = false;
    }

    void Awake()
    {
        // 씬 전환 시에도 유지되도록 함
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this){
            Destroy(gameObject);
        }

        // 플레이어 오브젝트를 찾아 참조
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null){
            Debug.LogError("Player not found in the scene!");
        }

        audioSource = GetComponent<AudioSource>();
    }

    // 다음 씬으로 이동하는 메서드
    public void LoadNextScene()
    {
        string nextSceneName = "";
        
        // 현재 진행 중인 월드에 따라 다음 씬 이름 설정
        if (currentWorld == "World1")
        {
            if (world1MoveCount < 4) // World1에서는 총 3번의 랜덤 씬 이동
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
            // BossRoom1 종료 후 World2로 이동
            clearStage1 = true;
            nextSceneName = "World2_Start";
            currentWorld = "World2";

        }
        else if (currentWorld == "World2")
        {
            if (world2MoveCount < 4) // World2에서는 총 3번의 랜덤 씬 이동
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
            clearStage2 = true;
            nextSceneName = "World3_Start";
            currentWorld = "World3";
        }
        else if (currentWorld == "World3")
        {
            if (world3MoveCount < 4) // World3에서는 총 3번의 랜덤 씬 이동
            {
                nextSceneName = GetRandomWorld3SceneName();
            }
            else
            {
                nextSceneName = "BossRoom3"; // World2 종료 후 BossRoom2로 이동
                currentWorld = "BossRoom3"; // 현재 월드 설정 변경
            }
        }
        else if (currentWorld == "BossRoom3")
        {
            // BossRoom3 종료 후 게임 종료 등의 처리 가능
            clearStage3 = true;
            nextSceneName = "World4_Start";
            currentWorld = "World4";
        }
        else if (currentWorld == "World4")
        {
            if (world4MoveCount < 3) // World4에서는 총 3번의 랜덤 씬 이동
            {
                nextSceneName = GetRandomWorld4SceneName();
            }
            else
            {
                nextSceneName = "BossRoom4"; // World2 종료 후 BossRoom2로 이동
                currentWorld = "BossRoom4"; // 현재 월드 설정 변경
            }
        }
        else if (currentWorld == "BossRoom4")
        {
            // BossRoom4 종료 후 게임 종료 등의 처리 가능
            gameClear = true;
            Debug.Log("Game completed!");
            return;
        }

        // 다음 씬으로 이동
        SceneManager.LoadScene(nextSceneName);

        // 씬이 로드된 후에 플레이어를 spawn 위치에 배치
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 씬 이동 횟수 증가
        if (currentWorld == "World1")
        {
            world1MoveCount++;
        }
        else if (currentWorld == "World2")
        {
            world2MoveCount++;
        }
        else if (currentWorld == "World3")
        {
            world3MoveCount++;
        }
        else if (currentWorld == "World4")
        {
            world4MoveCount++;
        }
    }

    // World1에서 랜덤하게 씬을 선택하는 메서드
    string GetRandomWorld1SceneName()
    {
        //List<string> world1Scenes = new List<string> { "World4_Start" };
        List<string> world1Scenes = new List<string> { "Stage1_1", "Stage1_2", "Stage1_3", "Stage1_4", "Stage1_5", "Stage1_6", "Stage1_7" };
        int randomIndex = Random.Range(0, world1Scenes.Count);
        return world1Scenes[randomIndex];
    }

    // World2에서 랜덤하게 씬을 선택하는 메서드
    string GetRandomWorld2SceneName()
    {
        List<string> world2Scenes = new List<string> { "Stage2_1", "Stage2_2", "Stage2_3", "Stage2_4", "Stage2_5", "Stage2_6" };
        int randomIndex = Random.Range(0, world2Scenes.Count);
        return world2Scenes[randomIndex];
    }

    string GetRandomWorld3SceneName()
    {
        List<string> world3Scenes = new List<string> { "Stage3_1", "Stage3_2" , "Stage3_3" , "Stage3_4" , "Stage3_5" };
        int randomIndex = Random.Range(0, world3Scenes.Count);
        return world3Scenes[randomIndex];
    }

    string GetRandomWorld4SceneName()
    {
        List<string> world4Scenes = new List<string> { "Stage4_1", "Stage4_2", "Stage4_3", "Stage4_4", "Stage4_5" };
        int randomIndex = Random.Range(0, world4Scenes.Count);
        return world4Scenes[randomIndex];
    }

    // 씬이 로드된 후에 호출되는 콜백 메서드
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlacePlayerInSpawn();
        CameraManager.instance.SetConfiner();
        PoolManager.instance.DisableAllObjects();
        SetBounds();

        if (currentWorld == "World1")
        {
            MusicManager.instance.PlayMusicForWorld(1);
        }
        else if (currentWorld == "World2")
        {
            MusicManager.instance.PlayMusicForWorld(2);
        }
        else if (currentWorld == "World3")
        {
            MusicManager.instance.PlayMusicForWorld(3);
        }
        else if (currentWorld == "World4")
        {
            MusicManager.instance.PlayMusicForWorld(4);
        }

        SceneManager.sceneLoaded -= OnSceneLoaded; // 이벤트 핸들러 해제
    }

    // spawn 위치에 플레이어를 배치하는 메서드
    public void PlacePlayerInSpawn()
    {
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("Spawn");
        if (spawnPoint != null && Player.Instance != null)
        {
            Player.Instance.transform.position = spawnPoint.transform.position;
            CameraManager.instance.SetPosition(spawnPoint.transform.position);
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

    // 탄막 범위 설정 함수
    public void SetBounds(){
        GameObject confinerGO = GameObject.FindWithTag("Confiner");
        if (confinerGO != null){
            PolygonCollider2D confinerCollider = confinerGO.GetComponent<PolygonCollider2D>();
            if (confinerCollider != null){
                Vector2[] points = confinerCollider.points;

                rightBound = points[0].x;
                leftBound = points[0].x;
                upperBound = points[0].y;
                lowerBound = points[0].y;

                for (int i = 1; i < points.Length; i++){
                    if (points[i].x > rightBound)
                    {
                        rightBound = points[i].x;
                    }
                    if (points[i].x < leftBound)
                    {
                        leftBound = points[i].x;
                    }
                    if (points[i].y > upperBound)
                    {
                        upperBound = points[i].y;
                    }
                    if (points[i].y < lowerBound)
                    {
                        lowerBound = points[i].y;
                    }
                }
            }

        } else {
            Debug.LogWarning("Can't find confiner GameObject!");
        }
    }

    // 적 피격 소리 실행 함수
    public void PlayEnemyHitSound()
    {
        audioSource.PlayOneShot(enemyHitSound);
    }

    // 볼륨 세팅 함수
    public void SetVolume(float amount)
    {
        audioSource.volume = amount;
    }
}
