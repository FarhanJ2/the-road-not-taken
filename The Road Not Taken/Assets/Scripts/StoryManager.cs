using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public class PathData
{
    public string pathA;
    public string pathB;
    public string consquenceA;
    public string consquenceB;
}

[System.Serializable]
public class PathDataList
{
    public PathData[] paths;
}

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        LoadJson();
    }

    private void LoadJson()
    {
        try
        {
            string json = File.ReadAllText("./Assets/Scripts/story.json");
            PathDataList dataList = JsonUtility.FromJson<PathDataList>("{\"paths\":" + json + "}");
            foreach (PathData pathData in dataList.paths)
            {
                Debug.Log("Path A: " + pathData.pathA);
                Debug.Log("Path B: " + pathData.pathB);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error deserializing JSON: " + e.Message);
        }
    }

    public void StartGame()
    {
        Debug.Log("Game started!");
    }

    public void EndGame()
    {
        Debug.Log("Game ended!");
    }
}