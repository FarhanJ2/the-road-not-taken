using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ForestSearch : MonoBehaviour
{
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioClip song;

    [SerializeField] private GameObject boundaryBack;
    [SerializeField] private GameObject boundaryForward;

    public enum State
    {
        Uninitialized,
        Start,
        FoundKey,
        FoundAnotherKey,
        UsedKey,
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
            case State.FoundKey:
                Interactable interactable = new Interactable
                {
                    dialogue = new Dialogue
                    {
                        sentences = new string[] { "You found a key! Maybe you can find more for a special reward!" }
                    }
                };
                interactable.TriggerDialogue();
                break;
            case State.FoundAnotherKey:
                break;
            case State.End:
                break;
        }
    }

    private static ForestSearch instance;
    public static ForestSearch Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ForestSearch>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "ForestSearch";
                    instance = obj.AddComponent<ForestSearch>();
                }
            }
            return instance;
        }
    }

    public void OnEnterForest()
    {
        if (GameState == State.Uninitialized)
        {
            StoryManager.Instance.GameState = StoryManager.State.ForestSearch;
            GameState = State.Start;
            boundaryBack.SetActive(true);

            music.clip = song;
            music.Play();
        }
    }

    public void CheckKey()
    {
        if (GameState == State.Start)
        {
            Interactable interactable = new Interactable
            {
                dialogue = new Dialogue
                {
                    sentences = new string[] { "Hmmm, I need a key to open this gate...", "I wonder where I can find one..." }
                }
            };
            interactable.TriggerDialogue();
        }

        if (GameState == State.FoundKey || GameState == State.FoundAnotherKey)
        {
            GameState = State.UsedKey;
            boundaryForward.SetActive(false);
            Interactable interactable = new Interactable
                {
                    dialogue = new Dialogue
                    {
                        sentences = new string[] { "Amazing, I am so close to home!" }
                    }
                };
                interactable.TriggerDialogue();
        }
    }
}