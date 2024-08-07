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
    private StoryLine[] storyLines;
    private int index = 0;

    private void Start()
    {
        int worldIndex = GetWorldIndex();

        if (worldIndex > 0)
        {
            LoadStory(worldIndex);
            ShowStory();
            storyButton.onClick.AddListener(OnNextButtonClick);
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
        if (index < storyLines.Length)
        {
            nameText.text = storyLines[index].speaker;
            storyText.text = storyLines[index].line;
        }
    }

    private void OnNextButtonClick()
    {
        index++;
        if (index < storyLines.Length)
        {
            nameText.text = storyLines[index].speaker;
            storyText.text = storyLines[index].line;
        }
        else
        {
            gameObject.SetActive(false);
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
