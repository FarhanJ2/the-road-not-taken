using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;
    [TextArea(3, 10)]
    public string[] sentences;
}


public class Interactable : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool playOnce = false;
    private bool hasPlayed = false;

    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        if (playOnce && hasPlayed)
        {
            return;
        }

        if (playOnce)
        {
            hasPlayed = true;
        }

        PlayDialogue();
    }

    private void PlayDialogue()
    {
        if (dialogue != null)
        {
            DialogueManager.Instance.StartDialogue(dialogue);
        }
        else
        {
            Debug.LogWarning("Dialogue is not assigned");
        }
    }
}
