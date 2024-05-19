using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : MonoBehaviour
{
    [SerializeField] private int scorePerCollection = 5;
    private SpriteRenderer spriteRenderer;
    public AudioSource pickUpSound;
    private Color lerpColor = Color.white;
    private bool Enabled = true;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    IEnumerator FadeOut()
    {
        enabled = false;
        while (spriteRenderer.color.a > 0.01)
        {
            lerpColor = Color.Lerp(lerpColor, new Color(255, 255, 255, 0), Time.deltaTime * 5);
            spriteRenderer.color = lerpColor;
            yield return null;
        }
        
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!Enabled) return;

        PlayerStats.ChangeScore(scorePerCollection);

        switch (Maze.Instance.GameState)
        {
            case Maze.State.Start:
                Maze.Instance.GameState = Maze.State.FeatherOne;
                break;
            case Maze.State.FeatherOne:
                Maze.Instance.GameState = Maze.State.FeatherTwo;
                break;
            case Maze.State.FeatherTwo:
                Maze.Instance.GameState = Maze.State.FeatherThree;
                break;
            case Maze.State.FeatherThree:
                Maze.Instance.GameState = Maze.State.FeatherFour;
                break;
            case Maze.State.FeatherFour:
                Maze.Instance.GameState = Maze.State.FeatherFive;
                break;
            case Maze.State.FeatherFive:
                Maze.Instance.GameState = Maze.State.End;
                break;
            default:
                break;
        }

        Debug.Log(Maze.Instance.GameState);
        pickUpSound.Play();
        StartCoroutine(FadeOut());
    }
}
