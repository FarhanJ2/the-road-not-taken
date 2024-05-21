using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text currScore;
    [SerializeField] private TMP_Text highScore;

    private void Awake()
    {
        PlayerStats.onScoreChange += UpdateScore;
        currScore.text = PlayerStats.Score.ToString();
        highScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        // highScore.text = "N/a"; // grab from somewhere later
    }

    private void OnDisable()
    {
        PlayerStats.onScoreChange -= UpdateScore;
    }

    private void UpdateScore()
    {
        currScore.text = PlayerStats.Score.ToString();
        highScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        // if (PlayerStats.Score > PlayerPrefs.GetInt("HighScore", 0))
        // {
        //     PlayerPrefs.SetInt("HighScore", PlayerStats.Score);
        //     highScore.text = PlayerStats.Score.ToString();
        // }
    }
}
