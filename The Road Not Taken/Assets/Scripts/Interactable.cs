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
    public Dialogue dialogue;

    public void TriggerDialogue()
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
