using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Option : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;  // TMP_Dropdown UI 요소
    public Toggle fullscreenToggle;          // 전체 화면을 위한 Toggle
    public Slider bgmSlider;
    public Slider seSlider;
    public TextMeshProUGUI bgmText;
    public TextMeshProUGUI seText;

    private void Start()
    {
        // Dropdown 초기화
        InitializeDropdown();

        // 해상도 변경 시 호출되는 함수 연결
        resolutionDropdown.onValueChanged.AddListener(delegate { SetResolution(resolutionDropdown.value); });
        fullscreenToggle.onValueChanged.AddListener(delegate { SetFullscreen(fullscreenToggle.isOn); });

        // 슬라이더 값 변경시 볼륨 설정
        bgmSlider.onValueChanged.AddListener(SetBgmVolume);
        seSlider.onValueChanged.AddListener(SetSEVolume);

        // 초기치 불러오기
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 100f);
        seSlider.value = PlayerPrefs.GetFloat("SEVolume", 100f);
        bgmText.text = Mathf.Floor(bgmSlider.value).ToString();
        seText.text = Mathf.Floor(seSlider.value).ToString();

        // 비활성화
        gameObject.SetActive(false);
    }


    void InitializeDropdown()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string> { "HD (1280x720)", "FHD (1920x1080)" };
        resolutionDropdown.AddOptions(options);
    }

    public void SetResolution(int index)
    {
        switch (index)
        {
            case 0: // HD
                Screen.SetResolution(1280, 720, Screen.fullScreen);
                break;
            case 1: // FHD
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                break;
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetBgmVolume(float volume)
    {
        bgmText.text = Mathf.Floor(volume).ToString();
        MusicManager.instance.SetBGM(volume/bgmSlider.maxValue);
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void SetSEVolume(float volume)
    {
        seText.text = Mathf.Floor(volume).ToString();
        MusicManager.instance.SetSE(volume/seSlider.maxValue);
        PlayerPrefs.SetFloat("SEVolume", volume);
    }

}
