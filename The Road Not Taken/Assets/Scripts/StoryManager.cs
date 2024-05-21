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
        StartGame();
    }

    private void LoadJson()
    {
        try
        {
            string json = File.ReadAllText("./Assets/Scripts/story.json");
            PathDataList dataList = JsonUtility.FromJson<PathDataList>("{\"paths\":" + json + "}");
            foreach (PathData pathData in dataList.paths)
            {
                // Debug.Log("Path A: " + pathData.pathA);
                // Debug.Log("Path B: " + pathData.pathB);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error deserializing JSON: " + e.Message);
        }
    }

    public delegate void OnLevelChange(string levelName);
    public static event OnLevelChange onLevelChange;

    public enum State
    {
        Start,
        Maze,
        ForestSearch,
        FlightPath,
        MusicPuzzle,
        End
    }

    public State GameState
    {
        get { return gameState; } 
        set
        {
            gameState = value;
            Debug.Log("Game state changed to: " + value);
            CheckForStateChange();
        }
    }

    private void CheckForStateChange()
    {
        switch (GameState)
        {
            case State.Start:
                onLevelChange?.Invoke("WELCOME TO THE GAME!");
                break;
            case State.Maze:
                onLevelChange?.Invoke("LEVEL ONE");
                break;
            case State.ForestSearch:
                onLevelChange?.Invoke("LEVEL TWO");
                break;
            case State.FlightPath:
                break;
            case State.MusicPuzzle:
                onLevelChange?.Invoke("LEVEL THREE");
                break;
            case State.End:
                break;
        }
    }

    private State gameState;

    public void StartGame()
    {
        Debug.Log("Game started!");
        GameState = State.Start;
    }

    public void EndGame()
    {
        Debug.Log("Game ended!");
    }
}