using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPuzzle : MonoBehaviour
{
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioClip song;

    [SerializeField] private GameObject boundary;

    public enum State
    {
        Uninitialized,
        Start,
        SoundOne,
        SoundTwo,
        SoundThree,
        SoundFour,
        End
    }

    private State gameState = State.Uninitialized;
    public State GameState
    {
        get { return gameState; }
        set
        {
            gameState = value;
            CheckForStateChange();
        }
    }

    private void CheckForStateChange()
    {
        switch (GameState)
        {
            case State.Uninitialized:
                break;
            case State.Start:
                break;
            case State.SoundOne:
                break;
            case State.SoundTwo:
                break;
            case State.SoundThree:
                break;
            case State.SoundFour:
                break;
            case State.End:
                break;
        }
    }

    private static MusicPuzzle instance;
    public static MusicPuzzle Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MusicPuzzle>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "MusicPuzzle";
                    instance = obj.AddComponent<MusicPuzzle>();
                }
            }
            return instance;
        }
    }

    public void OnEnterMusicPuzzle()
    {
        if (GameState == State.Uninitialized)
        {
            StoryManager.Instance.GameState = StoryManager.State.ForestSearch;
            GameState = State.Start;
            boundary.SetActive(true);
        }
    }
}
