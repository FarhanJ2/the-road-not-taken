using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPuzzle : MonoBehaviour
{
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioClip song;

    [SerializeField] private AudioClip[] pianoNotes;
    [SerializeField] private AudioSource pianoSound;

    [SerializeField] private GameObject boundary;
    [SerializeField] private GameObject boundaryToExit;

    public List<int> sequence = new List<int>();
    private List<int> correctSequence = new List<int>();

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

    private void OnEnable()
    {
        DialogueManager.onDialogueEnd += StartSequence;
    }

    private void OnDisable()
    {
        DialogueManager.onDialogueEnd -= StartSequence;
    }

    private void StartSequence()
    {
        if (GameState == State.Start || GameState == State.SoundOne || GameState == State.SoundTwo || GameState == State.SoundThree || GameState == State.SoundFour)
        {
            GameState = State.SoundOne;
            GenerateSequence();
            StartCoroutine(PlaySequence());
        }
    }

    private void CheckForStateChange()
    {
        switch (GameState)
        {
            case State.Uninitialized:
                break;
            case State.Start:
                // PlayerMovement.disabled = true;
                // StartCoroutine(PianoTutorial());
                Interactable interactable = new Interactable
                {
                    dialogue = new Dialogue
                    {
                        sentences = new string[] { "In this challenege you will have to replicate a tone that will be played shortly", "Go on to each piano key to play the sound", "Be careful, you need a 50% or more to pass!", "Ready?" }
                    }
                };
                interactable.TriggerDialogue();
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
                boundaryToExit.SetActive(false);
                break;
        }
    }

    // private IEnumerator PianoTutorial()
    // {
    //     Interactable interactable = new Interactable
    //     {
    //         dialogue = new Dialogue
    //         {
    //             sentences = new string[] { "" }
    //         }
    //     };
    //     interactable.TriggerDialogue();
    // }

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

    private IEnumerator FadeOutMusic(AudioSource audioSource, float FadeTime)
    {
        Debug.Log("FadeOutMusic started");
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            // Decrease volume proportional to the elapsed time
            audioSource.volume -= startVolume * (Time.deltaTime / FadeTime);
            Debug.Log("Volume: " + audioSource.volume);

            // Ensure volume does not go below zero
            if (audioSource.volume < 0)
            {
                audioSource.volume = 0;
            }

            yield return null; // Wait for the next frame
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
        Debug.Log("FadeOutMusic completed");
    }

    public void OnEnterMusicPuzzle()
    {
        if (GameState == State.Uninitialized)
        {
            FadeOutMusic(music, 3f);
            StoryManager.Instance.GameState = StoryManager.State.MusicPuzzle;
            GameState = State.Start;
            boundary.SetActive(true);
        }
    }

    private void GenerateSequence()
    {
        for (int i = 0; i < Random.Range(2, 5); i++)
        {
            correctSequence.Add(Random.Range(0, pianoNotes.Length));
        }
    }

    private IEnumerator PlaySequence()
    {
        foreach (int note in correctSequence)
        {
            pianoSound.clip = pianoNotes[note];
            pianoSound.Play();
            yield return new WaitForSeconds(1f);
        }
    }

    public void CheckPlayerSequence()
    {
        int correctCount = 0;
        int minLength = Mathf.Min(sequence.Count, correctSequence.Count);
        for (int i = 0; i < minLength; i++)
        {
            if (sequence[i] == correctSequence[i])
            {
                correctCount++;
            }
        }

        float percentageCorrect = (float)correctCount / (float)sequence.Count;
        Debug.Log(percentageCorrect);

        if (percentageCorrect >= 0.65)
        {
            GameState = State.End;
            Debug.Log("Player Passed");

            Interactable interactable = new Interactable
            {
                dialogue = new Dialogue
                {
                    sentences = new string[] { "The gates opening!" }
                }
            };
            interactable.TriggerDialogue();
        }
        else
        {
            sequence.Clear();
            Interactable interactable = new Interactable
            {
                dialogue = new Dialogue
                {
                    sentences = new string[] { "Unfortunately, you did not get more than 50%, redoing!" }
                }
            };
            interactable.TriggerDialogue();

            PlayerStats playerStats = FindObjectOfType<PlayerStats>();
            playerStats.TakeDamage(25);
        }

        switch (GameState)
        {
            case State.Start:
                GameState = State.SoundOne;
                break;
            case State.SoundOne:
                GameState = State.SoundTwo;
                break;
            case State.SoundTwo:
                GameState = State.SoundThree;
                break;
            case State.SoundThree:
                GameState = State.SoundFour;
                break;
            case State.SoundFour:
                GameState = State.End;
                break;
        }
    }
}
