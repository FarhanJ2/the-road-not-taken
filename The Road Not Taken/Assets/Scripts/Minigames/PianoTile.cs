using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PianoTile : MonoBehaviour
{
    // [SerializeField] private AudioSource pianoSound;
    private AudioSource pianoSound;
    [SerializeField] private AudioClip pianoNote;

    private void Awake()
    {
        gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
        pianoSound = gameObject.AddComponent<AudioSource>();
    }

    private int NoteIndex()
    {
        return int.Parse(pianoNote.name.Substring(3)) - 1;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Player")) return;
        pianoSound.clip = pianoNote;
        pianoSound.Play();

        MusicPuzzle.Instance.sequence.Add(NoteIndex());
    }
}
