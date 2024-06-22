using UnityEngine;

public class PianoTile : MonoBehaviour
{
    [SerializeField] private AudioClip pianoNote;
    [SerializeField] private GameObject highlight;
    private AudioSource pianoSound;

    private void Awake()
    {
        gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
        pianoSound = gameObject.AddComponent<AudioSource>();
        highlight.SetActive(false);
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

    public void ToggleHighlight()
    {
        highlight.SetActive(!highlight.activeSelf);
    }
}
