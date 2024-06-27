using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI ���� ���ӽ����̽� �߰�

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private GameObject player; // �÷��̾� ������Ʈ ����

    public PlayerGoldManager playerGoldManager; // ��� ����
    public UpgradeManager upgradeManager; // ��ȭ �ý���

    // UI Text ���
    public Text healthText;
    public TextMeshProUGUI goldText;

    // �̱��� �ν��Ͻ� ��ȯ
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

    private int world1MoveCount = 0; // World1������ �� �̵� Ƚ���� �����ϴ� ����
    private int world2MoveCount = 0; // World2������ �� �̵� Ƚ���� �����ϴ� ����
    private string currentWorld = "World1"; // ���� ���� ���� ���� �̸�

    void Awake()
    {
        // �� ��ȯ �ÿ��� �����ǵ��� ��
        DontDestroyOnLoad(gameObject);

        // �÷��̾� ������Ʈ�� ã�� ����
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

   

    // ���� ������ �̵��ϴ� �޼���
    public void LoadNextScene()
    {

        // ü�� ȸ�� ����
        PlayerHealthManager.Instance.TakeDamage(20);

        // ���� ���� ����
        PlayerStatsManager.Instance.IncreaseStrength(1);

        string nextSceneName = "";
        
        // ���� ���� ���� ���忡 ���� ���� �� �̸� ����
        if (currentWorld == "World1")
        {
            PlayerGoldManager.Instance.AddGold(10);
            if (world1MoveCount < 1) // World1������ �� 1���� ���� �� �̵�
            {
                nextSceneName = GetRandomWorld1SceneName();
            }
            else
            {
                nextSceneName = "BossRoom1"; // World1 ���� �� BossRoom1�� �̵�
                currentWorld = "BossRoom1"; // ���� ���� ���� ����
            }
        }
        else if (currentWorld == "BossRoom1")
        {
            PlayerGoldManager.Instance.AddGold(100);
            // BossRoom1 ���� �� World2�� �̵�
            nextSceneName = "World2_Start";
            currentWorld = "World2";
        }
        else if (currentWorld == "World2")
        {
            PlayerGoldManager.Instance.AddGold(20);
            if (world2MoveCount < 5) // World2������ �� 5���� ���� �� �̵�
            {
                nextSceneName = GetRandomWorld2SceneName();
            }
            else
            {
                nextSceneName = "BossRoom2"; // World2 ���� �� BossRoom2�� �̵�
                currentWorld = "BossRoom2"; // ���� ���� ���� ����
            }
        }
        else if (currentWorld == "BossRoom2")
        {
            // BossRoom2 ���� �� ���� ���� ���� ó�� ����
            Debug.Log("Game completed!");
            return;
        }

        // ���� ������ �̵�
        SceneManager.LoadScene(nextSceneName);

        // ���� �ε�� �Ŀ� �÷��̾ spawn ��ġ�� ��ġ
        SceneManager.sceneLoaded += OnSceneLoaded;

        // �� �̵� Ƚ�� ����
        if (currentWorld == "World1")
        {
            world1MoveCount++;
        }
        else if (currentWorld == "World2")
        {
            world2MoveCount++;
        }
    }

    // World1���� �����ϰ� ���� �����ϴ� �޼���
    string GetRandomWorld1SceneName()
    {
        List<string> world1Scenes = new List<string> { "World1_1", "World1_2" };
        int randomIndex = Random.Range(0, world1Scenes.Count);
        return world1Scenes[randomIndex];
    }

    // World2���� �����ϰ� ���� �����ϴ� �޼���
    string GetRandomWorld2SceneName()
    {
        List<string> world2Scenes = new List<string> { "World2_1", "World2_2", "World2_3" };
        int randomIndex = Random.Range(0, world2Scenes.Count);
        return world2Scenes[randomIndex];
    }

    // ���� �ε�� �Ŀ� ȣ��Ǵ� �ݹ� �޼���
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���� �ε�� �Ŀ� spawn ��ġ�� ã�Ƽ� �÷��̾ �ش� ��ġ�� ��ġ
        PlacePlayerInSpawn();

       // PlayerGoldManager.Instance.FindGoldTextInNewScene();
    }

    // spawn ��ġ�� �÷��̾ ��ġ�ϴ� �޼���
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

    public void ShowUpgradeOptions()
    {
        upgradeManager.ShowUpgradeOptions();
    }
}
