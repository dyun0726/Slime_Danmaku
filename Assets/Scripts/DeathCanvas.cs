using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathCanvas : MonoBehaviour
{
    private static DeathCanvas instance;
    public static DeathCanvas Instance
    {
        get
        {
            return instance;
        }
    }
    private Button confirmButton; // 확인 버튼

    void Awake()
    {
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        confirmButton = GetComponentInChildren<Button>();
    }

    private void Start() {
        transform.gameObject.SetActive(false);
        confirmButton.onClick.AddListener(OnConfirmButtonClick);
    }

    private void OnConfirmButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
        gameObject.SetActive(false);
    }
}
