using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestSearch : MonoBehaviour
{
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioClip song;

    public enum State
    {
        Uninitialized,
        FoundKey,
        FoundAnotherKey,
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
            case State.FoundKey:
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

    public void AddKey()
    {
        GameState = State.FoundKey;
        CheckForStateChange();
    }
}
