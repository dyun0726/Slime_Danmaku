using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // 게임 시작 버튼 클릭 시 호출되는 메서드
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("CharacterSelect");
    }

    // 옵션 버튼 클릭 시 호출되는 메서드
    public void OnOptionsButtonClicked()
    {
        // 옵션 메뉴 로직 구현
        Debug.Log("Options button clicked");
    }

    // 게임 종료 버튼 클릭 시 호출되는 메서드
    public void OnExitButtonClicked()
    {
        Application.Quit();
        Debug.Log("Game exited");
    }
}
