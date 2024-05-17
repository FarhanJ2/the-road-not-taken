using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : MonoBehaviour
{
    [SerializeField] private int scorePerCollection = 5;

    private void OnTriggerEnter2D(Collider2D col)
    {
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
        Destroy(gameObject);
    }
}
