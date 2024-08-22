using UnityEngine;

public class GameQuit : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();

        // 에디터에서 실행시 에디터 종료
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
