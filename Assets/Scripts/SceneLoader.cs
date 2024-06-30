using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public string[] sceneNames; // 추가한 씬의 이름 배열
    private Vector3 nextScenePosition = Vector3.zero; // 다음 씬이 배치될 위치

    private void Start()
    {
        // 첫 번째 씬을 로드하고 나머지는 플레이어 진행에 따라 로드합니다.
        LoadNextScene();
    }

    public void LoadNextScene()
    {
        // 랜덤하게 다음 씬 선택
        string nextSceneName = sceneNames[Random.Range(0, sceneNames.Length)];

        // 비동기 씬 로드
        StartCoroutine(LoadSceneAndSetPosition(nextSceneName));
    }

    private IEnumerator LoadSceneAndSetPosition(string sceneName)
    {
        // 씬 로드를 비동기로 시작
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // 씬 로드 완료까지 대기
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // 로드된 씬의 루트 오브젝트들 가져오기
        Scene loadedScene = SceneManager.GetSceneByName(sceneName);
        GameObject[] rootObjects = loadedScene.GetRootGameObjects();

        // 각 루트 오브젝트의 위치를 이동
        foreach (GameObject rootObject in rootObjects)
        {
            rootObject.transform.position = nextScenePosition;
        }

        // 다음 씬이 배치될 위치 업데이트
        nextScenePosition += new Vector3(20, 0, 0); // 씬의 가로 크기에 맞게 조정
    }
}
