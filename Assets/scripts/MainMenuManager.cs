using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // ���� ���� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("CharacterSelect");
    }

    // �ɼ� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnOptionsButtonClicked()
    {
        // �ɼ� �޴� ���� ����
        Debug.Log("Options button clicked");
    }

    // ���� ���� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnExitButtonClicked()
    {
        Application.Quit();
        Debug.Log("Game exited");
    }
}
