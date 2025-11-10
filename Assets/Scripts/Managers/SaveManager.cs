using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    private string saveFilePath;

    // Persistent data object
    public ScoreData scoreData;

    void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "score.ini");
        LoadOrCreateScore();
    }

    private void LoadOrCreateScore()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            scoreData = JsonUtility.FromJson<ScoreData>(json);
            Debug.Log($"Loaded score from {saveFilePath}");
        }
        else
        {
            scoreData = new ScoreData();
            SaveScore();                                         // create initial file
            Debug.Log("No save found — created new score.ini");
        }
    }

    public void LoadScore()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            scoreData = JsonUtility.FromJson<ScoreData>(json);
            Debug.Log($"Loaded score from {saveFilePath}");
        }
    }

    public void SaveScore()
    {
        string json = JsonUtility.ToJson(scoreData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"Saved score to {saveFilePath}");
    }
}
