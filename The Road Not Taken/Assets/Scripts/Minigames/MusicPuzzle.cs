using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private PianoTile[] pianoTiles; 

    public List<int> sequence = new List<int>();
    private List<int> correctSequence = new List<int>();

    private string dialogueId;
    private bool isDialogueDone;
    private bool IsDialogueDone
    {
        get
        {
            bool val = isDialogueDone;
            isDialogueDone = false;
            return val;
        }
        set => isDialogueDone = value;
    }

    public enum State
    {
        Uninitialized,
        Start,
        Tutorial,
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
        DialogueManager.onDialogueEnd += HandleDialogueEnd;
    }

    private void OnDisable()
    {
        DialogueManager.onDialogueEnd -= HandleDialogueEnd;
    }
    private void HandleDialogueEnd(string id)
    {
        dialogueId = id;
        IsDialogueDone = true;
    }

    private void StartSequence()
    {
        if (GameState == State.Start || GameState == State.SoundOne || GameState == State.SoundTwo ||
            GameState == State.SoundThree || GameState == State.SoundFour)
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
            case State.Tutorial:
                StartCoroutine(PianoTutorial());
                break;
            case State.Start:
                StartSequence();
                break;
            case State.SoundOne:
            case State.SoundTwo:
            case State.SoundThree:
            case State.SoundFour:
                break;
            case State.End:
                boundaryToExit.SetActive(false);
                break;
        }
    }

    private IEnumerator PianoTutorial()
    {
        Interactable interactable = new Interactable
        {
            dialogue = new Dialogue
            {
                id = "Tutorial Dialogue 1",
                sentences = new string[]
                {
                    "This is the final step to achieve freedom",
                    "In this puzzle you will have to listen to a series of notes and do your best to replicate it",
                    "To replicate the sound, you will go over each piano key and replay the audio you have heard",
                    "Here is a sample of what they sound like",
                }
            }
        };
        interactable.TriggerDialogue();

        DialogueManager.onDialogueEnd += (id) =>
        {
            
        };
        
        yield return new WaitUntil(() => IsDialogueDone && dialogueId == "Tutorial Dialogue 1");
        Debug.Log("Testing");

        for (int i = 0; i < pianoTiles.Length + 1; i++)
        {
            if (i == pianoTiles.Length)
            {
                pianoTiles[i - 1].ToggleHighlight();
                break;
            }
            if (i != 0 )
                pianoTiles[i - 1].ToggleHighlight();
            Debug.Log("Playing Note" + i);
            pianoTiles[i].ToggleHighlight();
            pianoSound.clip = pianoNotes[i];
            pianoSound.Play();
            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(5f);
        
        Interactable interactable2 = new Interactable
        {
            dialogue = new Dialogue
            {
                id = "Tutorial Dialogue 2",
                sentences = new string[]
                {
                    "Be careful, each time you fail, you will take damage!",
                    "You can always use the replay button if you missed a note",
                    "Ready?"
                }
            }
        };
        interactable2.TriggerDialogue();

        yield return new WaitUntil(() => IsDialogueDone && dialogueId == "Tutorial Dialogue 2");
        GameState = State.Start;
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
                    GameObject obj = new GameObject
                    {
                        name = "MusicPuzzle"
                    };
                    instance = obj.AddComponent<MusicPuzzle>();
                }
            }

            return instance;
        }
    }

    private IEnumerator FadeOutMusic(AudioSource audioSource, float fadeTime)
    {
        // Debug.Log("FadeOutMusic started");
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            // Decrease volume proportional to the elapsed time
            audioSource.volume -= startVolume * (Time.deltaTime / fadeTime);
            // Debug.Log("Volume: " + audioSource.volume);

            // Ensure volume does not go below zero
            if (audioSource.volume < 0)
            {
                audioSource.volume = 0;
            }

            yield return null; // wait for the next frame
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
        audioSource.enabled = false;
        // Debug.Log("FadeOutMusic completed");
    }

    public void OnEnterMusicPuzzle()
    {
        if (GameState == State.Uninitialized)
        {
            StartCoroutine(FadeOutMusic(music, 3f));
            StoryManager.Instance.GameState = StoryManager.State.MusicPuzzle;
            GameState = State.Tutorial;
            boundary.SetActive(true);
        }
    }

    private void GenerateSequence()
    {
        correctSequence.Clear();

        for (int i = 0; i < Random.Range(2, 4); i++)
        {
            correctSequence.Add(Random.Range(0, pianoNotes.Length));
        }
    }

    public void ReplaySequence()
    {
        StartCoroutine(PlaySequence());
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

    public void ResetSequence() {
        sequence.Clear();
    }

    public void CheckPlayerSequence()
    {
        if (GameState == State.End) return;
        
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
                    sentences = new string[] { "The gates opening!", "I'm finally back home." }
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

    public void EndGame()
    {
        StoryManager.Instance.GameState = StoryManager.State.End;
    }
}