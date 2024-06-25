using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private GameObject player; // �÷��̾� ������Ʈ ����

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

    void Awake()
    {
        // �� ��ȯ �ÿ��� �����ǵ��� ��
        DontDestroyOnLoad(gameObject);

        // �÷��̾� ������Ʈ�� ã�� ����
        player = GameObject.FindGameObjectWithTag("Player");

        // �÷��̾� ������Ʈ�� DontDestroyOnLoad ����
        if (player != null)
        {
            DontDestroyOnLoad(player);
        }
        else
        {
            Debug.LogError("Player not found in the scene!");
        }
    }

    // ���� ������ �̵��ϴ� �޼���
    public void LoadNextRandomScene()
    {
        // ��� ������ �� ����Ʈ���� �����ϰ� ���� ����
        string nextSceneName = GetRandomSceneName();

        // ���� ������ �̵�
        SceneManager.LoadScene(nextSceneName);

        // ���� �ε�� �Ŀ� �÷��̾ spawn ��ġ�� ��ġ
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // ��� ������ �� ����Ʈ���� �����ϰ� ���� �����ϴ� �޼���
    string GetRandomSceneName()
    {
        if (availableScenes.Count == 0)
        {
            Debug.LogWarning("No scenes available to load.");
            return null;
        }

        // ������ �ε����� ����
        int randomIndex = Random.Range(0, availableScenes.Count);
        return availableScenes[randomIndex];
    }

    // ���� �ε�� �Ŀ� ȣ��Ǵ� �ݹ� �޼���
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���� �ε�� �Ŀ� spawn ��ġ�� ã�Ƽ� �÷��̾ �ش� ��ġ�� ��ġ
        PlacePlayerInSpawn();
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

    // Inspector���� ������ �� �ִ� ��� ������ �� ����Ʈ
    public List<string> availableScenes = new List<string>();
}
