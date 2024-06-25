using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public string[] sceneNames; // �߰��� ���� �̸� �迭
    private Vector3 nextScenePosition = Vector3.zero; // ���� ���� ��ġ�� ��ġ

    private void Start()
    {
        // ù ��° ���� �ε��ϰ� �������� �÷��̾� ���࿡ ���� �ε��մϴ�.
        LoadNextScene();
    }

    public void LoadNextScene()
    {
        // �����ϰ� ���� �� ����
        string nextSceneName = sceneNames[Random.Range(0, sceneNames.Length)];

        // �񵿱� �� �ε�
        StartCoroutine(LoadSceneAndSetPosition(nextSceneName));
    }

    private IEnumerator LoadSceneAndSetPosition(string sceneName)
    {
        // �� �ε带 �񵿱�� ����
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // �� �ε� �Ϸ���� ���
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // �ε�� ���� ��Ʈ ������Ʈ�� ��������
        Scene loadedScene = SceneManager.GetSceneByName(sceneName);
        GameObject[] rootObjects = loadedScene.GetRootGameObjects();

        // �� ��Ʈ ������Ʈ�� ��ġ�� �̵�
        foreach (GameObject rootObject in rootObjects)
        {
            rootObject.transform.position = nextScenePosition;
        }

        // ���� ���� ��ġ�� ��ġ ������Ʈ
        nextScenePosition += new Vector3(20, 0, 0); // ���� ���� ũ�⿡ �°� ����
    }
}
