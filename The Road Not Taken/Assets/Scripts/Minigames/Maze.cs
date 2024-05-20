using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    [Header("Devices")]
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioClip song;

    [SerializeField] private GameObject gateOpen;
    [SerializeField] private GameObject gateClose;
    [SerializeField] private GameObject boundary;

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

        gateClose.SetActive(true);
        gateOpen.SetActive(false);
        boundary.SetActive(true);
    }

    private void OnDestroy()
    {
        // StoryManager.Instance.EndGame();
        // StoryManager.Instance.State = StoryManager.State.ForestSearch;
        // StoryManager.Instance.GameState = StoryManager.State.ForestSearch;
    }

    private void CheckForStateChange()
    {
        switch (GameState)
        {
            case State.FeatherFive:
                Interactable interactable = new Interactable
                {
                    dialogue = new Dialogue
                    {
                        sentences = new string[] { "You found all the feathers! The gate is open now!" }
                    }
                };
                interactable.TriggerDialogue();
                gateClose.SetActive(false);
                gateOpen.SetActive(true);
                boundary.SetActive(false);
                break;
            case State.End:
                break;
            default:
                break;
        }
    }

    // private bool mazeStarted = false;
    public void OnEnterMaze()
    {
        if (GameState == State.Uninitialized)
        {
            GameState = State.Start;
            StartCoroutine(StartMaze());
            // mazeStarted = true;
        }
    }

    // public void closeGate()
    // {
    //     gateClose.SetActive(true);
    //     gateOpen.SetActive(false);
    //     boundary.SetActive(true);
    // }

    private bool playerPassed = false;
    public void CheckFeathersCollected()
    {
        if (playerPassed) return;

        if (GameState == State.FeatherFive)
        {
            GameState = State.End;
            Interactable interactable = new Interactable
            {
                dialogue = new Dialogue
                {
                    sentences = new string[] { "You found all the feathers! Onwards!" }
                }
            };
            interactable.TriggerDialogue();
            playerPassed = true;
            Destroy(this); // end of this scripts usefullness
        }
        else
        {
            Interactable interactable = new Interactable
            {
                dialogue = new Dialogue
                {
                    sentences = new string[] { "You need all feathers to pass. Currently you need " + (int)(State.FeatherFive - GameState) + " more!" }
                }
            };
            interactable.TriggerDialogue();
            Debug.Log("Only " + (int)(State.FeatherFive - GameState) + " more to find!");
            playerPassed = false;
        }
    }

    IEnumerator StartMaze()
    {
        PlayerMovement.disabled = true;
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        music.clip = song;
        music.Play();
        PlayerMovement.disabled = false;
        yield return new WaitForSeconds(1f);
        StoryManager.Instance.GameState = StoryManager.State.Maze;
    }
}
