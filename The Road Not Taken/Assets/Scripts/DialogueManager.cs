using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    private static DialogueManager instance;

    private void Start()
    {
        sentences = new Queue<string>();
    }

    public static DialogueManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DialogueManager>();
                if (instance == null)
                {
                    GameObject singleton = new GameObject("DialogueManager");
                    instance = singleton.AddComponent<DialogueManager>();
                }
            }
            return instance;
        }
    }

    // events
    public delegate void OnDialogueChange(string name, string dialogue);
    public static event OnDialogueChange onDialogueChange;

    public delegate void OnDialogueEnd();
    public static event OnDialogueEnd onDialogueEnd;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private string currentName;
    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Starting conversation with " + dialogue.name);

        currentName = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        Debug.Log(sentence);
        onDialogueChange?.Invoke(currentName, sentence);
    }

    private void EndDialogue()
    {
        Debug.Log("End of conversation");
        onDialogueEnd?.Invoke();
    }
}
