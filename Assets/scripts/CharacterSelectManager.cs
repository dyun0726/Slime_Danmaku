using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{

    public CharacterInfo[] characters; // ��� ĳ������ ���� �迭
    public Image[] characterButtons; // ĳ���� ���� ��ư �迭

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI STRText;
    public TextMeshProUGUI AGIText;
    public TextMeshProUGUI INTText;
    public TextMeshProUGUI SpeedText;


    public Slider STRSlider;
    public Slider AGISlider;
    public Slider INTSlider;
    public Slider SpeedSlider;

    private int selectedCharacterIndex = -1;

    void Start()
    {
        // ù ��° ĳ���͸� ���õ� ���·� �ʱ�ȭ (�ɼ�)
        SelectCharacter(0);
    }

    public void SelectCharacter(int index)
    {
        if (index < 0 || index >= characters.Length) return;

        selectedCharacterIndex = index;
        UpdateCharacterInfo();
        HighlightSelectedButton();
    }

    void UpdateCharacterInfo()
    {
        if (selectedCharacterIndex < 0 || selectedCharacterIndex >= characters.Length) return;

        CharacterInfo character = characters[selectedCharacterIndex];
        nameText.text = character.characterName;
        descriptionText.text = character.description;
        STRText.text = "STR: " + character.STR;
        AGIText.text = "AGI: " + character.AGI;
        INTText.text = "INT: " + character.INT;
        SpeedText.text = "Speed: " + character.SPEED;

        STRSlider.value = character.STR;
        AGISlider.value = character.AGI;
        INTSlider.value = character.INT;
        SpeedSlider.value = character.SPEED;



    }

    void HighlightSelectedButton()
    {
        for (int i = 0; i < characterButtons.Length; i++)
        {
            Image buttonImage = characterButtons[i];
            if (i == selectedCharacterIndex)
            {
                buttonImage.color = Color.white; // ���õ� ��ư ���� (���� ����)
            }
            else
            {
                buttonImage.color = Color.gray; // �⺻ ��ư ���� (��ο� ����)
            }
        }
    }

    // ĳ���� ���� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnCharacterSelectButtonClicked()
    {
        // ĳ���� ���� ���� ����
        Debug.Log("Character selected");
    }

    // ���� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("World1_Start");
    }
}
