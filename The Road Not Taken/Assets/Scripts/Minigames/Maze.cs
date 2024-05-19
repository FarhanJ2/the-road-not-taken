using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    [Header("Devices")]
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioClip song;

    public enum State
    {
        Uninitialized,
        Start,
        FeatherOne,
        FeatherTwo,
        FeatherThree,
        FeatherFour,
        FeatherFive,
        End
    }

    private static Maze instance;

    public static Maze Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Maze>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "Maze";
                    instance = obj.AddComponent<Maze>();
                }
            }
            return instance;
        }
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
    private void Start()
    {
        StoryManager.Instance.StartGame();
        GameState = State.Uninitialized;
    }

    private void OnDestroy()
    {
        // StoryManager.Instance.EndGame();
        // StoryManager.Instance.State = StoryManager.State.NestBuilding;
        StoryManager.Instance.GameState = StoryManager.State.NestBuilding;
    }

    private void CheckForStateChange()
    {
        switch (GameState)
        {
            case State.Uninitialized:
                break;
            case State.Start:
                break;
            case State.FeatherOne:
                break;
            case State.FeatherTwo:
                break;
            case State.FeatherThree:
                break;
            case State.FeatherFour:
                break;
            case State.FeatherFive:
                break;
            case State.End:
                break;
            default:
                break;
        }
    }

    private bool mazeStarted = false;
    public void OnEnterMaze()
    {
        if (!mazeStarted)
        {
            GameState = State.Start;
            StartCoroutine(StartMaze());
            mazeStarted = true;
        }
    }

    public void CheckFeathersCollected()
    {
        if (GameState == State.FeatherFive)
        {
            GameState = State.End;
            Debug.Log("Onwards!");
        }
        else
        {
            Debug.Log("Only " + (int)(State.FeatherFive - GameState) + " more to find!");
        }
    }

    IEnumerator StartMaze()
    {
        PlayerMovement.disabled = true;
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        music.clip = song;
        music.Play();
        PlayerMovement.disabled = false;
    }
}
