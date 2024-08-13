using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class StoryCanvas : MonoBehaviour
{
    public TextMeshProUGUI storyText;
    public TextMeshProUGUI nameText;
    public Button storyButton;
    public Button skipButton;
    private StoryLine[] storyLines;
    private int index = 0;

    private void Start()
    {
        int worldIndex = GetWorldIndex();

        if (worldIndex > 0)
        {
            LoadStory(worldIndex);
            ShowStory();
            storyButton.onClick.AddListener(OnStoryButtonClick);
            skipButton.onClick.AddListener(EndStory);

            // 시간 정지
            GameManager.Instance.Stop();
        }
        else
        {
            // 혹시 스테이지 시작 월드가 아닐때
            EndStory();
        }
    }

    private int GetWorldIndex()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "World1_Start": return 1;
            case "World2_Start": return 2;
            case "World3_Start": return 3;
            case "World4_Start": return 4;
            default: return 0;

        }
    }

    private void LoadStory(int worldIndex)
    {
        string filePath = Path.Combine(Application.dataPath, "Json", $"World{worldIndex}.json");
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
        nameText.text = storyLines[index].speaker;
        storyText.text = storyLines[index].line;
    }

    private void EndStory()
    {
        gameObject.SetActive(false);
        GameManager.Instance.Resume();
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

[System.Serializable]
public class StoryLine
{
    public string speaker;
    public string line;
}

[System.Serializable]
public class StoryData
{
    public StoryLine[] worldStories;
}
