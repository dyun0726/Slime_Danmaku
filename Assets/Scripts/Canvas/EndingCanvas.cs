using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class EndingCanvas : MonoBehaviour
{
    private GameObject myPanel;
    private GameObject otherPanel;
    private TextMeshProUGUI storyText;
    private TextMeshProUGUI myText;
    private TextMeshProUGUI otherText;
    public Button storyButton;
    public Button skipButton;
    private StoryLine[] storyLines;
    private int index = 0;

    private void Awake() {
        TextMeshProUGUI[] TMPros = GetComponentsInChildren<TextMeshProUGUI>();
        storyText = TMPros[0];
        myText = TMPros[1];
        otherText = TMPros[2];
        myPanel = myText.transform.parent.gameObject;
        otherPanel = otherText.transform.parent.gameObject;

        storyButton.onClick.AddListener(OnStoryButtonClick);
        skipButton.onClick.AddListener(EndStory);
    }

    private void Start()
    {
        LoadStory();
        ShowStory();

        // 시간 정지
        GameManager.Instance.Stop();
    }

    private void LoadStory()
    {
        string filePath = Path.Combine(Application.dataPath, "Json", $"Ending.json");
        if (File.Exists(filePath))
        {
            string jsonContent = File.ReadAllText(filePath);
            StoryData storyData = JsonUtility.FromJson<StoryData>(jsonContent);
            storyLines = storyData.worldStories;
        }
        else
        {
            storyLines = new StoryLine[] { new StoryLine { speaker = "System", line = "Story not found." } };
        }
    }

    private void ShowStory()
    {
        if (storyLines[index].speaker == "나")
        {
            myPanel.SetActive(true);
            myText.text = storyLines[index].speaker;
            otherPanel.SetActive(false);
        }
        else
        {
            otherPanel.SetActive(true);
            otherText.text = storyLines[index].speaker;
            myPanel.SetActive(false);
        }
        
        storyText.text = storyLines[index].line;
    }

    private void EndStory()
    {
        GameManager.Instance.gameClear = true;
        GameManager.Instance.Resume();
        PlayerManager.Instance.GameClear();


        gameObject.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }

    private void OnStoryButtonClick()
    {
        index++;
        if (index < storyLines.Length)
        {
            ShowStory();
        }
        else
        {
            EndStory();
        }
    }
}
