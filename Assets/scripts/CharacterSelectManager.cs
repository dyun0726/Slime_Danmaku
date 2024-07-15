using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;
using UnityEngine.UI;
using System;

public class CharacterSelectManager : MonoBehaviour
{

    public CharacterInfo[] characters; // 모든 캐릭터의 정보 배열
    public Image[] characterButtons; // 캐릭터 선택 버튼 배열

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI maxHealthText;
    public TextMeshProUGUI AGIText;
    public TextMeshProUGUI INTText;
    public TextMeshProUGUI SpeedText;


    public Slider maxHealthSlider;
    public Slider AGISlider;
    public Slider INTSlider;
    public Slider SpeedSlider;

    public int selectedCharacterIndex=0 ;

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
        maxHealthText.text = "maxHealth: " + character.maxHealth;
        AGIText.text = "AGI: " + character.AGI;
        INTText.text = "INT: " + character.INT;
        SpeedText.text = "Speed: " + character.SPEED;

        maxHealthSlider.value = character.maxHealth;
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
    public void OnCharacterSelectButtonClicked(int index)
    {
        SelectCharacter(index);
        selectedCharacterIndex=index;
        // 캐릭터 선택 로직 구현


        Debug.Log(index);

        Debug.Log(selectedCharacterIndex);
        Debug.Log("Character selected");
    }

    // 시작 버튼 클릭 시 호출되는 메서드
    public void OnStartButtonClicked()
    {
        if (selectedCharacterIndex < 0 || selectedCharacterIndex >= characters.Length)
        {
            Debug.LogError("Invalid character selection: " + selectedCharacterIndex);
            return;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.selectedCharacter = characters[selectedCharacterIndex];
            //GameManager.Instance.ApplyCharacterStats(GameManager.Instance.selectedCharacter);


            SceneManager.LoadScene("World1_Start");
        }
        else
        {
            Debug.LogError("GameManager instance is null. Make sure the GameManager is properly initialized.");
        }
    }
    /*
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬 로드 콜백 등록 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (GameManager.Instance != null && GameManager.Instance.selectedCharacter != null)
        {
            GameManager.Instance.CreatePlayer(GameManager.Instance.selectedCharacter);
        }
    }
    */
}
