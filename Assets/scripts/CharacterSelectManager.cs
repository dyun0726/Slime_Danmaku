using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{

    public CharacterInfo[] characters; // 모든 캐릭터의 정보 배열
    public Image[] characterButtons; // 캐릭터 선택 버튼 배열

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
        // 첫 번째 캐릭터를 선택된 상태로 초기화 (옵션)
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
                buttonImage.color = Color.white; // 선택된 버튼 강조 (원래 색상)
            }
            else
            {
                buttonImage.color = Color.gray; // 기본 버튼 색상 (어두운 색상)
            }
        }
    }

    // 캐릭터 선택 버튼 클릭 시 호출되는 메서드
    public void OnCharacterSelectButtonClicked()
    {
        // 캐릭터 선택 로직 구현
        Debug.Log("Character selected");
    }

    // 시작 버튼 클릭 시 호출되는 메서드
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("World1_Start");
    }
}
