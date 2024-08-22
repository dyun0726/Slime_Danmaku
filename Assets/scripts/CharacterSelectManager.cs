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

    private int selectedCharacterIndex = 0;
    private int selectedWeaponIndex = 0;

    private bool[] characterUnlocked = new bool[8] { true, false, false, false, false, false, false, false };
    private bool[] weaponUnlocked = new bool[4] { true, true, false, false };

    public Button startButton;

    void Start()
    {   
        // 해금 캐릭터 불러오기 및 적용
        LoadUnlockData();
        // 첫 번째 캐릭터 및 무기를 선택된 상태로 초기화 (옵션)
        SelectCharacter(0);
        SelectWeapon(0);
    }

    public void SelectCharacter(int index)
    {
        if (index < 0 || index >= characters.Length) return;

        // 해금된 캐릭터가 아니면 Start 버튼 비활성화
        selectedCharacterIndex = index;
        startButton.interactable = characterUnlocked[index];
        UpdateCharacterInfo(characterUnlocked[index]);
        HighlightSelectedCharacterButton();
    }

    public void SelectWeapon(int index)
    {
        if (index < 0 || index >= weaponInfos.Length) return;

        selectedWeaponIndex = index;
        startButton.interactable = weaponUnlocked[index];
        UpdateWeaponInfo();
        HighlightSelectedWeaponButton();
    }

    void UpdateCharacterInfo(bool unlocked)
    {
        if (selectedCharacterIndex < 0 || selectedCharacterIndex >= characters.Length) return;

        CharacterInfo character = characters[selectedCharacterIndex];
        nameText.text = character.characterName;
        maxHealthText.text = "HP: " + character.maxHealth;
        csText.text = "CS: " + character.castingSpeed;
        magicText.text = "Magic: " + character.baseMagic;
        speedText.text = "Speed: " + character.moveSpeed;

        maxHealthSlider.value = character.maxHealth;
        magicSlider.value = character.baseMagic;
        csSlider.value = character.castingSpeed;
        speedSlider.value = character.moveSpeed;

        if (unlocked)
        {
            descriptionText.text = character.description;
        }
        else
        {
            descriptionText.text = "해금 조건: " + character.required;
        }
    }

    private void UpdateWeaponInfo()
    {
        if (selectedWeaponIndex < 0 || selectedWeaponIndex >= characters.Length) return;
        WeaponInfo weaponInfo = weaponInfos[selectedWeaponIndex];
        weaponName.text = weaponInfo.weaponName;
        if (weaponUnlocked[selectedWeaponIndex]){
            weaponDescriptionText.text = weaponInfo.description;
        }
        else
        {
            weaponDescriptionText.text = "해금 조건: 근접 공격 100번 시도";
        }
        

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
            if (characterUnlocked[i])
            {
                if (i == selectedCharacterIndex)
                {
                    buttonImage.color = Color.white; // 선택된 버튼 강조 (원래 색상)
                }
                else
                {
                    buttonImage.color = Color.gray; // 기본 버튼 색상 (어두운 색상)
                }
            }
            else
            {
                buttonImage.color = new Color(0.1f, 0.1f, 0.1f);
            }
        }
    }

    private void HighlightSelectedWeaponButton()
    {
        for (int i = 0; i < weaponButtons.Length; i++)
        {
            Image buttonImage = weaponButtons[i];
            if (weaponUnlocked[i])
            {
                if (i == selectedWeaponIndex)
                {
                    buttonImage.color = Color.white; // 선택된 버튼 강조 (원래 색상)
                }
                else
                {
                    buttonImage.color = Color.gray; // 기본 버튼 색상 (어두운 색상)
                }
            }
            else
            {
                buttonImage.color = new Color(0.1f, 0.1f, 0.1f);
            }
        }
    }

    // 시작 버튼 클릭 시 호출되는 메서드
    public void OnStartButtonClicked()
    {
        if (selectedCharacterIndex < 0 || selectedCharacterIndex >= characters.Length)
        {
            Debug.LogError("Invalid character selection: " + selectedCharacterIndex);
            return;
        }

        SceneManager.LoadScene("World1_Start");
        SceneManager.sceneLoaded += OnSceneLoaded; // 콜백 메소드 등록 (다음 씬의 awake, enable, start 실행 후)
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        // 플레이어 활성화
        if (Player.Instance != null) {
            Player.Instance.ActivatePlayer();
            Player.Instance.SetSpriteAnimatorController(characters[selectedCharacterIndex]);
            Player.Instance.SetWeapon(weaponInfos[selectedWeaponIndex].prefab);
        }

        // UI 활성화 및 초기화 (플레이어 매니저와 레벨업 ui 연결 전)
        if (PersistentCanvas.instance != null){
            PersistentCanvas.instance.ActivateCanvas();
            PersistentCanvas.instance.GetComponentInChildren<LevelUp>().Init();
        }

        // 콜백 메소드는 awake랑 start실행 후에 실행됨
        if (PlayerManager.Instance != null) {
            PlayerManager.Instance.Init();
            PlayerManager.Instance.FindCanvas(); // 레벨업 ui
            PlayerManager.Instance.SetPlayerAllStats(characters[selectedCharacterIndex], weaponInfos[selectedWeaponIndex]);
        }

        // 카메라 세팅 confiner
        if (CameraManager.instance != null){
            CameraManager.instance.SetConfiner();
            CameraManager.instance.SetFollow();
        }

        // 탄막 bound 세팅
        if (GameManager.Instance != null){
            GameManager.Instance.Init();
            GameManager.Instance.PlacePlayerInSpawn();
            GameManager.Instance.SetBounds();
            GameManager.Instance.Resume();
        }

        // 풀매니저 활성화
        if (PoolManager.instance != null){
            PoolManager.instance.gameObject.SetActive(true);
        }

        // 뮤직 매니저 세팅
        if (MusicManager.instance != null)
        {
            MusicManager.instance.PlayMusicForWorld(1);
        }

        // 씬 로드 콜백 등록 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadUnlockData()
    {
        // 각 캐릭터가 해금되었는지 확인
        for (int i = 1; i < characterUnlocked.Length; i++)
        {
            characterUnlocked[i] = PlayerPrefs.GetInt("CharacterUnlocked_" + i, 0) == 1;
        }

        // 각 무기가 해금되었는지 확인
        for (int i = 2; i < weaponUnlocked.Length; i++)
        {
            weaponUnlocked[i] = PlayerPrefs.GetInt("WeaponUnlocked_" + i, 0) == 1;
        }
    }
}
