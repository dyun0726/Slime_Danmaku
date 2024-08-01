using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;
using UnityEngine.UI;
using System;

public class CharacterSelectManager : MonoBehaviour
{

    public CharacterInfo[] characters; // 모든 캐릭터의 정보 배열
    public Image[] characterButtons; // 캐릭터 선택 버튼 배열

    public WeaponInfo[] weaponInfos; // 무기 정보 배열
    public Image[] weaponButtons; // 무기 선택 버튼 배열

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI maxHealthText;
    public TextMeshProUGUI csText;
    public TextMeshProUGUI magicText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI weaponDescriptionText;
    public TextMeshProUGUI strText;
    public TextMeshProUGUI agiText;


    public Slider maxHealthSlider;
    public Slider magicSlider;
    public Slider csSlider;
    public Slider speedSlider;
    public Slider strSlider;
    public Slider agiSlider;

    public int selectedCharacterIndex = 0;
    private int selectedWeaponIndex = 0;

    void Start()
    {
        // 첫 번째 캐릭터를 선택된 상태로 초기화 (옵션)
       SelectCharacter(0);
       SelectWeapon(0);
    }

    public void SelectCharacter(int index)
    {
        if (index < 0 || index >= characters.Length) return;

        selectedCharacterIndex = index;
        UpdateCharacterInfo();
        HighlightSelectedCharacterButton();

        Debug.Log(selectedCharacterIndex);
        Debug.Log("Character selected: " + characters[selectedCharacterIndex]);
    }

    public void SelectWeapon(int index)
    {
        if (index < 0 || index >= weaponInfos.Length) return;
        selectedWeaponIndex = index;
        UpdateWeaponInfo();
        HighlightSelectedWeaponButton();
    }

    void UpdateCharacterInfo()
    {
        if (selectedCharacterIndex < 0 || selectedCharacterIndex >= characters.Length) return;

        CharacterInfo character = characters[selectedCharacterIndex];
        nameText.text = character.characterName;
        descriptionText.text = character.description;
        maxHealthText.text = "HP: " + character.maxHealth;
        csText.text = "CS: " + character.castingSpeed;
        magicText.text = "Magic: " + character.baseMagic;
        speedText.text = "Speed: " + character.moveSpeed;

        maxHealthSlider.value = character.maxHealth;
        magicSlider.value = character.baseMagic;
        csSlider.value = character.castingSpeed;
        speedSlider.value = character.moveSpeed;


    }

    private void UpdateWeaponInfo()
    {
        if (selectedWeaponIndex < 0 || selectedWeaponIndex >= characters.Length) return;
        WeaponInfo weaponInfo = weaponInfos[selectedWeaponIndex];
        weaponName.text = weaponInfo.weaponName;
        weaponDescriptionText.text = weaponInfo.description;

        strText.text = "STR: " + weaponInfo.strength;
        agiText.text = "AGI: " + weaponInfo.agility;

        strSlider.value = weaponInfo.strength;
        agiSlider.value = weaponInfo.agility;
    }

    void HighlightSelectedCharacterButton()
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

    private void HighlightSelectedWeaponButton()
    {
        for (int i = 0; i < weaponButtons.Length; i++)
        {
            Image buttonImage = weaponButtons[i];
            if (i == selectedWeaponIndex)
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
    // public void OnCharacterSelectButtonClicked(int index)
    // {
    //     SelectCharacter(index);
    //     selectedCharacterIndex=index;
    //     // 캐릭터 선택 로직 구현


    //     Debug.Log(index);
    //     Debug.Log(selectedCharacterIndex);
    //     Debug.Log("Character selected");
    // }

    // 시작 버튼 클릭 시 호출되는 메서드
    public void OnStartButtonClicked()
    {
        if (selectedCharacterIndex < 0 || selectedCharacterIndex >= characters.Length)
        {
            Debug.LogError("Invalid character selection: " + selectedCharacterIndex);
            return;
        }

        SceneManager.LoadScene("World1_Start");
        // 콜백 메소드 등록 (다음 씬의 awake, enable, start 실행 후)
        SceneManager.sceneLoaded += OnSceneLoaded;

        // if (GameManager.Instance != null)
        // {
        //     GameManager.Instance.selectedCharacter = characters[selectedCharacterIndex];
        //     //GameManager.Instance.ApplyCharacterStats(GameManager.Instance.selectedCharacter);


        //     SceneManager.LoadScene("World1_Start");
        // }
        // else
        // {
        //     Debug.LogError("GameManager instance is null. Make sure the GameManager is properly initialized.");
        // }
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 플레이어 활성화
        if (Player.Instance != null) {
            Player.Instance.ActivatePlayer();
            Player.Instance.SetSpriteAnimatorController(characters[selectedCharacterIndex]);
        }

        // 콜백 메소드는 awake랑 start실행 후에 실행됨
        if (PlayerManager.Instance != null) {
            Debug.Log("PlayerManager is loaded");
            PlayerManager.Instance.SetPlayerAllStats(characters[selectedCharacterIndex], weaponInfos[selectedWeaponIndex]);
        }

        // 카메라 세팅 confiner
        if (CameraManager.instance != null){
            CameraManager.instance.SetConfiner();
            CameraManager.instance.SetFollow();
        }

        // 탄막 bound 세팅
        if (GameManager.Instance != null){
            GameManager.Instance.PlacePlayerInSpawn();
            GameManager.Instance.SetBounds();
            GameManager.Instance.Resume();
        }

        // UI 활성화
        if (PersistentCanvas.instance != null){
            PersistentCanvas.instance.ActivateCanvas();
        }

        // 풀매니저 활성화
        if (PoolManager.instance != null){
            PoolManager.instance.gameObject.SetActive(true);
        }

        // 씬 로드 콜백 등록 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
}
